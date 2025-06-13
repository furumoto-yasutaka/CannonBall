/*******************************************************************************
*
*	�^�C�g���F	���ۓ��͏��(2�����x�N�g��)�X�N���v�g
*	�t�@�C���F	NewInputAction_Vector2.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/09/21
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NewInputAction_Vector2 : NewInputAction
{
    #region enum

    /// <summary> Joycon2�����x�N�g�����̓��X�g </summary>
    public enum JoyconVector2Id
    {
        Dpad = 0,
        Stick,

        Length,
    }

    /// <summary> Pad2�����x�N�g�����̓��X�g </summary>
    public enum PadVector2Id
    {
        Dpad = 0,
        LeftStick,
        RightStick,

        Length,
    }

    #endregion


    /// <summary> �֐��|�C���^�̌^�錾 </summary>
    public delegate void GetInput(int deviceId, int i);


    /// <summary> Joycon�̃L�[�ݒ� </summary>
    [SerializeField]
    private List<JoyconVector2Id> m_joyconActionIdList_Inspector = new List<JoyconVector2Id>();

    /// <summary> Pad�̃L�[�ݒ� </summary>
    [SerializeField]
    private List<PadVector2Id> m_padActionIdList_Inspector = new List<PadVector2Id>();

    /// <summary> �e�L�[�̓��͏� </summary>
    private List<Vector2>[] m_inputInfoList = new List<Vector2>[DeviceInfo.m_DeviceNum];

    /// <summary> Joycon�p�{�^���̍X�V���\�b�h </summary>
    private List<GetInput> m_joyconGetInputMethodList = new List<GetInput>();

    /// <summary> Pad�p�{�^���̍X�V���\�b�h </summary>
    private List<GetInput> m_padGetInputMethodList = new List<GetInput>();


    public override void Init()
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            m_inputInfoList[i] = new List<Vector2>();
        }

        foreach (JoyconVector2Id id in m_joyconActionIdList_Inspector)
        {
            switch (id)
            {
                case JoyconVector2Id.Dpad:     m_joyconGetInputMethodList.Add(Update_Joycon_Dpad); break;
                case JoyconVector2Id.Stick:     m_joyconGetInputMethodList.Add(Update_Joycon_Stick); break;
                default: continue;
            }

            for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
            {
                m_inputInfoList[i].Add(Vector2.zero);
            }
        }

        foreach (PadVector2Id id in m_padActionIdList_Inspector)
        {
            switch (id)
            {
                case PadVector2Id.Dpad:         m_padGetInputMethodList.Add(Update_Pad_Dpad); break;
                case PadVector2Id.LeftStick:    m_padGetInputMethodList.Add(Update_Pad_LeftStick); break;
                case PadVector2Id.RightStick:   m_padGetInputMethodList.Add(Update_Pad_RightStick); break;
                default: continue;
            }

            for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
            {
                m_inputInfoList[i].Add(Vector2.zero);
            }
        }
    }

    public override void Update()
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            if (JoyconManager.Instance.GetIsEntry(i))
            {
                for (int j = 0; j < m_joyconActionIdList_Inspector.Count; j++)
                {
                    m_joyconGetInputMethodList[j](i, j);
                }
            }
            else
            {
                for (int j = 0; j < m_joyconActionIdList_Inspector.Count; j++)
                {
                    Update_InputReset(i, j);
                }
            }

            if (PadManager.Instance.GetIsEntry(i))
            {
                for (int j = 0; j < m_padActionIdList_Inspector.Count; j++)
                {
                    m_padGetInputMethodList[j](i, m_joyconActionIdList_Inspector.Count + j);
                }
            }
            else
            {
                for (int j = 0; j < m_padActionIdList_Inspector.Count; j++)
                {
                    Update_InputReset(i, m_joyconActionIdList_Inspector.Count + j);
                }
            }
        }
    }

    public override void Reset()
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            for (int j = 0; j < m_joyconActionIdList_Inspector.Count; j++)
            {
                Update_InputReset(i, j);
            }
            for (int j = 0; j < m_padActionIdList_Inspector.Count; j++)
            {
                Update_InputReset(i, m_joyconActionIdList_Inspector.Count + j);
            }
        }
    }

    protected override void Update_InputReset(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i] = Vector2.zero;
    }


    //--------------------�擾����--------------------

    /// <summary>
    /// ���͒l���擾(����̃f�o�C�X����)
    /// </summary>
    /// <param name="deviceId"> ���b�N����O���[�v�̏�� </param>
    public Vector2 GetVec2(int deviceId)
    {
        Vector2 value = Vector2.zero;

        int i = 0;
        foreach (Vector2 info in m_inputInfoList[deviceId])
        {
            i++;
            if (value.sqrMagnitude < info.sqrMagnitude)
            {
                value = info;
            }
        }

        return value;
    }

    /// <summary>
    /// ���͒l���擾(�S�Ẵf�o�C�X����)
    /// </summary>
    public Vector2 GetVec2All()
    {
        Vector2 value = Vector2.zero;

        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            foreach (Vector2 info in m_inputInfoList[i])
            {
                if (value.sqrMagnitude < info.sqrMagnitude)
                {
                    value = info;
                }
            }
        }

        return value;
    }


    //--------------------�X�V����(Joycon)--------------------

    private void Update_Joycon_Dpad(int deviceId, int i)
    {
        Joycon jc = JoyconManager.Instance.GetDevice(deviceId);
        Vector2 v;
        v.x = (jc.GetButton(Joycon.Button.DPAD_LEFT) ? -1.0f : 0.0f) + (jc.GetButton(Joycon.Button.DPAD_RIGHT) ? 1.0f : 0.0f);
        v.y = (jc.GetButton(Joycon.Button.DPAD_DOWN) ? -1.0f : 0.0f) + (jc.GetButton(Joycon.Button.DPAD_UP) ? 1.0f : 0.0f);
        m_inputInfoList[deviceId][i] = v;
    }

    private void Update_Joycon_Stick(int deviceId, int i)
    {
        Joycon jc = JoyconManager.Instance.GetDevice(deviceId);
        m_inputInfoList[deviceId][i] = new Vector2(jc.GetStick()[0], jc.GetStick()[1]);
    }


    //--------------------�X�V����(Pad)--------------------

    private void Update_Pad_Dpad(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i] = PadManager.Instance.GetDevice(deviceId).dpad.value;
    }

    private void Update_Pad_LeftStick(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i] = PadManager.Instance.GetDevice(deviceId).leftStick.value;
    }

    private void Update_Pad_RightStick(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i] = PadManager.Instance.GetDevice(deviceId).rightStick.value;
    }
}
