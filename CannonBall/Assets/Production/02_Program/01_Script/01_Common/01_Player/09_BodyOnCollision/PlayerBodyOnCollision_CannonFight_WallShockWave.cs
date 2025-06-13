using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyOnCollision_CannonFight_WallShockWave : PlayerBodyOnCollision_CannonFight
{
    /// <summary> プレイヤー必殺技制御コンポーネント </summary>
    private PlayerSpMove_WallShockWave m_playerSpMove;

    private bool m_isCreateShockWave = false;


    protected override void Start()
    {
        base.Start();

        m_playerSpMove = transform.root.GetComponent<PlayerSpMove_WallShockWave>();
    }

    protected void Update()
    {
        m_isCreateShockWave = false;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.transform.CompareTag("Platform"))
        {
            OnCollisionEnter_Platform(collision);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Platform"))
        {
            OnTriggerEnter_Platform(collision);
        }
    }

    /// <summary> プレイヤーが触れた際のイベント </summary>
    /// <param name="collision"> プレイヤーのコリジョン </param>
    protected override void OnCollisionEnter_Player(Collision2D collision)
    {
        if (m_playerSpMove.m_IsSpMove)
        {
            OnCollisionEnter_Platform(collision);
        }
        else
        {
            base.OnCollisionEnter_Player(collision);
        }
    }

    /// <summary> 地形が触れた際のイベント </summary>
    /// <param name="collision"> 地形のコリジョン </param>
    protected override void OnCollisionEnter_Platform(Collision2D collision)
    {
        base.OnCollisionEnter_Platform(collision);

        // 必殺技発動中でなければ処理しない
        if (m_playerSpMove.m_IsSpMove && !m_isCreateShockWave)
        {
            CreateShockWave(collision.collider);
        }
    }

    /// <summary> 地形が触れた際のイベント </summary>
    /// <param name="collision"> 地形のコリジョン </param>
    protected void OnTriggerEnter_Platform(Collider2D collision)
    {
        // 必殺技発動中でなければ処理しない
        if (m_playerSpMove.m_IsSpMove && !m_isCreateShockWave)
        {
            CreateShockWave(collision);
        }
    }

    private void CreateShockWave(Collider2D collision)
    {
        // エフェクト生成
        GameObject effect = null;
        EffectContainer.Instance.EffectPlay(
            ref effect,
            "必殺技_壁衝撃波_" + (transform.root.GetComponent<PlayerId>().m_Id + 1) + "P",
            collision.ClosestPoint(transform.position),
            /*collision.GetContact(0).point*/
            Quaternion.Euler(-90.0f, 0.0f, 0.0f));
        WallShockWaveOnCollision onCollision = effect.GetComponent<WallShockWaveOnCollision>();

        // パラメータ設定
        onCollision.InitParam(
            m_playerSpMove.gameObject,
            m_playerSpMove.m_WallShockWavePower,
            m_playerSpMove.m_WallShockWaveActivateTime);

        m_playerSpMove.Bounce();

        AudioManager.Instance.PlaySe("キャノンファイト_衝撃波", false);
        // コントローラー振動
        VibrationManager.Instance.SetVibration(transform.root.GetComponent<PlayerId>().m_Id, 3, 1.0f);

        m_isCreateShockWave = true;
    }
}
