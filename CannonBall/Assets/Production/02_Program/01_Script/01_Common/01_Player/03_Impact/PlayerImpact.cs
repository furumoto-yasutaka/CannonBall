/*******************************************************************************
*
*	タイトル：	プレイヤーが吹き飛ぶ処理をまとめたスクリプト
*	ファイル：	PlayerImpact.cs
*	作成者：	古本 泰隆
*	制作日：    2023/09/12
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpact : MonoBehaviour
{
    /// <summary> 地形を蹴る力 </summary>
    [SerializeField, CustomLabel("地形を蹴る力")]
    protected float m_kickPlatformPower = 1.0f;

    /// <summary> 他プレイヤーを蹴る力 </summary>
    [SerializeField, CustomLabel("他プレイヤーを蹴る力")]
    protected float m_kickPlayerPower = 1.0f;

    /// <summary> 他プレイヤーを蹴ったときの反動 </summary>
    [SerializeField, CustomLabel("他プレイヤーを蹴ったときの反動")]
    protected float m_kickPlayerRecoil = 1.0f;

    /// <summary> ぶっとんだときにおける加速度1あたりの回転加速度 </summary>
    [SerializeField, CustomLabel("ぶっとんだときにおける加速度1あたりの回転加速度")]
    protected float m_kickAngularPower = 100.0f;

    /// <summary> 地形を蹴ったときにおける加速度1あたりの回転加速度 </summary>
    [SerializeField, CustomLabel("加速度1あたりの地形蹴り時の追加速度割合")]
    protected Vector2 m_inertiaPowerRate_Platform;

    /// <summary> プレイヤーを蹴ったときにおける加速度1あたりの回転加速度 </summary>
    [SerializeField, CustomLabel("加速度1あたりの他プレイヤー蹴り時の追加速度割合")]
    protected float m_inertiaPowerRate_Player;

    /// <summary> 蹴った相手を受け身不可状態にするか </summary>
    [SerializeField, CustomLabel("蹴った相手を受け身不可状態にするか")]
    protected bool m_isNotPassiveKick = false;

    /// <summary> 受け身不可状態にする時間 </summary>
    [SerializeField, CustomLabel("受け身不可状態にする時間")]
    protected float m_notPassiveTime = 1.0f;

    /// <summary> 自分が受け身不可状態かどうか </summary>
    [SerializeField, CustomLabelReadOnly("自分が受け身不可状態かどうか")]
    protected bool m_isNotPassive = false;

    /// <summary> 頭突きの強さ(基礎値) </summary>
    [SerializeField, CustomLabel("頭突きの強さ(基礎値)")]
    protected float m_headbuttBasePower = 3.0f;

    /// <summary> 自分の速度に応じた頭突きの加算割合 </summary>
    [SerializeField, CustomLabel("自分の速度に応じた頭突きの強さの加算割合")]
    protected float m_headbuttAddRate = 0.2f;

    /// <summary> ぶっ飛んでいる時に放出するパーティクルの親オブジェクト </summary>
    [SerializeField, CustomLabel("ぶっ飛んでいる時に放出するパーティクルの親オブジェクト")]
    protected Transform m_blownawayEffObj;

    /// <summary> ぶっ飛んでいる時に放出するパーティクルを消す速度の閾値 </summary>
    [SerializeField, CustomLabel("ぶっ飛んでいる時に放出するパーティクルを消す速度の閾値")]
    protected float m_blownawayEffHideThreshold = 5.0f;

    /// <summary> ぶっ飛んでいる時に振動させる速度の閾値 </summary>
    [SerializeField, CustomLabel("ぶっ飛んでいる時に振動させる速度の閾値")]
    protected float m_vibrationThreshold = 5.0f;

    /// <summary> ぶっ飛んでいる時に振動させる速度の閾値 </summary>
    [SerializeField, CustomLabel("ぶっ飛んでいる時に最大に振動させる速度の閾値")]
    protected float m_vibrationMaxThreshold = 25.0f;

    /// <summary> リジッドボディ </summary>
    protected Rigidbody2D m_rb;

    /// <summary> プレイヤー入力受付 </summary>
    protected PlayerController m_playerController;

    /// <summary> プレイヤー無敵制御 </summary>
    protected PlayerInvincible m_playerInvincible;

    /// <summary> 受け身不可状態の残り時間 </summary>
    protected float m_notPassiveTimeCount = 0.0f;

    /// <summary> ぶっ飛んでいる時に放出するパーティクル </summary>
    protected RotateLock_Custom m_blownawayRotateSetter;

    /// <summary> プレイヤー表情制御コンポーネント </summary>
    protected PlayerFaceController m_faceController;

    protected bool m_isBlownVibration = false;


    public float m_KickPlatformPower { get { return m_kickPlatformPower; } }

    public float m_KickPlayerPower { get { return m_kickPlayerPower; } }

    public float m_KickPlayerRecoil { get { return m_kickPlayerRecoil; } }

    public float m_KickAngularPower { get { return m_kickAngularPower; } }

    public Vector2 m_InertiaPowerRate_Platform { get { return m_inertiaPowerRate_Platform; } }
    
    public float m_InertiaPowerRate_Player { get { return m_inertiaPowerRate_Player; } }

    public bool m_IsNotPassiveKick { get { return m_isNotPassiveKick; } }

    public float m_NotPassiveTime { get { return m_notPassiveTime; } }

    public bool m_IsNotPassive { get { return m_isNotPassive; } }

    public float m_HeadbuttBasePower { get { return m_headbuttBasePower; } }

    public float m_HeadbuttAddRate { get { return m_headbuttAddRate; } }


    protected virtual void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_playerController = GetComponent<PlayerController>();
        m_playerInvincible = GetComponent<PlayerInvincible>();
        m_blownawayRotateSetter = m_blownawayEffObj.GetComponent<RotateLock_Custom>();
        m_faceController = GetComponent<PlayerFaceController>();
    }

    protected virtual void Update()
    {
        // 受け身不可状態についての更新処理
        NotPassiveTimeUpdate();

        // プレイヤーがぶっ飛ばされた時のエフェクトを出し続けるか確認する
        CheckBlownAwayEff();
        CheckVibration();
    }

    /// <summary>
    /// 蹴りの威力を計算
    /// </summary>
    /// <param name="dir"> 飛んでいく方向 </param>
    /// <param name="power"> キック力 </param>
    protected Vector2 CalcImpactPlatform(Vector2 dir, float power)
    {
        Vector2 baseVel = dir * power;
        Vector2 resultVel = baseVel;

        if (baseVel.x > 0)
        {
            resultVel.x += Mathf.Abs(m_rb.velocity.x) * m_inertiaPowerRate_Platform.x;
        }
        else
        {
            resultVel.x -= Mathf.Abs(m_rb.velocity.x) * m_inertiaPowerRate_Platform.x;
        }

        // 現在のプレイヤーの上下速度がジャンプの上下への加速度へ
        // 足される状態になるようにベクトルを計算する
        if (m_rb.velocity.y > 0 && baseVel.y > 0)
        {
            resultVel.y += Mathf.Abs(m_rb.velocity.y) * m_inertiaPowerRate_Platform.y;
        }

        return resultVel;
    }

    /// <summary>
    /// 蹴りの威力を計算
    /// </summary>
    /// <param name="dir"> 飛んでいく方向 </param>
    /// <param name="power"> キック力 </param>
    /// <param name="vel"> 慣性 </param>
    private Vector2 CalcImpactKicked(Vector2 dir, float power, Vector2 vel)
    {
        Vector2 baseVel = dir * power;
        Vector2 resultVel = baseVel;

        resultVel += dir * vel.magnitude * m_inertiaPowerRate_Player;

        return resultVel;
    }

    /// <summary>
    /// 地形を蹴る処理
    /// </summary>
    public void KickPlatform()
    {
        m_rb.velocity = CalcImpactPlatform(-m_playerController.m_KickDir, m_kickPlatformPower);

        // 速度を元に回転加速度を計算・反映
        m_rb.angularVelocity = m_rb.velocity.x * -m_kickAngularPower;
    }

    /// <summary>
    /// プレイヤーを蹴った反動の処理
    /// </summary>
    public void KickPlayerRecoil()
    {
        // 速度を計算・反映
        m_rb.velocity = -m_playerController.m_KickDir * m_kickPlayerRecoil;

        // 速度を元に回転加速度を計算・反映
        m_rb.angularVelocity = m_rb.velocity.x * -m_kickAngularPower;
    }

    /// <summary>
    /// プレイヤーに蹴られた処理
    /// </summary>
    public virtual void Kicked(Vector2 dir, float power, Vector2 vel)
    {
        // 無敵状態の時は衝撃を与えない
        if (m_playerInvincible.m_IsInvincible) { return; }

        // 速度を計算・反映
        m_rb.velocity = CalcImpactKicked(dir, power, vel);

        // 速度を元に回転加速度を計算・反映
        m_rb.angularVelocity = m_rb.velocity.x * -m_kickAngularPower;

        m_blownawayEffObj.GetChild(0).GetComponent<ParticleSystem>().Play();
        m_blownawayRotateSetter.SetRotate(dir);

        m_isBlownVibration = true;

        m_faceController.SetHitFace(true);
    }

    /// <summary>
    /// プレイヤーに頭突かれた処理
    /// </summary>
    public void Headbutted(Vector2 dir, float power, Vector2 vel)
    {
        // 無敵状態の時は衝撃を与えない
        if (m_playerInvincible.m_IsInvincible) { return; }

        // 速度を計算・反映
        Vector2 resultVel = dir * power;
        resultVel += dir * vel.magnitude * m_headbuttAddRate;
        m_rb.velocity = resultVel;

        // 速度を元に回転加速度を計算・反映
        m_rb.angularVelocity = m_rb.velocity.x * -m_kickAngularPower;
    }

    /// <summary>
    /// 蹴った相手を受け身不可状態にするかどうか設定
    /// </summary>
    /// <param name="isActive"> 蹴った相手を受け身不可状態にするか </param>
    public void NotPassiveKick(bool isActive)
    {
        m_isNotPassiveKick = isActive;
    }

    /// <summary>
    /// 受け身不可状態についての更新処理
    /// </summary>
    public void NotPassiveTimeUpdate()
    {
        if (m_isNotPassive)
        {
            m_notPassiveTimeCount -= Time.deltaTime;
            if (m_notPassiveTimeCount <= 0.0f)
            {
                m_notPassiveTimeCount = 0.0f;
                m_isNotPassive = false;
            }
        }
    }

    /// <summary>
    /// 受け身不可状態にする
    /// </summary>
    /// <param name="time"> 受け身不可状態の時間 </param>
    public virtual void SetNotPassive(float time)
    {
        if (m_notPassiveTimeCount < time)
        {
            m_isNotPassive = true;
            m_notPassiveTimeCount = time;
        }
    }

    /// <summary>
    /// プレイヤーがぶっ飛ばされた時のエフェクトを出し続けるか確認
    /// </summary>
    public void CheckBlownAwayEff()
    {
        ParticleSystem ps = m_blownawayEffObj.GetChild(0).GetComponent<ParticleSystem>();

        if (!ps.isPlaying) { return; }

        // 一定速度以下だったらエフェクトを隠し顔も戻す
        if (m_rb.velocity.sqrMagnitude <= m_blownawayEffHideThreshold * m_blownawayEffHideThreshold)
        {
            m_faceController.SetHitFace(false);
            ps.Stop();
        }
    }

    public void CheckVibration()
    {
        if (!m_isBlownVibration) { return; }

        float sqrThreshold = m_vibrationThreshold * m_vibrationThreshold;
        if (m_rb.velocity.sqrMagnitude <= sqrThreshold)
        {
            m_isBlownVibration = false;
        }
        else
        {
            float sqrMaxThreshold = m_vibrationMaxThreshold * m_vibrationMaxThreshold;
            float p = Mathf.Clamp(
                m_rb.velocity.sqrMagnitude,
                sqrThreshold,
                sqrMaxThreshold);
            p -= sqrThreshold;
            p /= sqrMaxThreshold - sqrThreshold;
            VibrationManager.Instance.SetVibration(GetComponent<PlayerId>().m_Id, 1, p);
        }
    }
}
