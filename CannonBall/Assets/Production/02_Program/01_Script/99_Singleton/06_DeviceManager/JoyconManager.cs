/*******************************************************************************
*
*	�^�C�g���F	Joycon�Ǘ��V���O���g���X�N���v�g
*	�t�@�C���F	JoyconManager.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/09/18
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

public class JoyconManager : SingletonMonoBehaviour<JoyconManager>
{
    /// <summary> Joycon�̏� </summary>
    [SerializeField, CustomLabelReadOnly("Joycon�̐ڑ���"), CustomArrayLabel(typeof(PlayerNumber))]
    private int[] m_joyconConnectInfo = new int[DeviceInfo.m_DeviceNum];

    /// <summary> �v���C���[���Ƃ�Joycon��� </summary>
    private DeviceInfo_Joycon[] m_joyconInfos = new DeviceInfo_Joycon[DeviceInfo.m_DeviceNum];

    /// <summary> Joycon�X�V�p���X�g </summary>
    private List<Joycon> m_tempJoycon;

    /// <summary> �f�o�C�X�ڑ����R�[���o�b�N </summary>
    private UnityEvent m_addDeviceCallBack = new UnityEvent();
    /// <summary> �f�o�C�X�ؒf���R�[���o�b�N </summary>
    private UnityEvent m_removeDeviceCallBack = new UnityEvent();

    /// <summary> OnValidate�����s�\���ǂ��� </summary>
    private bool m_isCanOnValidate = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
        // ���s���ȊO�ł̕ύX�͎󂯕t���Ȃ�
        if (!EditorApplication.isPlaying || !m_isCanOnValidate)
        {
            return;
        }

        DeviceInfo_Joycon[] temp = new DeviceInfo_Joycon[DeviceInfo.m_DeviceNum];

        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            int id = m_joyconInfos[i].m_IsEntry ? m_joyconInfos[i].m_Device.deviceId : -1;
            if (m_joyconConnectInfo[i] != id)
            {
                temp[i] = m_joyconInfos[SearchInfoNum(m_joyconConnectInfo[i])];
            }
            else
            {
                temp[i] = m_joyconInfos[i];
            }
        }

        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            temp[i].m_PlayerId = i;
            m_joyconInfos[i] = temp[i];
        }
    }
#endif

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            m_joyconInfos[i] = new DeviceInfo_Joycon(i);
            m_joyconConnectInfo[i] = -1;
        }

        m_tempJoycon = new List<Joycon>();

        for (int i = 0; i < JoyconConnector.Instance.m_JoyconList.Count; i++)
        {
            AddJoycon(JoyconConnector.Instance.m_JoyconList[i], i);
            m_joyconConnectInfo[i] = JoyconConnector.Instance.m_JoyconList[i].deviceId;
        }

        m_isCanOnValidate = true;
    }

    private void Update()
    {
        if (!JoyconConnector.Instance.m_IsRun) { return; }

        // ���ݐڑ�����Ă���R���g���[���[��ID�������X�g�ɒǉ�
        for (int i = 0; i < JoyconConnector.Instance.m_JoyconList.Count; i++)
        {
            m_tempJoycon.Add(JoyconConnector.Instance.m_JoyconList[i]);
        }

        // �ڑ��ς݂̃R���g���[���[��S�Ē��ׁA
        // �ڑ�����������Ă���f�o�C�X���폜����
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            if (m_joyconInfos[i].m_IsEntry)
            {
                Joycon joycon = m_joyconInfos[i].m_Device;
                // �R���g���[���[���܂����݂��Ă��邩�T������
                int elemIndex = IndexOfDeviceId_FromTempDevice(joycon.deviceId);

                if (elemIndex == -1)
                {// �ؒf����Ă���
                    // �Y���̃R���g���[���[��������������
                    RemoveJoycon(i);
                }
                else
                {// �ڑ����p������Ă���
                    // ���ɘA�g�ς݂̃R���g���[���[�Ȃ̂ŉ����X�g����폜����
                    m_tempJoycon.RemoveAt(elemIndex);
                }
            }
        }

        // tempDevice�Ɏc�����v�f�͐V�����ڑ����ꂽ�R���g���[���[�Ȃ̂œo�^����
        for (int i = 0; i < m_tempJoycon.Count; i++)
        {
            int emptyIndex = IndexOfEmptyDevice();

            // �v���C���[�ɋ󂫂�����Γo�^��F�߂�
            if (emptyIndex != -1)
            {
                AddJoycon(m_tempJoycon[i], emptyIndex);
            }
            else
            {
                // �󂫂��Ȃ��ꍇ�͑����Ă��Ӗ����Ȃ��̂�
                break;
            }
        }

        m_tempJoycon.Clear();
    }

    /// <summary>
    /// �f�o�C�X�o�^
    /// </summary>
    private void AddJoycon(Joycon device, int playerId)
    {
        m_joyconInfos[playerId].m_IsEntry = true;
        m_joyconInfos[playerId].m_Device = device;
        m_addDeviceCallBack.Invoke();
        m_joyconInfos[playerId].m_AddDevicePartsCallBack.Invoke();
    }

    /// <summary>
    /// �f�o�C�X�폜
    /// </summary>
    private void RemoveJoycon(int playerId)
    {
        m_joyconInfos[playerId].m_IsEntry = false;
        m_joyconInfos[playerId].m_Device = null;
        m_removeDeviceCallBack.Invoke();
        m_joyconInfos[playerId].m_RemoveDevicePartsCallBack.Invoke();
    }

    /// <summary>
    /// �f�o�C�X�擾
    /// </summary>
    public Joycon GetDevice(int playerId)
    {
        if (playerId >= DeviceInfo.m_DeviceNum) { return null; }

        if (m_joyconInfos[playerId].m_IsEntry)
        {
            return m_joyconInfos[playerId].m_Device;
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
        return m_joyconInfos[playerId].m_IsEntry;
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
        m_joyconInfos[playerId].m_AddDevicePartsCallBack.AddListener(action);
    }

    /// <summary>
    /// �Y���v���C���[�f�o�C�X�o�^���R�[���o�b�N�폜
    /// </summary>
    public void Add_RemoveDevicePartsCallBack(UnityAction action, int playerId)
    {
        m_joyconInfos[playerId].m_RemoveDevicePartsCallBack.AddListener(action);
    }

    /// <summary>
    /// �Y���v���C���[�f�o�C�X�폜���R�[���o�b�N�o�^
    /// </summary>
    public void Remove_AddDevicePartsCallBack(UnityAction action, int playerId)
    {
        m_joyconInfos[playerId].m_AddDevicePartsCallBack.RemoveListener(action);
    }

    /// <summary>
    /// �Y���v���C���[�f�o�C�X�폜���R�[���o�b�N�폜
    /// </summary>
    public void Remove_RemoveDevicePartsCallBack(UnityAction action, int playerId)
    {
        m_joyconInfos[playerId].m_RemoveDevicePartsCallBack.RemoveListener(action);
    }


    /// <summary>
    /// �f�o�C�X�ԍ��Ō���(�f�o�C�X�̕��ёւ��ł̂ݎg�p)
    /// </summary>
    private int SearchInfoNum(int deviceId)
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            int id = m_joyconInfos[i].m_IsEntry ? m_joyconInfos[i].m_Device.deviceId : -1;
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
        for (int i = 0; i < m_joyconInfos.Length; i++)
        {
            if (m_joyconInfos[i].m_Device.deviceId == deviceId)
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
        for (int i = 0; i < m_tempJoycon.Count; i++)
        {
            if (m_tempJoycon[i].deviceId == deviceId)
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
        for (int i = 0; i < m_joyconInfos.Length; i++)
        {
            if (!m_joyconInfos[i].m_IsEntry)
            {
                return i;
            }
        }

        return -1;
    }

    public int GetDeviceCount()
    {
        int count = 0;

        for (int i = 0; i < m_joyconInfos.Length; i++)
        {
            if (m_joyconInfos[i].m_IsEntry)
            {
                count++;
            }
        }

        return count;
    }
}
