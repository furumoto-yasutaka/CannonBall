/*******************************************************************************
*
*	タイトル：	入力受付制御シングルトンスクリプト
*	ファイル：	InputGroupManager.cs
*	作成者：	古本 泰隆
*	制作日：    2023/04/16
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;

#region support class

/// <summary> 入力受付グループ </summary>
[System.Serializable]
public class InputGroup
{
    /// <summary> グループ名 </summary>
    [CustomLabel("グループ名")]
    public string m_Name;
    /// <summary> 入力受付しているか </summary>
    [HideInInspector]
    public bool m_IsCanInput;
    /// <summary> グループに紐づくウィンドウ </summary>
    [CustomLabel("対応するウィンドウスクリプト")]
    public Window m_Window;
    /// <summary> ウィンドウで使用するインプットアクションマップ名 </summary>
    [CustomLabel("インプットアクション名")]
    public NewInputActionMapName m_inputActionMapName;
    /// <summary> 制御対象のクラス </summary>
    [Header("※ウィンドウはこの中に含めなくてもOK\n" +
        "(含めてしまった場合でもエラーにはなりません)")]
    public InputElement[] m_Elemants;
}

/// <summary> ロック解除予定グループ情報 </summary>
public class OrderInfo
{
    /// <summary> 対象のグループ </summary>
    public InputGroup m_Group;
    /// <summary> 要素単位で反映するか </summary>
    public bool m_IsRefChild;

    public OrderInfo(InputGroup group, bool isRefChild)
    {
        m_Group = group;
        m_IsRefChild = isRefChild;
    }
}

#endregion


public class InputGroupManager : SingletonMonoBehaviour<InputGroupManager>
{
    #region field

    [Header("一番上の要素が最初にロック解除状態になります")]

    /// <summary> ロックが解除されているグループ名 </summary>
    [SerializeField, CustomLabel("ロックが解除されているグループ名")]
    private string m_unlockGroupName = "";

    /// <summary> インプットアクションマップ </summary>
    [SerializeField]
    private List<NewInputActionMap> m_actionMapList = new List<NewInputActionMap>();

    /// <summary> インスペクターから入力可能にするための一時変数 </summary>
    [SerializeField]
    private InputGroup[] m_inputGroupListInspector;

    /// <summary> ロック状態変更に使う情報 </summary>
    private OrderInfo m_lockOrderInfo = null;

    /// <summary> ロック状態変更に使う情報 </summary>
    private OrderInfo m_unlockOrderInfo = null;

    /// <summary> 各グループの情報 </summary>
    private Dictionary<string, InputGroup> m_inputGroupList = new Dictionary<string, InputGroup>();

    /// <summary> 入力のロック解除状態のグループ名 </summary>
    private InputGroup m_UnlockGroup;

    #endregion

    #region function

    /// <summary>
    /// 他のスクリプトの初期化を行うため、
    /// 最速で実行できるように優先度が-100に上がっています
    /// </summary>
    protected override void Awake()
    {
        // シーン遷移で削除されるようにする
        dontDestroyOnLoad = false;

        base.Awake();

        // InputActionMapを初期化する
        foreach (NewInputActionMap map in m_actionMapList)
        {
            map.Init();
        }

        // リストの初期化
        foreach (InputGroup info in m_inputGroupListInspector)
        {
            m_inputGroupList.Add(info.m_Name, info);

            // ウィンドウの初期化
            info.m_Window.InitializeParam(this, info);

            // 入力を持つ要素の初期化
            foreach (InputElement elem in info.m_Elemants)
            {
                elem.InitializeParam(this, info);
            }

            // アクションマップの初期化
            info.m_Window.InitInputActionMap(m_actionMapList.FirstOrDefault(map => map.m_mapName == info.m_inputActionMapName.ToString()));
        }

        // 最初のグループのロックを解除する
        if (m_inputGroupListInspector.Length > 0)
        {
            UnlockInputGroup(new OrderInfo(m_inputGroupListInspector[0], false));
        }
    }

    /// <summary>
    /// 最速で実行できるように優先度が-100に上がっています
    /// </summary>
    private void Update()
    {
        // ロック状況の変化を反映
        if (m_lockOrderInfo != null)
        {
            LockInputGroup(m_lockOrderInfo);
            m_lockOrderInfo = null;
        }
        if (m_unlockOrderInfo != null)
        {
            UnlockInputGroup(m_unlockOrderInfo);
            m_unlockOrderInfo = null;
        }

        // InputActionMapに登録されている全てのInputActionを更新する
        foreach (NewInputActionMap map in m_actionMapList)
        {
            if (map.m_IsEnable)
            {
                map.Update();
            }
            else
            {
                map.Reset();
            }
        }
    }


    /// <summary>
    /// グループ単位で入力をロック
    /// </summary>
    /// <param name="info"> ロックするグループの情報 </param>
    private void LockInputGroup(OrderInfo info)
    {
        InputGroup group = info.m_Group;

        group.m_IsCanInput = false;
        group.m_Window.DisableInputActionMap();

        m_unlockGroupName = "";

        if (info.m_IsRefChild)
        {
            group.m_Window.LockInput();
            foreach (InputElement elem in group.m_Elemants)
            {
                elem.LockInput();
            }
        }
    }

    /// <summary>
    /// グループ単位で入力のロック解除
    /// </summary>
    /// <param name="info"> ロックするグループの情報 </param>
    private void UnlockInputGroup(OrderInfo info)
    {
        InputGroup group = info.m_Group;

        group.m_IsCanInput = true;
        group.m_Window.EnableInputActionMap();

        m_UnlockGroup = info.m_Group;
        m_unlockGroupName = info.m_Group.m_Name;

        if (info.m_IsRefChild)
        {
            group.m_Window.UnlockInput();
            foreach (InputElement elem in group.m_Elemants)
            {
                elem.UnlockInput();
            }
        }
    }

    /// <summary>
    /// ロックを要請する
    /// </summary>
    /// <param name="isRefChild"> 要素単位で反映するか </param>
    public void LockInputGroupOrder(bool isRefChild)
    {
        if (m_UnlockGroup == null) { return; }

        // 先に要請が無ければ登録する
        if (m_lockOrderInfo == null)
        {
            m_lockOrderInfo = new OrderInfo(m_UnlockGroup, isRefChild);
        }
    }

    /// <summary>
    /// ロック解除を要請する
    /// </summary>
    /// <param name="group"> 対象グループ </param>
    /// <param name="isRefChild"> 要素単位で反映するか </param>
    public void UnlockInputGroupOrder(InputGroup group, bool isRefChild)
    {
        // 先に要請が無ければ登録する
        if (m_unlockOrderInfo == null)
        {
            m_unlockOrderInfo = new OrderInfo(group, isRefChild);
        }
    }

    #endregion
}
