/*******************************************************************************
*
*	タイトル：	プレイヤーの入力を受け付けるスクリプト
*	ファイル：	PlayerController.cs
*	作成者：	古本 泰隆
*	制作日：    2023/09/12
*
*******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        Death = 0,
        Respawn,
        Play,
    }

    public enum Type
    {
        Balance = 0,
        Power,
        Speed,
    }

    #region 変数

    [Header("参照")]
    /// <summary> 足オブジェクト </summary>
    [SerializeField, CustomLabel("足オブジェクト")]
    protected GameObject m_leg;
    /// <summary> 足の当たり判定オブジェクト </summary>
    [SerializeField, CustomLabel("足の当たり判定オブジェクト")]
    protected Collider2D m_legCollision;
    /// <summary> 足の親オブジェクト </summary>
    [SerializeField, CustomLabel("足の親オブジェクト")]
    protected GameObject m_legParent;
    /// <summary> 体の当たり判定オブジェクト </summary>
    [SerializeField, CustomLabel("体の当たり判定オブジェクト")]
    protected Collider2D m_bodyCollision;
    /// <summary> リスポーンボックス </summary>
    [SerializeField, CustomLabel("リスポーンボックス")]
    protected GameObject m_respawnBox;
    /// <summary> 蹴り時のエフェクトの親オブジェクト </summary>
    [SerializeField, CustomLabel("蹴り時のエフェクトの親オブジェクト")]
    protected Transform m_kickEffectParent;
    /// <summary> プレイヤータイプ </summary>
    [SerializeField, CustomLabel("プレイヤータイプ")]
    protected Type m_type = Type.Balance;

    [Header("蹴り関係")]
    /// <summary> 蹴る方向 </summary>
    [SerializeField, CustomLabelReadOnly("蹴る方向")]
    protected Vector2 m_kickDir = Vector2.zero;
    /// <summary> 蹴り_突き出す速さ(秒) </summary>
    [SerializeField, CustomLabel("蹴り_突き出す速さ(倍)")]
    protected float m_kickStickOutSpeed = 0.05f;
    /// <summary> 蹴り_ひっこめる速さ(秒) </summary>
    [SerializeField, CustomLabel("蹴り_ひっこめる速さ(倍)")]
    protected float m_kickRetractSpeed = 0.1f;
    /// <summary> 頭突き状態の最長継続時間 </summary>
    [SerializeField, CustomLabel("頭突き状態の最長継続時間")]
    protected float m_headbuttTime = 1.0f;
    /// <summary> 頭突き状態を継続する速度 </summary>
    [SerializeField, CustomLabel("頭突き状態を継続する速度")]
    protected float m_headbuttKeepSpeed = 2.0f;
    /// <summary> 頭突き状態を継続する速度 </summary>
    [SerializeField, CustomLabel("頭突き時の質量")]
    protected float m_headbuttMass = 5.0f;

    [Header("移動関係")]
    /// <summary> 移動速度 </summary>
    [SerializeField, CustomLabel("移動加速度")]
    protected float m_moveSpeed = 2.0f;
    /// <summary> 入力が現在の移動方向と逆の時の減衰速度 </summary>
    [SerializeField, CustomLabel("入力が現在の移動方向と逆の時の減衰速度")]
    protected float m_moveDecaySpeed = 0.8f;
    /// <summary> 入力による最大移動速度 </summary>
    [SerializeField, CustomLabel("入力による最大移動速度")]
    protected float m_moveSpeedMax = 2.5f;

    [Header("その他")]
    /// <summary> 復活後の無敵時間 </summary>
    [SerializeField, CustomLabel("復活後の無敵時間")]
    protected float m_revivalInvincible = 2.0f;
    /// <summary> バンパーを無視するか </summary>
    [SerializeField, CustomLabel("バンパーを無視するか")]
    protected bool m_bumperIgnore = false;

    [Header("デバッグ用")]
    /// <summary> ステート </summary>
    [SerializeField, CustomLabel("ステート")]
    protected State m_state = State.Play;

    /// <summary> 各タイプの日本語名 </summary>
    public static readonly string[] m_TypeStr =
    {
        "バランス",
        "パワー",
        "スピード",
    };

    /// <summary> アニメーターコンポーネント </summary>
    protected Action[] m_stateAction;
    /// <summary> アニメーターコンポーネント </summary>
    protected Animator m_animator;
    /// <summary> リスポーン用アニメーターコンポーネント </summary>
    protected Animator m_respawnAnimator;
    /// <summary> リジッドボディコンポーネント </summary>
    protected Rigidbody2D m_rb;
    /// <summary> プレイヤーの番号 </summary>
    protected PlayerId m_playerId;
    /// <summary> プレイヤーへの衝撃管理コンポーネント </summary>
    protected PlayerImpact m_playerImpact;
    /// <summary> プレイヤーの無敵管理コンポーネント </summary>
    protected PlayerInvincible m_playerInvincible;
    /// <summary> プレイヤーの足の判定コールバック </summary>
    protected PlayerLegOnCollision m_playerLegOnCollision;
    /// <summary> コリジョン制御コンポーネント </summary>
    protected CollisionController m_colController;
    /// <summary> プレイヤー入力取得用コンポーネント </summary>
    protected PlayerInputController m_inputController;
    /// <summary> プレイヤー表情制御コンポーネント </summary>
    protected PlayerFaceController m_faceController;

    /// <summary> 蹴っているかどうか </summary>
    protected bool m_isKicking = false;
    /// <summary> 頭突きしているかどうか </summary>
    protected bool m_isHeadbutt = false;
    /// <summary> 頭突きの残り時間 </summary>
    protected float m_headbuttTimeCount = 0.0f;
    /// <summary> 入力不可かどうか </summary>
    protected bool m_isNotInputReceive = false;
    /// <summary> 強制復活までの制限時間 </summary>
    protected float m_revivalTimeCount = 0.0f;
    /// <summary> 復活した次のフレームかどうか </summary>
    protected bool m_revivalNextFrame = false;

    #endregion

    public Type m_Type { get { return m_type; } }

    public float m_KickStickOutSpeed { get { return m_kickStickOutSpeed; } }

    public float m_KickRetractSpeed { get { return m_kickRetractSpeed; } }

    public bool m_IsKicking { get { return m_isKicking; } }

    public State m_State { get { return m_state; } }

    public Vector2 m_KickDir { get { return m_kickDir; } }

    public bool m_BumperIgnore { get { return m_bumperIgnore; } }

    public bool m_IsHeadbutt { get { return m_isHeadbutt; } }


    protected virtual void Awake()
    {
        m_stateAction = new Action[]{ DeathAction, RespawnAction, PlayAction };

        m_animator = GetComponent<Animator>();
        m_respawnAnimator = transform.GetChild(0).GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody2D>();
        m_playerId = GetComponent<PlayerId>();
        m_playerImpact = GetComponent<PlayerImpact>();
        m_playerInvincible = GetComponent<PlayerInvincible>();
        m_playerLegOnCollision = m_legCollision.GetComponent<PlayerLegOnCollision>();
        m_colController = GetComponent<CollisionController>();
        m_inputController = transform.parent.GetComponent<PlayerInputController>();
        m_faceController = GetComponent<PlayerFaceController>();

        // 蹴りの速さをアニメーションコントローラーに反映
        m_animator.SetFloat("StickOutSpeed", m_kickStickOutSpeed);
        m_animator.SetFloat("RetractSpeed", m_kickRetractSpeed);

        // 足を非アクティブ化
        // ※シーン上でやらない理由は初期化で足を参照することがあるため
        m_leg.SetActive(false);
    }

    protected virtual void Update()
    {
        m_stateAction[(int)m_state]();
    }

    private void DeathAction() {}

    private void RespawnAction()
    {
        // 復活についての更新
        RevivalUpdate();
    }

    protected virtual void PlayAction()
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
            m_playerImpact.KickPlatform();
            m_revivalNextFrame = false;
        }
    }

    /// <summary>
    /// スティックを倒したかどうか判断する(Pad)
    /// </summary>
    private bool IsStickTilt()
    {
        return m_inputController.GetKick(m_playerId.m_Id);
    }

    #region 更新処理

    /// <summary>
    /// 移動関連の更新
    /// </summary>
    protected virtual void MoveUpdate()
    {
        // 入力受付不可状態の場合移動入力を受け付けない
        if (m_isNotInputReceive) { return; }

        // 入力取得
        float horizontal = m_inputController.GetMove(m_playerId.m_Id).x;

        // 通常の速度計算
        float defaultSpeed = m_moveSpeed * horizontal * Time.deltaTime;
        // 減衰する場合の速度計算
        float decaySpeed = m_rb.velocity.x * m_moveDecaySpeed * horizontal * Time.deltaTime;

        float addSpeed = defaultSpeed;
        if (horizontal < 0.0f && m_rb.velocity.x >= -m_moveSpeedMax)
        {
            // 値の大きい加速度(より加速する方)を反映する
            if (decaySpeed < defaultSpeed)
            {
                addSpeed = decaySpeed;
            }
        }
        else if (horizontal > 0.0f && m_rb.velocity.x <= m_moveSpeedMax)
        {
            // 値の大きい加速度(より加速する方)を反映する
            if (decaySpeed > defaultSpeed)
            {
                addSpeed = decaySpeed;
            }
        }

        // 速度反映
        m_rb.velocity += new Vector2(addSpeed, 0.0f);
    }

    /// <summary>
    /// 蹴り関連の更新
    /// </summary>
    protected virtual void KickUpdate()
    {
        // 受け身不可状態または入力受付不可状態の場合蹴り入力を受け付けない
        if (m_playerImpact.m_IsNotPassive || m_isNotInputReceive || m_isKicking) { return; }

        bool isKickStart = false;

        if (IsStickTilt())
        {// Pad
            // 蹴る方向更新
            m_kickDir = m_inputController.GetKickDir(m_playerId.m_Id).normalized;

            isKickStart = true;
        }

        if (isKickStart)
        {
            // 蹴りを開始
            m_leg.SetActive(true);
            m_legCollision.enabled = true;
            m_animator.SetTrigger("Kick");
            m_isKicking = true;
            EffectContainer.Instance.EffectPlay(
                "蹴り風切り_" + m_TypeStr[(int)m_type],
                m_kickEffectParent.position,
                m_kickEffectParent.rotation,
                m_kickEffectParent,
                transform.localScale);
            AudioManager.Instance.PlaySe(
                "プレイヤー_蹴りの風切り音_" + m_TypeStr[(int)m_type],
                false);
            AudioManager.Instance.PlaySe(
                "プレイヤー_蹴りのホログラム音_" + m_TypeStr[(int)m_type],
                false);
            m_faceController.SetAngryFace(true);
        }
    }

    /// <summary>
    /// 頭突き関連の更新
    /// </summary>
    protected void HeadbuttUpdate()
    {
        if (!m_isHeadbutt) { return; }

        m_headbuttTimeCount -= Time.deltaTime;
        if (m_headbuttTimeCount <= 0.0f ||
            m_rb.velocity.sqrMagnitude < m_headbuttKeepSpeed * m_headbuttKeepSpeed)
        {
            EndHeadbutt();
        }
    }

    protected virtual void RevivalUpdate()
    {
        KickUpdate();
        if (m_isKicking)
        {
            m_legCollision.enabled = false;
            Revival();
            m_revivalNextFrame = true;
            return;
        }

        m_revivalTimeCount -= Time.deltaTime;
        if (m_revivalTimeCount <= 0.0f)
        {
            m_revivalTimeCount = 0.0f;
            Revival();
            return;
        }
    }

    #endregion

    #region その他

    public void SetState(State next)
    {
        m_state = next;
    }

    public void SetBumperIgnore(bool balue)
    {
        m_bumperIgnore = balue;
    }

    /// <summary>
    /// 蹴りの当たり判定削除コールバック
    /// </summary>
    public void DisableLegCollisionCallback()
    {
        m_legCollision.enabled = false;
    }

    /// <summary>
    /// 蹴り終了時コールバック
    /// </summary>
    public void DisableLegCallback()
    {
        // 蹴りを終了する
        m_leg.SetActive(false);
        m_isKicking = false;
        // 足の衝突済みリストをリセットする
        m_playerLegOnCollision.ResetContactList();
        m_faceController.SetAngryFace(false);
    }

    /// <summary>
    /// 頭突き開始処理
    /// </summary>
    public void StartHeadbutt()
    {
        // 頭突きの開始
        m_isHeadbutt = true;
        m_rb.mass = m_headbuttMass;
        m_headbuttTimeCount = m_headbuttTime;
    }

    public void EndHeadbutt()
    {
        m_isHeadbutt = false;
        m_rb.mass = 1;
        m_headbuttTimeCount = 0.0f;
    }

    /// <summary>
    /// リスポーンボックス移動完了コールバック
    /// </summary>
    public void JumpOutFinishCallback_Respawn()
    {
        // ステートをリスポーン待ちに変更
        SetState(State.Respawn);
    }

    /// <summary>
    /// リスポーンボックス移動完了コールバック
    /// </summary>
    public void JumpOutFinishCallback_FirstRespawn()
    {
        // 復活させる
        Revival();
    }

    /// <summary>
    /// 入力受付不可状態を変更する
    /// </summary>
    /// <param name="isNotInputReceive"> 入力受付不可にするかどうか </param>
    public void SetIsNotInputReceive(bool isNotInputReceive)
    {
        m_isNotInputReceive = isNotInputReceive;
    }

    protected virtual void Revival()
    {
        // ステートを通常操作に変更
        SetState(State.Play);
        // コリジョンを有効にする
        m_colController.EnableCollider();
        // 物理演算系を再起動
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = 0.0f;
        m_rb.constraints = RigidbodyConstraints2D.None;
        // リスポーンボックスを無効にする
        m_respawnBox.SetActive(false);
        // アニメーションのリスポーン待機を終了する
        m_respawnAnimator.SetBool("IsRespawn", false);
        // 無敵時間を設定
        m_playerInvincible.SetInvincible(m_revivalInvincible);
        // 復活したのでリスポーンマネージャーに終了通知
        RespawnManager.Instance.EndRespawn(transform);
        // 座標関係を元に戻す
        ResetPosition();
    }

    protected virtual void ResetPosition()
    {
        transform.root.position = transform.GetChild(0).position;
        transform.root.rotation = transform.GetChild(0).rotation;
        transform.GetChild(0).localPosition = Vector3.zero;
        transform.GetChild(0).localRotation = Quaternion.identity;
    }

    public virtual void Death(float revivalTime)
    {
        // ステートを死亡に変更
        SetState(State.Death);
        // コリジョンを無効にする
        m_colController.DisableCollider();
        // 物理演算系を停止
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = 0.0f;
        m_rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // 座標を見えない位置に移動
        transform.position = new Vector3(0.0f, -100.0f, 0.0f);
        // 蹴りを終了する
        DisableLegCallback();
        // 頭突きを終了する
        m_isHeadbutt = false;
        // リスポーンボックスを有効にする
        m_respawnBox.SetActive(true);
        // 復活までの時間を設定
        m_revivalTimeCount = revivalTime;
    }

    #endregion
}
