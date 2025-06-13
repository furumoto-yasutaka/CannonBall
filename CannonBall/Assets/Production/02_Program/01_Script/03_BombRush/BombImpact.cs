/*******************************************************************************
*
*	タイトル：	爆弾が蹴られたりして動くスクリプト
*	ファイル：	BombImpact.cs
*	作成者：	青木 大夢
*	制作日：    2023/09/19
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombImpact : MonoBehaviour
{
    /// <summary> リジッドボディ </summary>
    private Rigidbody2D m_rb;

    /// <summary> 自分の速度に応じた頭突きの加算割合 </summary>
    [SerializeField, CustomLabel("自分の速度に応じた頭突きの強さの加算割合")]
    protected float m_headbuttAddRate = 0.2f;

    /// <summary> ぶっとんだときにおける加速度1あたりの回転加速度 </summary>
    [SerializeField, CustomLabel("ぶっとんだときにおける加速度1あたりの回転加速度")]
    protected float m_kickAngularPower = 15.0f;



    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    public void Impact(Vector2 dir, float power, Vector2 vel)
    {
        // 速度を計算・反映
        Vector2 baseVel = dir * power;
        Vector2 resultVel = baseVel;

        resultVel += dir * vel.magnitude;

        m_rb.velocity = resultVel;
    }

    /// <summary>
    /// プレイヤーに頭突かれた処理
    /// </summary>
    public void Headbutted(Vector2 dir, float power, Vector2 vel)
    {
        // 速度を計算・反映
        Vector2 resultVel = dir * power;
        resultVel += dir * vel.magnitude * m_headbuttAddRate;
        m_rb.velocity = resultVel;

        // 速度を元に回転加速度を計算・反映
        m_rb.angularVelocity = m_rb.velocity.x * -m_kickAngularPower;
    }
}
