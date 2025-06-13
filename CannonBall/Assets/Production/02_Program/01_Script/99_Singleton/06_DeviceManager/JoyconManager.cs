/*******************************************************************************
*
*	タイトル：	Joycon管理シングルトンスクリプト
*	ファイル：	JoyconManager.cs
*	作成者：	古本 泰隆
*	制作日：    2023/09/18
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
    /// <summary> Joyconの状況 </summary>
    [SerializeField, CustomLabelReadOnly("Joyconの接続状況"), CustomArrayLabel(typeof(PlayerNumber))]
    private int[] m_joyconConnectInfo = new int[DeviceInfo.m_DeviceNum];

    /// <summary> プレイヤーごとのJoycon情報 </summary>
    private DeviceInfo_Joycon[] m_joyconInfos = new DeviceInfo_Joycon[DeviceInfo.m_DeviceNum];

    /// <summary> Joycon更新用リスト </summary>
    private List<Joycon> m_tempJoycon;

    /// <summary> デバイス接続時コールバック </summary>
    private UnityEvent m_addDeviceCallBack = new UnityEvent();
    /// <summary> デバイス切断時コールバック </summary>
    private UnityEvent m_removeDeviceCallBack = new UnityEvent();

    /// <summary> OnValidateを実行可能かどうか </summary>
    private bool m_isCanOnValidate = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
        // 実行中以外での変更は受け付けない
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

        // 現在接続されているコントローラーのIDを仮リストに追加
        for (int i = 0; i < JoyconConnector.Instance.m_JoyconList.Count; i++)
        {
            m_tempJoycon.Add(JoyconConnector.Instance.m_JoyconList[i]);
        }

        // 接続済みのコントローラーを全て調べ、
        // 接続が解除されているデバイスを削除する
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            if (m_joyconInfos[i].m_IsEntry)
            {
                Joycon joycon = m_joyconInfos[i].m_Device;
                // コントローラーがまだ存在しているか探索する
                int elemIndex = IndexOfDeviceId_FromTempDevice(joycon.deviceId);

                if (elemIndex == -1)
                {// 切断されている
                    // 該当のコントローラー情報を初期化する
                    RemoveJoycon(i);
                }
                else
                {// 接続が継続されている
                    // 既に連携済みのコントローラーなので仮リストから削除する
                    m_tempJoycon.RemoveAt(elemIndex);
                }
            }
        }

        // tempDeviceに残った要素は新しく接続されたコントローラーなので登録する
        for (int i = 0; i < m_tempJoycon.Count; i++)
        {
            int emptyIndex = IndexOfEmptyDevice();

            // プレイヤーに空きがあれば登録を認める
            if (emptyIndex != -1)
            {
                AddJoycon(m_tempJoycon[i], emptyIndex);
            }
            else
            {
                // 空きがない場合は続けても意味がないので
                break;
            }
        }

        m_tempJoycon.Clear();
    }

    /// <summary>
    /// デバイス登録
    /// </summary>
    private void AddJoycon(Joycon device, int playerId)
    {
        m_joyconInfos[playerId].m_IsEntry = true;
        m_joyconInfos[playerId].m_Device = device;
        m_addDeviceCallBack.Invoke();
        m_joyconInfos[playerId].m_AddDevicePartsCallBack.Invoke();
    }

    /// <summary>
    /// デバイス削除
    /// </summary>
    private void RemoveJoycon(int playerId)
    {
        m_joyconInfos[playerId].m_IsEntry = false;
        m_joyconInfos[playerId].m_Device = null;
        m_removeDeviceCallBack.Invoke();
        m_joyconInfos[playerId].m_RemoveDevicePartsCallBack.Invoke();
    }

    /// <summary>
    /// デバイス取得
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
    /// デバイスが登録されているかを返す
    /// </summary>
    public bool GetIsEntry(int playerId)
    {
        return m_joyconInfos[playerId].m_IsEntry;
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
        m_joyconInfos[playerId].m_AddDevicePartsCallBack.AddListener(action);
    }

    /// <summary>
    /// 該当プレイヤーデバイス登録時コールバック削除
    /// </summary>
    public void Add_RemoveDevicePartsCallBack(UnityAction action, int playerId)
    {
        m_joyconInfos[playerId].m_RemoveDevicePartsCallBack.AddListener(action);
    }

    /// <summary>
    /// 該当プレイヤーデバイス削除時コールバック登録
    /// </summary>
    public void Remove_AddDevicePartsCallBack(UnityAction action, int playerId)
    {
        m_joyconInfos[playerId].m_AddDevicePartsCallBack.RemoveListener(action);
    }

    /// <summary>
    /// 該当プレイヤーデバイス削除時コールバック削除
    /// </summary>
    public void Remove_RemoveDevicePartsCallBack(UnityAction action, int playerId)
    {
        m_joyconInfos[playerId].m_RemoveDevicePartsCallBack.RemoveListener(action);
    }


    /// <summary>
    /// デバイス番号で検索(デバイスの並び替えでのみ使用)
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

        Debug.LogError("デバイスの情報の並び替えでエラーが発生しました");
        return 0;
    }

    /// <summary>
    /// 指定したデバイスIdが存在するかDeviceInfosから探索
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
    /// 指定したデバイスIdが存在するかTempDeviceから探索
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
    /// 未登録の枠が空いているか探索
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
