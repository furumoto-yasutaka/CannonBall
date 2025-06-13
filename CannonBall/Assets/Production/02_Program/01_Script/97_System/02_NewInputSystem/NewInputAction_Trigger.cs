/*******************************************************************************
*
*	タイトル：	抽象入力情報(トリガー)スクリプト
*	ファイル：	NewInputAction_Trigger.cs
*	作成者：	古本 泰隆
*	制作日：    2023/09/21
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NewInputAction_Trigger : NewInputAction
{
    #region enum

    /// <summary> Joyconトリガーリスト </summary>
    public enum JoyconTriggerId
    {
        Length = 0,
    }

    /// <summary> Padトリガーリスト </summary>
    public enum PadTriggerId
    {
        LeftTrigger = 0,
        RightTrigger,

        Length,
    }

    #endregion


    /// <summary> 関数ポインタの型宣言 </summary>
    public delegate void GetInput(int deviceId, int i);


    /// <summary> Joyconのキー設定 </summary>
    [SerializeField]
    private List<JoyconTriggerId> m_joyconActionIdList_Inspector = new List<JoyconTriggerId>();

    /// <summary> Padのキー設定 </summary>
    [SerializeField]
    private List<PadTriggerId> m_padActionIdList_Inspector = new List<PadTriggerId>();

    /// <summary> 各キーの入力状況 </summary>
    private List<float>[] m_inputInfoList = new List<float>[DeviceInfo.m_DeviceNum];

    /// <summary> Pad用ボタンの更新メソッド </summary>
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


    //--------------------取得処理--------------------

    /// <summary>
    /// 押され具合を取得(特定のデバイスから)
    /// </summary>
    /// <param name="deviceId"> ロックするグループの情報 </param>
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
    /// 押され具合を取得(全てのデバイスから)
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


    //--------------------更新処理(Pad)--------------------

    private void Update_Pad_LeftTrigger(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i] = PadManager.Instance.GetDevice(deviceId).leftTrigger.value;
    }

    private void Update_Pad_RightTrigger(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i] = PadManager.Instance.GetDevice(deviceId).rightTrigger.value;
    }
}
