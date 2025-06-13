using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_CannonFight : PlayerController
{
    [SerializeField, CustomLabel("カメラターゲットグループ登録")]
    private CinemachineTargetGroupRegister m_targetGroupResister;

    /// <summary> プレイヤー必殺技管理コンポーネント </summary>
    private PlayerSpMove m_playerSpMove;

    /// <summary> 蹴り_突き出す速さ(退避用) </summary>
    private float m_tempKickStickOutSpeed;

    /// <summary> 蹴り_ひっこめる速さ(退避用) </summary>
    private float m_tempKickRetractSpeed;


    protected override void Awake()
    {
        base.Awake();

        m_playerSpMove = GetComponent<PlayerSpMove>();
    }

    protected override void PlayAction()
    {
        base.PlayAction();

        // 必殺技発動についての更新
        SpMoveUpdate();
    }

    private void SpMoveUpdate()
    {
        if (((PlayerInputController_CannonFight)m_inputController).GetSpMove(m_playerId.m_Id))
        {
            // 必殺技ゲージがMAXになっているか
            if (m_playerSpMove.m_IsSpMovePointMaxCharge)
            {
                // 必殺技を発動する
                m_playerSpMove.StartSpMove();
            }
        }
    }

    protected override void RevivalUpdate()
    {
        base.KickUpdate();
        if (m_isKicking)
        {
            Revival();
            m_legCollision.enabled = false;
            // 蹴って出てくる動き
            m_playerImpact.KickPlatform();
        }
        else
        {
            m_revivalTimeCount -= Time.deltaTime;
            if (m_revivalTimeCount <= 0.0f)
            {
                m_revivalTimeCount = 0.0f;
                Revival();
            }
        }
    }

    /// <summary> パラメータを必殺技用に変更 </summary>
    public void SetDuringSpMoveParam()
    {
        // 足が出る速さ
        if (m_playerSpMove.m_IsChangeDuringSpMove_KickStickOutSpeed)
        {
            m_tempKickStickOutSpeed = m_kickStickOutSpeed;
            m_kickStickOutSpeed = m_playerSpMove.m_DuringSpMove_KickStickOutSpeed;
            m_animator.SetFloat("StickOutSpeed", m_kickStickOutSpeed);
        }
        // 足を戻す速さ
        if (m_playerSpMove.m_IsChangeDuringSpMove_KickRetractSpeed)
        {
            m_tempKickRetractSpeed = m_kickRetractSpeed;
            m_kickRetractSpeed = m_playerSpMove.m_DuringSpMove_KickRetractSpeed;
            m_animator.SetFloat("RetractSpeed", m_kickRetractSpeed);
        }
    }

    /// <summary> パラメータをデフォルト値に変更 </summary>
    public void SetDefaultParam()
    {
        // 足が出る速さ
        if (m_playerSpMove.m_IsChangeDuringSpMove_KickStickOutSpeed)
        {
            m_kickStickOutSpeed = m_tempKickStickOutSpeed;
            m_animator.SetFloat("StickOutSpeed", m_kickStickOutSpeed);
        }
        // 足を戻す速さ
        if (m_playerSpMove.m_IsChangeDuringSpMove_KickRetractSpeed)
        {
            m_kickRetractSpeed = m_tempKickRetractSpeed;
            m_animator.SetFloat("RetractSpeed", m_kickRetractSpeed);
        }
    }

    protected override void Revival()
    {
        base.Revival();

        m_respawnAnimator.SetBool("IsFirstRespawn", false);
        m_targetGroupResister.Resist(transform);
    }

    public override void Death(float revivalTime)
    {
        base.Death(revivalTime);

        m_targetGroupResister.Delete(transform);
    }

    public void FirstDeath()
    {
        // ステートを死亡に変更
        SetState(State.Death);
        // コリジョンを無効にする
        m_colController.DisableCollider();
        // 物理演算系を停止
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = 0.0f;
        m_rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // リスポーンボックスを有効にする
        m_respawnBox.SetActive(true);
        // 復活までの時間を設定
        m_revivalTimeCount = 0.0f;
    }

    public void InitCameraTargetGroup(CinemachineTargetGroupRegister targetGroup)
    {
        m_targetGroupResister = targetGroup;
    }
}
