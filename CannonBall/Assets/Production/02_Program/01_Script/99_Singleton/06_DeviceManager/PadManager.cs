/*******************************************************************************
*
*	タイトル：	Pad管理シングルトンスクリプト
*	ファイル：	PadManager.cs
*	作成者：	古本 泰隆
*	制作日：    2023/09/23
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
    /// <summary> Padの状況 </summary>
    [SerializeField, CustomLabelReadOnly("Padの接続状況"), CustomArrayLabel(typeof(PlayerNumber))]
    private int[] m_padConnectInfo = new int[DeviceInfo.m_DeviceNum];

    /// <summary> プレイヤーごとのPad情報 </summary>
    private DeviceInfo_Pad[] m_padInfos = new DeviceInfo_Pad[DeviceInfo.m_DeviceNum];

    /// <summary> Pad更新用リスト </summary>
    private List<Gamepad> m_tempPad;

    /// <summary> デバイス接続時コールバック </summary>
    private UnityEvent m_addDeviceCallBack = new UnityEvent();
    /// <summary> デバイス切断時コールバック </summary>
    private UnityEvent m_removeDeviceCallBack = new UnityEvent();

    /// <summary> OnValidateを実行可能かどうか </summary>
    private bool m_isCanOnValidate = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
        // 実行中以外または初期化前での変更は受け付けない
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
        // 現在接続されているコントローラーのIDを仮リストに追加
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            m_tempPad.Add(Gamepad.all[i]);
        }

        // 接続済みのコントローラーを全て調べ、
        // 接続が解除されているデバイスを削除する
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            if (m_padInfos[i].m_IsEntry)
            {
                Gamepad pad = m_padInfos[i].m_Device;
                // コントローラーがまだ存在しているか探索する
                int elemIndex = IndexOfDeviceId_FromTempDevice(pad.deviceId);

                if (elemIndex == -1)
                {//=====切断されている
                 // 該当のコントローラー情報を初期化する
                    RemovePad(i);
                }
                else
                {//=====接続が継続されている
                 // 既に連携済みのコントローラーなので仮リストから削除する
                    m_tempPad.RemoveAt(elemIndex);
                }
            }
        }

        // tempDeviceに残った要素は新しく接続されたコントローラーなので登録する
        for (int i = 0; i < m_tempPad.Count; i++)
        {
            int emptyIndex = IndexOfEmptyDevice();

            // プレイヤーに空きがあれば登録を認める
            if (emptyIndex != -1)
            {
                AddPad(m_tempPad[i], emptyIndex);
            }
            else
            {
                // 空きがない場合は続けても意味がないので
                break;
            }
        }

        m_tempPad.Clear();
    }

    /// <summary>
    /// デバイス登録
    /// </summary>
    private void AddPad(Gamepad device, int playerId)
    {
        m_padInfos[playerId].m_IsEntry = true;
        m_padInfos[playerId].m_Device = device;
        m_addDeviceCallBack.Invoke();
        m_padInfos[playerId].m_AddDevicePartsCallBack.Invoke();
    }

    /// <summary>
    /// デバイス削除
    /// </summary>
    private void RemovePad(int playerId)
    {
        m_padInfos[playerId].m_IsEntry = false;
        m_padInfos[playerId].m_Device = null;
        m_removeDeviceCallBack.Invoke();
        m_padInfos[playerId].m_RemoveDevicePartsCallBack.Invoke();
    }

    /// <summary>
    /// デバイス取得
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
    /// デバイスが登録されているかを返す
    /// </summary>
    public bool GetIsEntry(int playerId)
    {
        return m_padInfos[playerId].m_IsEntry;
    }

    /// <summary>
    /// デバイス登録時コールバック登録
    /// </summary>
    public void Add_AddDeviceCallBack(UnityAction action)
    {
        m_addDeviceCallBack.AddListener(action);
    }

    /// <summary>
    /// デバイス削除時コールバック登録
    /// </summary>
    public void Add_RemoveDeviceCallBack(UnityAction action)
    {
        m_removeDeviceCallBack.AddListener(action);
    }

    /// <summary>
    /// デバイス登録時コールバック削除
    /// </summary>
    public void Remove_AddDeviceCallBack(UnityAction action)
    {
        m_addDeviceCallBack.RemoveListener(action);
    }

    /// <summary>
    /// デバイス削除時コールバック削除
    /// </summary>
    public void Remove_RemoveDeviceCallBack(UnityAction action)
    {
        m_removeDeviceCallBack.RemoveListener(action);
    }

    /// <summary>
    /// 該当プレイヤーデバイス登録時コールバック登録
    /// </summary>
    public void Add_AddDevicePartsCallBack(UnityAction action, int playerId)
    {
        m_padInfos[playerId].m_AddDevicePartsCallBack.AddListener(action);
    }

    /// <summary>
    /// 該当プレイヤーデバイス登録時コールバック削除
    /// </summary>
    public void Add_RemoveDevicePartsCallBack(UnityAction action, int playerId)
    {
        m_padInfos[playerId].m_RemoveDevicePartsCallBack.AddListener(action);
    }

    /// <summary>
    /// 該当プレイヤーデバイス削除時コールバック登録
    /// </summary>
    public void Remove_AddDevicePartsCallBack(UnityAction action, int playerId)
    {
        m_padInfos[playerId].m_AddDevicePartsCallBack.RemoveListener(action);
    }

    /// <summary>
    /// 該当プレイヤーデバイス削除時コールバック削除
    /// </summary>
    public void Remove_RemoveDevicePartsCallBack(UnityAction action, int playerId)
    {
        m_padInfos[playerId].m_RemoveDevicePartsCallBack.RemoveListener(action);
    }


    /// <summary>
    /// デバイス番号で検索(デバイスの並び替えでのみ使用)
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

        Debug.LogError("デバイスの情報の並び替えでエラーが発生しました");
        return 0;
    }

    /// <summary>
    /// 指定したデバイスIdが存在するかDeviceInfosから探索
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
    /// 指定したデバイスIdが存在するかTempDeviceから探索
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
    /// 未登録の枠が空いているか探索
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
