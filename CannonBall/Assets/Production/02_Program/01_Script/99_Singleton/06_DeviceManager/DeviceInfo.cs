/*******************************************************************************
*
*	タイトル：	デバイス情報スクリプト
*	ファイル：	DeviceInfo.cs
*	作成者：	古本 泰隆
*	制作日：    2023/09/23
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class DeviceInfo
{
    /// <summary> 最大デバイス数 </summary>
    public const int m_DeviceNum = 4;

    /// <summary> どのプレイヤーに割り当てるか </summary>
    public int m_PlayerId = 0;
    /// <summary> 登録済みか </summary>
    public bool m_IsEntry = false;
    /// <summary> 該当プレイヤーデバイス接続時コールバック </summary>
    public UnityEvent m_AddDevicePartsCallBack = new UnityEvent();
    /// <summary> 該当プレイヤーデバイス切断時コールバック </summary>
    public UnityEvent m_RemoveDevicePartsCallBack = new UnityEvent();
}

public class DeviceInfo_Joycon : DeviceInfo
{
    /// <summary> デバイス </summary>
    public Joycon m_Device = null;

    public DeviceInfo_Joycon(int id)
    {
        m_PlayerId = id;
    }
}

public class DeviceInfo_Pad : DeviceInfo
{
    /// <summary> デバイス </summary>
    public Gamepad m_Device = null;

    public DeviceInfo_Pad(int id)
    {
        m_PlayerId = id;
    }
}
