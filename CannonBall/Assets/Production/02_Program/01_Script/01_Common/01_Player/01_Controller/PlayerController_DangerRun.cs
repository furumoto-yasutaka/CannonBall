using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_DangerRun : PlayerController
{
    private AnimationFloatParamLerp m_animationFloatParamLerp_RespawnSwitch;

    private AnimationFloatParamLerp m_animationFloatParamLerp_PlayersCount;

    private bool m_isActive = false;


    public AnimationFloatParamLerp m_AnimationFloatParamLerp_RespawnSwitch
        { get { return m_animationFloatParamLerp_RespawnSwitch; } }

    public AnimationFloatParamLerp m_AnimationFloatParamLerp_PlayersCount
        { get { return m_animationFloatParamLerp_PlayersCount; } }


    protected override void Awake()
    {
        base.Awake();

        m_animationFloatParamLerp_RespawnSwitch = 
            new AnimationFloatParamLerp(m_respawnAnimator, "RespawnSwitch", 0.0f, 0.0f);
        m_animationFloatParamLerp_PlayersCount =
            new AnimationFloatParamLerp(m_respawnAnimator, "PlayersCount", 0.0f, 0.0f);
    }

    protected override void Update()
    {
        if (!m_isActive) { return; }

        base.Update();

        m_animationFloatParamLerp_RespawnSwitch.Update();
        m_animationFloatParamLerp_PlayersCount.Update();
    }

    protected override void PlayAction()
    {
        // 移動についての更新
        MoveUpdate();
        // 蹴りについての更新
        KickUpdate();
        // 頭突きについての更新
        HeadbuttUpdate();
        // 体の回転に持っていかれないように足の方向を補正する
        m_legParent.transform.rotation =
            Quaternion.identity * Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.down, m_kickDir), Vector3.forward);

        if (m_revivalNextFrame)
        {
            // 蹴って出てくる動き
            ((PlayerImpact_DangerRun)m_playerImpact).KickRespawn();
            m_revivalNextFrame = false;
        }
    }

    protected override void ResetPosition()
    {
        Vector3 pos = transform.GetChild(0).position;
        transform.parent = null;
        transform.GetChild(0).position = Vector3.zero;
        transform.position = pos;
    }

    public override void Death(float revivalTime)
    {
        // ステートを死亡に変更
        SetState(State.Death);
        // コリジョンを無効にする
        m_colController.DisableCollider();
        // 物理演算系の座標系を停止
        m_rb.velocity = Vector3.zero;
        m_rb.constraints = RigidbodyConstraints2D.FreezePosition;
        // 蹴りを終了する
        DisableLegCallback();
        // 頭突きを終了する
        m_isHeadbutt = false;
        // 復活までの時間を設定
        m_revivalTimeCount = revivalTime;
    }

    public void StopAndWarp()
    {
        // 物理演算系を停止
        m_rb.angularVelocity = 0.0f;
        m_rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // 座標を見えない位置に移動
        transform.position = new Vector3(0.0f, -100.0f, 0.0f);
        // リスポーンボックスを有効にする
        m_respawnBox.SetActive(true);
    }

    public void Active()
    {
        m_isActive = true;
    }
}
