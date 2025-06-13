/*******************************************************************************
*
*	タイトル：	強制スクロールのカメラ移動経路参照用スクリプト
*	ファイル：	DangerRun_CameraMoveSource.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/06
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DangerRun_CameraMoveSource : MonoBehaviour
{
    /// <summary> 移動速度(秒速) </summary>
    [SerializeField, CustomLabel("移動速度(秒速)")]
    protected float m_speed = 1.0f;

    /// <summary> スピードアップイベントはあるか </summary>
    [SerializeField, CustomLabel("スピードアップイベントはあるか")]
    private bool m_isSpeedEvent = false;

    /// <summary> 追従する死亡判定を交換するか </summary>
    [SerializeField, CustomLabel("追従する死亡判定を交換するか")]
    private bool m_isReplaceDeadZone = true;

    /// <summary> コールバック関数 </summary>
    [SerializeField]
    private UnityEvent m_callback = new UnityEvent();

    /// <summary> 区間距離 </summary>
    protected float m_length = 1.0f;

    /// <summary> 区間ポイント番号 </summary>
    protected int m_sectionNumber = 0;

    /// <summary> 移動ベクトル </summary>
    protected Vector3 m_moveDistance;


    public int m_SectionNumber { get { return m_sectionNumber; } }

    public bool m_IsSpeedEvent { get { return m_isSpeedEvent; } }
    
    public bool m_IsReplaceDeadZone { get { return m_isReplaceDeadZone; } }


    /// <summary> パラメータ初期化 </summary>
    public void InitParam(int sectionNumber)
    {
        m_sectionNumber = sectionNumber;
        m_moveDistance = transform.parent.GetChild(m_sectionNumber + 1).position - transform.position;
        m_length = m_moveDistance.magnitude;
    }

    /// <summary> 現在位置を返す </summary>
    /// <param name="time"> 経過秒数 </param>
    /// <param name="isFinish"> この区間を通り終えたかどうか </param>
    /// <param name="overtime"> 距離が超過した場合余剰の秒数を返す </param>
    public virtual Vector3 GetPosition(float time, ref bool isFinish, ref float overtime)
    {
        // 位置を計算
        float len = time * m_speed;
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
            overtime = (len - m_length) / m_speed;
        }

        Vector3 pos = transform.position;
        pos += m_moveDistance * progress;

        return pos;
    }

    public void InvokeCallback()
    {
        m_callback.Invoke();
    }
}
