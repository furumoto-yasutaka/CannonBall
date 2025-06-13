/*******************************************************************************
*
*	�^�C�g���F	���ۓ��͏��(3�����x�N�g��)�X�N���v�g
*	�t�@�C���F	NewInputAction_Vector3.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/09/21
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NewInputAction_Vector3 : NewInputAction
{
    #region enum

    /// <summary> Joycon3�����x�N�g�����̓��X�g </summary>
    public enum JoyconVector3Id
    {
        Gyro = 0,
        Accel,

        Length,
    }

    /// <summary> Pad3�����x�N�g�����̓��X�g </summary>
    public enum PadVector3Id
    {
        Length = 0,
    }

    #endregion


    /// <summary> �֐��|�C���^�̌^�錾 </summary>
    public delegate void GetInput(int deviceId, int i);


    /// <summary> Joycon�̃L�[�ݒ� </summary>
    [SerializeField]
    private List<JoyconVector3Id> m_joyconActionIdList_Inspector = new List<JoyconVector3Id>();

    /// <summary> Pad�̃L�[�ݒ� </summary>
    [SerializeField]
    private List<PadVector3Id> m_padActionIdList_Inspector = new List<PadVector3Id>();

    /// <summary> �e�L�[�̓��͏� </summary>
    private List<Vector3>[] m_inputInfoList = new List<Vector3>[DeviceInfo.m_DeviceNum];

    /// <summary> Joycon�p�{�^���̍X�V���\�b�h </summary>
    private List<GetInput> m_joyconGetInputMethodList = new List<GetInput>();


    public override void Init()
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            m_inputInfoList[i] = new List<Vector3>();
        }

        foreach (JoyconVector3Id id in m_joyconActionIdList_Inspector)
        {
            switch (id)
            {
                case JoyconVector3Id.Gyro:  m_joyconGetInputMethodList.Add(Update_Joycon_Gyro); break;
                case JoyconVector3Id.Accel: m_joyconGetInputMethodList.Add(Update_Joycon_Accel); break;
                default: continue;
            }

            for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
            {
                m_inputInfoList[i].Add(Vector2.zero);
            }
        }

        foreach (PadVector3Id id in m_padActionIdList_Inspector)
        {
            switch (id)
            {
                default: continue;
            }

            //for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
            //{
            //    m_inputInfoList[i].Add(Vector2.zero);
            //}
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
                    //m_padGetInputMethodList[j](i, m_joyconActionIdList_Inspector.Count + j);
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
        m_inputInfoList[deviceId][i] = Vector3.zero;
    }


    //--------------------�擾����--------------------

    /// <summary>
    /// ���������擾(����̃f�o�C�X����)
    /// </summary>
    /// <param name="deviceId"> ���b�N����O���[�v�̏�� </param>
    public Vector3 GetVec3(int deviceId)
    {
        Vector3 value = Vector3.zero;

        foreach (Vector3 info in m_inputInfoList[deviceId])
        {
            if (value.sqrMagnitude < info.sqrMagnitude)
            {
                value = info;
            }
        }

        return value;
    }

    /// <summary>
    /// ���������擾(�S�Ẵf�o�C�X����)
    /// </summary>
    public Vector3 GetVec3All()
    {
        Vector3 value = Vector3.zero;

        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            foreach (Vector3 info in m_inputInfoList[i])
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

    private void Update_Joycon_Gyro(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i] = JoyconManager.Instance.GetDevice(deviceId).GetGyro();
    }

    private void Update_Joycon_Accel(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i] = JoyconManager.Instance.GetDevice(deviceId).GetAccel();
    }
}
