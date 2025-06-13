/*******************************************************************************
*
*	タイトル：	プレイヤーが生存しているか判断するスクリプト(大乱闘モードver)
*	ファイル：	AliveZone_CannonFight.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/08
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AliveZone_CannonFight : AliveZone
{
    private CinemachineImpulseSource m_impulseSource;

    private bool m_stop = false;


    private void Start()
    {
        m_impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    /// <summary> プレイヤーが離れた際のイベント </summary>
    /// <param name="collision"> プレイヤーのコリジョン </param>
    protected override void OnTriggerExit_Player(Collider2D collision)
    {
        if (m_stop) { return; }

        Transform parent = collision.transform.root;
        int id = parent.GetComponent<PlayerId>().m_Id;

        if (collision.transform.position.x < 0.0f)
        {
            transform.GetChild(0).GetChild(id).GetComponent<ParticleSystem>().Play();
        }
        else
        {
            transform.GetChild(1).GetChild(id).GetComponent<ParticleSystem>().Play();
        }

        base.OnTriggerExit_Player(collision);

        // キルされたことによるポイントの清算を行う
        parent.GetComponent<PlayerPoint_CannonFight>().KilledDividePoint();

        PlayerSpMove spMove = parent.GetComponent<PlayerSpMove>();
        // 必殺技発動中だったら終了する
        if (spMove.m_IsSpMove)
        {
            spMove.EndSpMove();
            spMove.ResetSpMovePoint();
        }
        // キルされたことによる必殺技ゲージの増加を行う
        spMove.AccumulateBeKilledPattern();

        // 画面をゆらす
        m_impulseSource.GenerateImpulse();

        AudioManager.Instance.PlaySe(
            "キャノンファイト_場外時の音",
            false);
        AudioManager.Instance.PlaySe(
            "キャノンファイト_場外時の歓声",
            false);

        // コントローラー振動
        VibrationManager.Instance.SetVibration(id, 30, 0.9f);
    }

    public void Stop()
    {
        m_stop = true;
    }
}
