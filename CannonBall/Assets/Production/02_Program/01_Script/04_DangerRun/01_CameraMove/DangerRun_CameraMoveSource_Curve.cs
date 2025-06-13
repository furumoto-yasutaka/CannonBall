/*******************************************************************************
*
*	タイトル：	強制スクロールのカメラ移動経路参照用スクリプト(移動速度をカーブ指定)
*	ファイル：	DangerRun_CameraMoveSource_Curve.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/06
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerRun_CameraMoveSource_Curve : DangerRun_CameraMoveSource
{
    /// <summary> 移動速度の目標値 </summary>
    [SerializeField, CustomLabel("移動速度の目標値")]
    private float m_speedTarget = 1.0f;

    /// <summary> 移動速度の補間速度 </summary>
    [SerializeField, CustomLabel("移動速度の補間速度")]
    private float m_speedLarpRate = 0.1f;

    private float m_nowSpeed;


    private void Start()
    {
        m_nowSpeed = m_speed;
    }

    /// <summary> 現在位置を返す </summary>
    /// <param name="time"> 経過秒数 </param>
    /// <param name="overtime"> 距離が超過した場合余剰の秒数を返す </param>
    public override Vector3 GetPosition(float time, ref bool isFinish, ref float overtime)
    {
        m_nowSpeed += (m_speedTarget - m_nowSpeed) * m_speedLarpRate;

        // 位置を計算
        float len = time * m_nowSpeed;
        float progress;

        // 区間を通り終えたか
        if (len < m_length)
        {
            progress = len / m_length;
        }
        else
        {
            progress = 1.0f;
            isFinish = true;
            // 超過分を時間に変換
            overtime = (len - m_length) / m_nowSpeed;
        }

        Vector3 pos = transform.position;
        pos += m_moveDistance * progress;

        return pos;
    }
}
