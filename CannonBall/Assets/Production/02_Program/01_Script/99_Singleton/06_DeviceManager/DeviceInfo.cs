/*******************************************************************************
*
*	�^�C�g���F	�f�o�C�X���X�N���v�g
*	�t�@�C���F	DeviceInfo.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/09/23
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class DeviceInfo
{
    /// <summary> �ő�f�o�C�X�� </summary>
    public const int m_DeviceNum = 4;

    /// <summary> �ǂ̃v���C���[�Ɋ��蓖�Ă邩 </summary>
    public int m_PlayerId = 0;
    /// <summary> �o�^�ς݂� </summary>
    public bool m_IsEntry = false;
    /// <summary> �Y���v���C���[�f�o�C�X�ڑ����R�[���o�b�N </summary>
    public UnityEvent m_AddDevicePartsCallBack = new UnityEvent();
    /// <summary> �Y���v���C���[�f�o�C�X�ؒf���R�[���o�b�N </summary>
    public UnityEvent m_RemoveDevicePartsCallBack = new UnityEvent();
}

public class DeviceInfo_Joycon : DeviceInfo
{
    /// <summary> �f�o�C�X </summary>
    public Joycon m_Device = null;

    public DeviceInfo_Joycon(int id)
    {
        m_PlayerId = id;
    }
}

public class DeviceInfo_Pad : DeviceInfo
{
    /// <summary> �f�o�C�X </summary>
    public Gamepad m_Device = null;

    public DeviceInfo_Pad(int id)
    {
        m_PlayerId = id;
    }
}
