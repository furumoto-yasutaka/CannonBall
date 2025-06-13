/*******************************************************************************
*
*	�^�C�g���F	���ۓ��͏��(�g���K�[)�X�N���v�g
*	�t�@�C���F	NewInputAction_Trigger.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/09/21
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NewInputAction_Trigger : NewInputAction
{
    #region enum

    /// <summary> Joycon�g���K�[���X�g </summary>
    public enum JoyconTriggerId
    {
        Length = 0,
    }

    /// <summary> Pad�g���K�[���X�g </summary>
    public enum PadTriggerId
    {
        LeftTrigger = 0,
        RightTrigger,

        Length,
    }

    #endregion


    /// <summary> �֐��|�C���^�̌^�錾 </summary>
    public delegate void GetInput(int deviceId, int i);


    /// <summary> Joycon�̃L�[�ݒ� </summary>
    [SerializeField]
    private List<JoyconTriggerId> m_joyconActionIdList_Inspector = new List<JoyconTriggerId>();

    /// <summary> Pad�̃L�[�ݒ� </summary>
    [SerializeField]
    private List<PadTriggerId> m_padActionIdList_Inspector = new List<PadTriggerId>();

    /// <summary> �e�L�[�̓��͏� </summary>
    private List<float>[] m_inputInfoList = new List<float>[DeviceInfo.m_DeviceNum];

    /// <summary> Pad�p�{�^���̍X�V���\�b�h </summary>
    private List<GetInput> m_padGetInputMethodList = new List<GetInput>();


    public override void Init()
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            m_inputInfoList[i] = new List<float>();
        }

        foreach (JoyconTriggerId id in m_joyconActionIdList_Inspector)
        {
            switch (id)
            {
                default: continue;
            }

            //for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
            //{
            //    m_inputInfoList[i].Add(0.0f);
            //}
        }

        foreach (PadTriggerId id in m_padActionIdList_Inspector)
        {
            switch (id)
            {
                case PadTriggerId.LeftTrigger:  m_padGetInputMethodList.Add(Update_Pad_LeftTrigger); break;
                case PadTriggerId.RightTrigger: m_padGetInputMethodList.Add(Update_Pad_RightTrigger); break;
                default: continue;
            }

            for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
            {
                m_inputInfoList[i].Add(0.0f);
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
                    //m_joyconGetInputMethodList[j](i, j, m_joyconGetInputArgList[j]);
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
        m_inputInfoList[deviceId][i] = 0.0f;
    }


    //--------------------�擾����--------------------

    /// <summary>
    /// ���������擾(����̃f�o�C�X����)
    /// </summary>
    /// <param name="deviceId"> ���b�N����O���[�v�̏�� </param>
    public float GetTrigger(int deviceId)
    {
        float value = 0.0f;

        foreach (float info in m_inputInfoList[deviceId])
        {
            if (value < info)
            {
                value = info;
            }
        }

        return value;
    }

    /// <summary>
    /// ���������擾(�S�Ẵf�o�C�X����)
    /// </summary>
    public float GetTriggerAll()
    {
        float value = 0.0f;

        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            foreach (float info in m_inputInfoList[i])
            {
                if (value < info)
                {
                    value = info;
                }
            }
        }

        return value;
    }


    //--------------------�X�V����(Pad)--------------------

    private void Update_Pad_LeftTrigger(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i] = PadManager.Instance.GetDevice(deviceId).leftTrigger.value;
    }

    private void Update_Pad_RightTrigger(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i] = PadManager.Instance.GetDevice(deviceId).rightTrigger.value;
    }
}
