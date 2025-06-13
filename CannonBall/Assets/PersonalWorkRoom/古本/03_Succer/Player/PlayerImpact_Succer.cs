/*******************************************************************************
*
*	タイトル：	プレイヤーへの衝撃を制御するスクリプト(サッカーモードver)
*	ファイル：	PlayerImpact_Succer.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpact_Succer : PlayerImpact
{
    /// <summary> ボールを蹴る力 </summary>
    // 本来はボール側に作るつもりの変数だったが、
    // 違う能力のキャラを選択する機能が開発された時の為、
    // プレイヤー側に定義する
    [SerializeField, CustomLabel("ボールを蹴る力")]
    private float m_kickBallPower = 1.0f;

    /// <summary> ボールを蹴った時の反動 </summary>
    [SerializeField, CustomLabel("ボールを蹴った時の反動")]
    private float m_kickBallRecoil = 1.0f;


    public float m_KickBallPower { get { return m_kickBallPower; } }


    /// <summary> ボール蹴り時の反動の反映 </summary>
    public void KickBallRecoil()
    {
        // 速度を計算・反映
        m_rb.velocity = -m_playerController.m_KickDir * m_kickBallRecoil;

        // 速度を元に回転加速度を計算・反映
        m_rb.angularVelocity = m_rb.velocity.x * -m_kickAngularPower;
    }
}
