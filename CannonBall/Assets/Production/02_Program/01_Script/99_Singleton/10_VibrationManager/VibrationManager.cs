/*******************************************************************************
*
*	タイトル：	コントローラー振動管理シングルトンスクリプト
*	ファイル：	VibrationManager.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/07
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationManager : SingletonMonoBehaviour<VibrationManager>
{
    [System.Serializable]
    public class VibrationInfo
    {
        public int m_time = 0;
        public float m_power = 0.0f;
    }

    [System.Serializable]
    public class VibrationInfoArray
    {
        public List<VibrationInfo> m_info = new List<VibrationInfo>();
    }


    /// <summary> Joycon振動の低周波数 </summary>
    [SerializeField, CustomLabel("Joycon振動の低周波数")]
    private float m_freqLow = 600.0f;

    /// <summary> Joycon振動の高周波数 </summary>
    [SerializeField, CustomLabel("Joycon振動の高周波数")]
    private float m_freqHigh = 600.0f;

    /// <summary> 現在作動中のバイブレーション情報 </summary>
    [SerializeField, CustomLabel("現在作動中のバイブレーション情報")]
    private VibrationInfo[] m_currentInfo = new VibrationInfo[DeviceInfo.m_DeviceNum];

    /// <summary> 作動待ちのバイブレーション情報 </summary>
    [SerializeField, CustomLabel("作動待ちのバイブレーション情報")]
    private VibrationInfoArray[] m_waitInfoList = new VibrationInfoArray[DeviceInfo.m_DeviceNum];


    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < m_waitInfoList.Length; i++)
        {
            m_waitInfoList[i].m_info = new List<VibrationInfo>();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            CurrentInfoUpdate(i);
            WaitInfoUpdate(i);
        }
    }

    /// <summary>
    /// 現在作動中の振動情報の更新
    /// </summary>
    /// <param name="playerId"> プレイヤー番号 </param>
    private void CurrentInfoUpdate(int playerId)
    {
        if (m_currentInfo[playerId].m_time == 0.0f) { return; }

        m_currentInfo[playerId].m_time--;
        if (m_currentInfo[playerId].m_time <= 0)
        {
            if (m_waitInfoList[playerId].m_info.Count > 0)
            {
                m_currentInfo[playerId] = m_waitInfoList[playerId].m_info[0];
                m_waitInfoList[playerId].m_info.RemoveAt(0);

                // 振動させる
                SetVibration(playerId);
            }
            else
            {
                m_currentInfo[playerId] = null;
                StopVibration(playerId);
            }
        }
    }

    /// <summary>
    /// 現在待機中の振動情報の更新
    /// </summary>
    /// <param name="playerId"> プレイヤー番号 </param>
    private void WaitInfoUpdate(int playerId)
    {
        int j = 0;
        while (j < m_waitInfoList[playerId].m_info.Count)
        {
            m_waitInfoList[playerId].m_info[j].m_time--;
            if (m_waitInfoList[playerId].m_info[j].m_time <= 0)
            {
                m_waitInfoList[playerId].m_info.RemoveAt(j);
            }
            else
            {
                j++;
            }
        }
    }

    /// <summary>
    /// バイブレーション作動
    /// </summary>
    /// <param name="playerId"> どのプレイヤーに対応したデバイスを振動させるか </param>
    /// <param name="time"> 振動時間(フレーム) </param>
    /// <param name="power"> 振動の強さ(0.0~1.0) </param>
    public void SetVibration(int playerId, int time, float power)
    {
        VibrationInfo info = new VibrationInfo();
        info.m_power = power;
        info.m_time = time;

        RegistInfo(info, playerId);

        SetVibration(playerId);
    }

    /// <summary>
    /// バイブレーション作動
    /// </summary>
    /// <param name="playerId"> どのプレイヤーに対応したデバイスを振動させるか </param>
    private void SetVibration(int playerId)
    {
        // Joycon
        if (JoyconConnector.Instance.m_IsRun)
        {
            if (JoyconManager.Instance.GetIsEntry(playerId))
            {
                Joycon jc = JoyconManager.Instance.GetDevice(playerId);
                jc.SetRumble(m_freqLow, m_freqHigh, m_currentInfo[playerId].m_power, 0);
            }
        }

        // Pad
        if (PadManager.Instance.GetIsEntry(playerId))
        {
            Gamepad pad = PadManager.Instance.GetDevice(playerId);
            pad.SetMotorSpeeds(m_currentInfo[playerId].m_power, m_currentInfo[playerId].m_power);
        }
    }

    /// <summary>
    /// バイブレーション作動(全プレイヤー)
    /// </summary>
    /// <param name="time"> 振動時間(フレーム) </param>
    /// <param name="power"> 振動の強さ </param>
    public void SetVibrationAll(int time, float power)
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            VibrationInfo info = new VibrationInfo();
            info.m_power = power;
            info.m_time = time;

            RegistInfo(info, i);

            SetVibration(i);
        }
    }

    /// <summary>
    /// バイブレーション停止
    /// </summary>
    /// <param name="playerId"> どのプレイヤーに対応したデバイスを振動を停止するか </param>
    public void StopVibration(int playerId)
    {
        ResetInfo(playerId);

        // Joycon
        if (JoyconConnector.Instance.m_IsRun)
        {
            if (JoyconManager.Instance.GetIsEntry(playerId))
            {
                Joycon jc = JoyconManager.Instance.GetDevice(playerId);
                jc.SetRumble(m_freqLow, m_freqHigh, 0.0f, 0);
            }
        }

        // Pad
        if (PadManager.Instance.GetIsEntry(playerId))
        {
            Gamepad pad = PadManager.Instance.GetDevice(playerId);
            pad.SetMotorSpeeds(0.0f, 0.0f);
        }
    }

    /// <summary>
    /// バイブレーション停止(全プレイヤー)
    /// </summary>
    public void StopVibrationAll()
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            StopVibration(i);
        }
    }

    /// <summary>
    /// 振動情報を記録
    /// </summary>
    /// <param name="info"> 記録する振動情報 </param>
    /// <param name="playerId"> 対応するプレイヤー番号 </param>
    private void RegistInfo(VibrationInfo info, int playerId)
    {
        int i = 0;

        if (m_currentInfo[playerId].m_power == 0.0f)
        {
            m_currentInfo[playerId] = info;
        }
        else if (info.m_power > m_currentInfo[playerId].m_power)
        {// 全ての振動情報の中で一番強い場合即座に作動させる
            m_waitInfoList[playerId].m_info.Insert(0, m_currentInfo[playerId]);
            m_currentInfo[playerId] = info;
        }
        else if (info.m_time > m_currentInfo[playerId].m_time)
        {// 現在の振動より振動時間が長い場合
            // この振動情報を一旦保存するか調べる
            for (; i < m_waitInfoList[playerId].m_info.Count; i++)
            {
                if (info.m_power > m_waitInfoList[playerId].m_info[i].m_power)
                {// より大きい強さの振動の場合
                    m_waitInfoList[playerId].m_info.Insert(i, info);
                    i++;
                    break;
                }
                else if (info.m_time <= m_waitInfoList[playerId].m_info[i].m_time)
                {// より長い振動時間ではない場合は記録の必要なし
                    return;
                }
            }

            if (i == m_waitInfoList[playerId].m_info.Count)
            {
                // 一番振動が弱いが振動時間が最長の場合なので記録する
                m_waitInfoList[playerId].m_info.Add(info);
                i++;
            }
        }

        // 保存した情報によって必要なくなる振動情報を削除する
        for (; i < m_waitInfoList[playerId].m_info.Count; i++)
        {
            // 記録された振動時間より短い場合は振動することは無いので削除する
            if (info.m_time >= m_waitInfoList[playerId].m_info[i].m_time)
            {
                m_waitInfoList[playerId].m_info.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 記録を削除
    /// </summary>
    /// <param name="playerId"> 対応するプレイヤー番号 </param>
    private void ResetInfo(int playerId)
    {
        m_currentInfo[playerId] = new VibrationInfo();
        m_waitInfoList[playerId].m_info.Clear();
    }

    private void OnApplicationQuit()
    {
        StopVibrationAll();
    }
}
