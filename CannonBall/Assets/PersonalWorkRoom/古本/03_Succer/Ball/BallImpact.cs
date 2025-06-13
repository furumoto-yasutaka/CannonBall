/*******************************************************************************
*
*	タイトル：	ボールの衝撃を制御するスクリプト(サッカーモードver)
*	ファイル：	BallImpact.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallImpact : MonoBehaviour
{
    /// <summary> 蹴ったときにおける加速度1あたりの回転加速度 </summary>
    [SerializeField, CustomLabel("加速度1あたりの蹴られ時の追加速度割合")]
    protected Vector2 m_inertiaPowerRate;

    /// <summary> 蹴られたときにおける加速度1あたりの回転加速度 </summary>
    [SerializeField, CustomLabel("蹴られたときにおける加速度1あたりの回転加速度")]
    protected float m_kickedAngularPower = 100.0f;

    /// <summary> リジッドボディ </summary>
    protected Rigidbody2D m_rb;


    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    /// <summary> 蹴られた衝撃を付与する </summary>
    /// <param name="dir"> 蹴られた方向 </param>
    /// <param name="power"> 蹴りの威力 </param>
    /// <param name="vel"> 蹴ったプレイヤーの加速度 </param>
    public void Kicked(Vector2 dir, float power, Vector2 vel)
    {
        // 速度を計算・反映
        Vector2 impact = dir * power;
        impact += dir * vel.magnitude * m_inertiaPowerRate;
        m_rb.velocity = impact;

        // 速度を元に回転加速度を計算・反映
        m_rb.angularVelocity = m_rb.velocity.x * -m_kickedAngularPower;
    }
}
