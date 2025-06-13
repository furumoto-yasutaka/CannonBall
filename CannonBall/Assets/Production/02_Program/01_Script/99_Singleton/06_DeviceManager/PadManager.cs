/*******************************************************************************
*
*	�^�C�g���F	Pad�Ǘ��V���O���g���X�N���v�g
*	�t�@�C���F	PadManager.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/09/23
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PadManager : SingletonMonoBehaviour<PadManager>
{
    /// <summary> Pad�̏� </summary>
    [SerializeField, CustomLabelReadOnly("Pad�̐ڑ���"), CustomArrayLabel(typeof(PlayerNumber))]
    private int[] m_padConnectInfo = new int[DeviceInfo.m_DeviceNum];

    /// <summary> �v���C���[���Ƃ�Pad��� </summary>
    private DeviceInfo_Pad[] m_padInfos = new DeviceInfo_Pad[DeviceInfo.m_DeviceNum];

    /// <summary> Pad�X�V�p���X�g </summary>
    private List<Gamepad> m_tempPad;

    /// <summary> �f�o�C�X�ڑ����R�[���o�b�N </summary>
    private UnityEvent m_addDeviceCallBack = new UnityEvent();
    /// <summary> �f�o�C�X�ؒf���R�[���o�b�N </summary>
    private UnityEvent m_removeDeviceCallBack = new UnityEvent();

    /// <summary> OnValidate�����s�\���ǂ��� </summary>
    private bool m_isCanOnValidate = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
        // ���s���ȊO�܂��͏������O�ł̕ύX�͎󂯕t���Ȃ�
        if (!EditorApplication.isPlaying || !m_isCanOnValidate)
        {
            return;
        }

        DeviceInfo_Pad[] temp = new DeviceInfo_Pad[DeviceInfo.m_DeviceNum];

        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            int id = m_padInfos[i].m_IsEntry ? m_padInfos[i].m_Device.deviceId : -1;
            if (m_padConnectInfo[i] != id)
            {
                temp[i] = m_padInfos[SearchInfoNum(m_padConnectInfo[i])];
            }
            else
            {
                temp[i] = m_padInfos[i];
            }
        }

        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            temp[i].m_PlayerId = i;
            m_padInfos[i] = temp[i];
        }
    }
#endif

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            m_padInfos[i] = new DeviceInfo_Pad(i);
            m_padConnectInfo[i] = -1;
        }

        m_tempPad = new List<Gamepad>();

        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            AddPad(Gamepad.all[i], i + JoyconManager.Instance.GetDeviceCount());
            m_padConnectInfo[i] = Gamepad.all[i].deviceId;
        }

        m_isCanOnValidate = true;
    }

    private void Update()
    {
        // ���ݐڑ�����Ă���R���g���[���[��ID�������X�g�ɒǉ�
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            m_tempPad.Add(Gamepad.all[i]);
        }

        // �ڑ��ς݂̃R���g���[���[��S�Ē��ׁA
        // �ڑ�����������Ă���f�o�C�X���폜����
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            if (m_padInfos[i].m_IsEntry)
            {
                Gamepad pad = m_padInfos[i].m_Device;
                // �R���g���[���[���܂����݂��Ă��邩�T������
                int elemIndex = IndexOfDeviceId_FromTempDevice(pad.deviceId);

                if (elemIndex == -1)
                {//=====�ؒf����Ă���
                 // �Y���̃R���g���[���[��������������
                    RemovePad(i);
                }
                else
                {//=====�ڑ����p������Ă���
                 // ���ɘA�g�ς݂̃R���g���[���[�Ȃ̂ŉ����X�g����폜����
                    m_tempPad.RemoveAt(elemIndex);
                }
            }
        }

        // tempDevice�Ɏc�����v�f�͐V�����ڑ����ꂽ�R���g���[���[�Ȃ̂œo�^����
        for (int i = 0; i < m_tempPad.Count; i++)
        {
            int emptyIndex = IndexOfEmptyDevice();

            // �v���C���[�ɋ󂫂�����Γo�^��F�߂�
            if (emptyIndex != -1)
            {
                AddPad(m_tempPad[i], emptyIndex);
            }
            else
            {
                // �󂫂��Ȃ��ꍇ�͑����Ă��Ӗ����Ȃ��̂�
                break;
            }
        }

        m_tempPad.Clear();
    }

    /// <summary>
    /// �f�o�C�X�o�^
    /// </summary>
    private void AddPad(Gamepad device, int playerId)
    {
        m_padInfos[playerId].m_IsEntry = true;
        m_padInfos[playerId].m_Device = device;
        m_addDeviceCallBack.Invoke();
        m_padInfos[playerId].m_AddDevicePartsCallBack.Invoke();
    }

    /// <summary>
    /// �f�o�C�X�폜
    /// </summary>
    private void RemovePad(int playerId)
    {
        m_padInfos[playerId].m_IsEntry = false;
        m_padInfos[playerId].m_Device = null;
        m_removeDeviceCallBack.Invoke();
        m_padInfos[playerId].m_RemoveDevicePartsCallBack.Invoke();
    }

    /// <summary>
    /// �f�o�C�X�擾
    /// </summary>
    public Gamepad GetDevice(int playerId)
    {
        if (playerId >= DeviceInfo.m_DeviceNum) { return null; }

        if (m_padInfos[playerId].m_IsEntry)
        {
            return m_padInfos[playerId].m_Device;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// �f�o�C�X���o�^����Ă��邩��Ԃ�
    /// </summary>
    public bool GetIsEntry(int playerId)
    {
        return m_padInfos[playerId].m_IsEntry;
    }

    /// <summary>
    /// �f�o�C�X�o�^���R�[���o�b�N�o�^
    /// </summary>
    public void Add_AddDeviceCallBack(UnityAction action)
    {
        m_addDeviceCallBack.AddListener(action);
    }

    /// <summary>
    /// �f�o�C�X�폜���R�[���o�b�N�o�^
    /// </summary>
    public void Add_RemoveDeviceCallBack(UnityAction action)
    {
        m_removeDeviceCallBack.AddListener(action);
    }

    /// <summary>
    /// �f�o�C�X�o�^���R�[���o�b�N�폜
    /// </summary>
    public void Remove_AddDeviceCallBack(UnityAction action)
    {
        m_addDeviceCallBack.RemoveListener(action);
    }

    /// <summary>
    /// �f�o�C�X�폜���R�[���o�b�N�폜
    /// </summary>
    public void Remove_RemoveDeviceCallBack(UnityAction action)
    {
        m_removeDeviceCallBack.RemoveListener(action);
    }

    /// <summary>
    /// �Y���v���C���[�f�o�C�X�o�^���R�[���o�b�N�o�^
    /// </summary>
    public void Add_AddDevicePartsCallBack(UnityAction action, int playerId)
    {
        m_padInfos[playerId].m_AddDevicePartsCallBack.AddListener(action);
    }

    /// <summary>
    /// �Y���v���C���[�f�o�C�X�o�^���R�[���o�b�N�폜
    /// </summary>
    public void Add_RemoveDevicePartsCallBack(UnityAction action, int playerId)
    {
        m_padInfos[playerId].m_RemoveDevicePartsCallBack.AddListener(action);
    }

    /// <summary>
    /// �Y���v���C���[�f�o�C�X�폜���R�[���o�b�N�o�^
    /// </summary>
    public void Remove_AddDevicePartsCallBack(UnityAction action, int playerId)
    {
        m_padInfos[playerId].m_AddDevicePartsCallBack.RemoveListener(action);
    }

    /// <summary>
    /// �Y���v���C���[�f�o�C�X�폜���R�[���o�b�N�폜
    /// </summary>
    public void Remove_RemoveDevicePartsCallBack(UnityAction action, int playerId)
    {
        m_padInfos[playerId].m_RemoveDevicePartsCallBack.RemoveListener(action);
    }


    /// <summary>
    /// �f�o�C�X�ԍ��Ō���(�f�o�C�X�̕��ёւ��ł̂ݎg�p)
    /// </summary>
    private int SearchInfoNum(int deviceId)
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            int id = m_padInfos[i].m_IsEntry ? m_padInfos[i].m_Device.deviceId : -1;
            if (deviceId == id)
            {
                return i;
            }
        }

        Debug.LogError("�f�o�C�X�̏��̕��ёւ��ŃG���[���������܂���");
        return 0;
    }

    /// <summary>
    /// �w�肵���f�o�C�XId�����݂��邩DeviceInfos����T��
    /// </summary>
    public int IndexOfDeviceId_FromDeviceInfos(int deviceId)
    {
        for (int i = 0; i < m_padInfos.Length; i++)
        {
            if (m_padInfos[i].m_Device.deviceId == deviceId)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// �w�肵���f�o�C�XId�����݂��邩TempDevice����T��
    /// </summary>
    private int IndexOfDeviceId_FromTempDevice(int deviceId)
    {
        for (int i = 0; i < m_tempPad.Count; i++)
        {
            if (m_tempPad[i].deviceId == deviceId)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// ���o�^�̘g���󂢂Ă��邩�T��
    /// </summary>
    private int IndexOfEmptyDevice()
    {
        for (int i = 0; i < m_padInfos.Length; i++)
        {
            if (!m_padInfos[i].m_IsEntry)
            {
                return i;
            }
        }

        return -1;
    }

    public int GetDeviceCount()
    {
        int count = 0;

        for (int i = 0; i < m_padInfos.Length; i++)
        {
            if (m_padInfos[i].m_IsEntry)
            {
                count++;
            }
        }

        return count;
    }
}
