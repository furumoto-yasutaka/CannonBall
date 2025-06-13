/*******************************************************************************
*
*	タイトル：	キーコンフィグ管理スクリプト
*	ファイル：	NewInputActionMap.cs
*	作成者：	古本 泰隆
*	制作日：    2023/09/21
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInputActionMap", menuName = "CreateNewInputActionMap")]
public class NewInputActionMap : ScriptableObject
{
    /// <summary> マップの名前 </summary>
    public string m_mapName = "";

    /// <summary> InputAction(Inspector入力用) </summary>
    public List<NewInputAction_Button> m_inputActionList_Button = new List<NewInputAction_Button>();
    public List<NewInputAction_Trigger> m_inputActionList_Trigger = new List<NewInputAction_Trigger>();
    public List<NewInputAction_Vector2> m_inputActionList_Vector2 = new List<NewInputAction_Vector2>();
    public List<NewInputAction_Vector3> m_inputActionList_Vector3 = new List<NewInputAction_Vector3>();

    /// <summary> InputAction(参照用) </summary>
    private Dictionary<string, NewInputAction_Button> m_inputActionDictionaly_Button
        = new Dictionary<string, NewInputAction_Button>();
    private Dictionary<string, NewInputAction_Trigger> m_inputActionDictionaly_Trigger
        = new Dictionary<string, NewInputAction_Trigger>();
    private Dictionary<string, NewInputAction_Vector2> m_inputActionDictionaly_Vector2
        = new Dictionary<string, NewInputAction_Vector2>();
    private Dictionary<string, NewInputAction_Vector3> m_inputActionDictionaly_Vector3
        = new Dictionary<string, NewInputAction_Vector3>();

    /// <summary> このキーコンフィツが有効か </summary>
    private bool m_isEnable = false;


    public bool m_IsEnable { get { return m_isEnable; } }


    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Init()
    {
        m_inputActionDictionaly_Button.Clear();
        m_inputActionDictionaly_Trigger.Clear();
        m_inputActionDictionaly_Vector2.Clear();
        m_inputActionDictionaly_Vector3.Clear();

        foreach (NewInputAction_Button act_button in m_inputActionList_Button)
        {
            act_button.Init();
            act_button.InitMap(this);
            m_inputActionDictionaly_Button.Add(act_button.m_ActionName, act_button);
        }
        foreach (NewInputAction_Trigger act_trigger in m_inputActionList_Trigger)
        {
            act_trigger.Init();
            m_inputActionDictionaly_Trigger.Add(act_trigger.m_ActionName, act_trigger);
        }
        foreach (NewInputAction_Vector2 act_vector2 in m_inputActionList_Vector2)
        {
            act_vector2.Init();
            m_inputActionDictionaly_Vector2.Add(act_vector2.m_ActionName, act_vector2);
        }
        foreach (NewInputAction_Vector3 act_vector3 in m_inputActionList_Vector3)
        {
            act_vector3.Init();
            m_inputActionDictionaly_Vector3.Add(act_vector3.m_ActionName, act_vector3);
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    public void Update()
    {
        foreach (NewInputAction_Button act_button in m_inputActionList_Button)
        {
            act_button.Update();
        }
        foreach (NewInputAction_Trigger act_trigger in m_inputActionList_Trigger)
        {
            act_trigger.Update();
        }
        foreach (NewInputAction_Vector2 act_vector2 in m_inputActionList_Vector2)
        {
            act_vector2.Update();
        }
        foreach (NewInputAction_Vector3 act_vector3 in m_inputActionList_Vector3)
        {
            act_vector3.Update();
        }
    }

    /// <summary>
    /// 入力状況リセット
    /// </summary>
    public void Reset()
    {
        foreach (NewInputAction_Button act_button in m_inputActionList_Button)
        {
            act_button.Reset();
        }
        foreach (NewInputAction_Trigger act_trigger in m_inputActionList_Trigger)
        {
            act_trigger.Reset();
        }
        foreach (NewInputAction_Vector2 act_vector2 in m_inputActionList_Vector2)
        {
            act_vector2.Reset();
        }
        foreach (NewInputAction_Vector3 act_vector3 in m_inputActionList_Vector3)
        {
            act_vector3.Reset();
        }
    }

    /// <summary>
    /// キーコンフィグの更新処理有効化
    /// </summary>
    public void Enable()
    {
        m_isEnable = true;
    }

    /// <summary>
    /// キーコンフィグの更新処理無効化
    /// </summary>
    public void Disable()
    {
        m_isEnable = false;
    }

    /// <summary>
    /// 特定のプレイヤーのデバイスが有効かどうか判断
    /// </summary>
    /// <param name="deviceId"> プレイヤー番号 </param>
    public bool GetIsEntryDevice(int deviceId)
    {
        if (!JoyconManager.Instance.GetIsEntry(deviceId) && !PadManager.Instance.GetIsEntry(deviceId))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 特定のプレイヤーのJoyconが左Joyconかどうか判断
    /// </summary>
    /// <param name="deviceId"> プレイヤー番号 </param>
    public bool GetJoyconIsLeft(int deviceId)
    {
        if (JoyconManager.Instance.GetIsEntry(deviceId))
        {
            return JoyconManager.Instance.GetDevice(deviceId).isLeft;
        }

        return false;
    }

    /// <summary>
    /// ボタン系InputActionを取得
    /// ※NewInputActionNameEnumをnameofで文字列化して参照
    /// </summary>
    /// <param name="key"> キー(NewInputActionNameEnumをnameofで文字列化して参照) </param>
    public NewInputAction_Button GetAction_Button(string key)
    {
        return m_inputActionDictionaly_Button[key];
    }

    /// <summary>
    /// トリガー系InputActionを取得
    /// ※NewInputActionNameEnumをnameofで文字列化して参照
    /// </summary>
    /// <param name="key"> キー(NewInputActionNameEnumをnameofで文字列化して参照) </param>
    public NewInputAction_Trigger GetAction_Trigger(string key)
    {
        return m_inputActionDictionaly_Trigger[key];
    }

    /// <summary>
    /// ２次元ベクトル系InputActionを取得
    /// ※NewInputActionNameEnumをnameofで文字列化して参照
    /// </summary>
    /// <param name="key"> キー(NewInputActionNameEnumをnameofで文字列化して参照) </param>
    public NewInputAction_Vector2 GetAction_Vec2(string key)
    {
        return m_inputActionDictionaly_Vector2[key];
    }

    /// <summary>
    /// ３次元ベクトル系InputActionを取得
    /// ※NewInputActionNameEnumをnameofで文字列化して参照
    /// </summary>
    /// <param name="key"> キー(NewInputActionNameEnumをnameofで文字列化して参照) </param>
    public NewInputAction_Vector3 GetAction_Vec3(string key)
    {
        return m_inputActionDictionaly_Vector3[key];
    }
}
