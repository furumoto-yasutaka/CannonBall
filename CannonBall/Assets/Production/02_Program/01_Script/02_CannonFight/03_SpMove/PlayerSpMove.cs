using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerSpMove : MonoBehaviour
{
    #region 必殺技ポイント関連パラメータ

    /// <summary> 必殺技ポイント最大値 </summary>
    [SerializeField, CustomLabel("必殺技ポイント最大値")]
    private float m_spMovePointMax = 100.0f;

    /// <summary> 必殺技ポイント </summary>
    [SerializeField, CustomLabel("必殺技ポイント")]
    private float m_spMovePoint = 0.0f;

    [Header("蹴られた時")]
    /// <summary> 蹴られた時に溜まるようにするか </summary>
    [SerializeField, CustomLabel("有効にするか")]
    private bool m_isUseBeKickedPattern;

    /// <summary> 蹴られた時のゲージの上昇率 </summary>
    [SerializeField, CustomLabel("上昇率")]
    private float m_bekickedPatternRateOfUp;

    [Header("敵プレイヤーを蹴った時")]
    /// <summary> 敵プレイヤーを蹴った時に溜まるようにするか </summary>
    [SerializeField, CustomLabel("有効にするか")]
    private bool m_isUseKickPattern;

    /// <summary> 敵プレイヤーを蹴った時のゲージの上昇率 </summary>
    [SerializeField, CustomLabel("上昇率")]
    private float m_kickPatternRateOfUp;

    [Header("キルされた時")]
    /// <summary> キルされた時に溜まるようにするか </summary>
    [SerializeField, CustomLabel("有効にするか")]
    private bool m_isUseBeKilledPattern;

    /// <summary> キルされた時のゲージの上昇率 </summary>
    [SerializeField, CustomLabel("上昇率")]
    private float m_bekilledPatternRateOfUp = 10.0f;

    [Header("時間経過")]
    /// <summary> 時間経過に溜まるようにするか </summary>
    [SerializeField, CustomLabel("有効にするか")]
    private bool m_isUseTimePattern;

    /// <summary> 時間経過のゲージの上昇率(0.1秒毎) </summary>
    [SerializeField, CustomLabel("上昇率(0.1秒毎)")]
    private float m_timePatternRateOfUp = 0.05f;

    /// <summary> 時間経過のゲージの上昇率 </summary>
    [SerializeField, CustomArrayLabel(new string[] { "1位","2位","3位","4位", })]
    private float[] m_timePatternRankMag = new float[] { 1.0f, 1.25f, 1.5f, 2.0f, };

    #endregion

    #region 必殺技中パラメータ

    [Header("必殺技中パラメータ")]

    /// <summary> 必殺技発動可能エフェクト </summary>
    [SerializeField, CustomLabel("必殺技発動可能エフェクト")]
    private GameObject m_isCanSpMoveEffect;

    /// <summary> 必殺技発動可能になった瞬間のエフェクト </summary>
    [SerializeField, CustomLabel("必殺技発動可能になった瞬間のエフェクト")]
    private Transform m_spmoveChargeEffect;

    /// <summary> 必殺技発動時間 </summary>
    [SerializeField, CustomLabel("発動時間")]
    private float m_spMoveActivationTime;

    [Space(8)]
    /// <summary> 足を突き出す速さを変化するか </summary>
    [SerializeField, CustomLabel("足を突き出す速さを変化するか")]
    private bool m_isChangeDuringSpMove_KickStickOutSpeed;
    /// <summary> 足を突き出す速さ </summary>
    [SerializeField, CustomLabel("足を突き出す速さ")]
    private float m_duringSpMove_KickStickOutSpeed;

    [Space(8)]
    /// <summary> 足を戻す速さを変化するか </summary>
    [SerializeField, CustomLabel("足を戻す速さを変化するか")]
    private bool m_isChangeDuringSpMove_KickRetractSpeed;
    /// <summary> 足を戻す速さ </summary>
    [SerializeField, CustomLabel("足を戻す速さ")]
    private float m_duringSpMove_KickRetractSpeed;

    [Space(8)]
    /// <summary> 地面を蹴る力を変化するか </summary>
    [SerializeField, CustomLabel("地面を蹴る力を変化するか")]
    private bool m_isChangeDuringSpMove_KickPlatformPower;
    /// <summary> 地面を蹴る力 </summary>
    [SerializeField, CustomLabel("地面を蹴る力")]
    private float m_duringSpMove_KickPlatformPower;

    [Space(8)]
    /// <summary> 敵を蹴る力を変化するか </summary>
    [SerializeField, CustomLabel("敵を蹴る力を変化するか")]
    private bool m_isChangeDuringSpMove_KickPlayerPower;
    /// <summary> 敵を蹴る力 </summary>
    [SerializeField, CustomLabel("敵を蹴る力")]
    private float m_duringSpMove_KickPlayerPower;

    [Space(8)]
    /// <summary> 蹴った相手を受け身不可状態にするかを変化するか </summary>
    [SerializeField, CustomLabel("蹴った相手を受け身不可状態にするかを変化するか")]
    private bool m_isChangeDuringSpMove_IsNotPassiveKick;
    /// <summary> 蹴った相手を受け身不可状態にするか </summary>
    [SerializeField, CustomLabel("蹴った相手を受け身不可状態にするか")]
    private bool m_duringSpMove_IsNotPassiveKick;

    [Space(8)]
    /// <summary> 受け身不可にする時間を変化するか </summary>
    [SerializeField, CustomLabel("受け身不可にする時間を変化するか")]
    private bool m_isChangeDuringSpMove_NotPassiveTime;
    /// <summary> 受け身不可にする時間 </summary>
    [SerializeField, CustomLabel("受け身不可にする時間")]
    private float m_duringSpMove_NotPassiveTime;

    #endregion

    #region プライベートメンバ変数

    /// <summary> 必殺技増加量増加タイムかどうか </summary>
    public static bool m_IsChargeAddition = false;

    /// <summary> 必殺技増加量2倍タイムかどうか </summary>
    protected static float m_isChargeAdditionRate = 2.0f;

    /// <summary> 必殺技ゲージが溜まりきっているか </summary>
    protected bool m_isSpMovePointMaxCharge = false;

    /// <summary> 必殺技発動中かどうか </summary>
    protected bool m_isSpMove = false;

    /// <summary> 必殺技発動中かどうか </summary>
    protected float m_spMoveActivationTimeCount = 0.0f;

    /// <summary> 必殺技ポイント溜まり割合(0.0~1.0) </summary>
    protected ReactiveProperty<float> m_spMovePointRate = new ReactiveProperty<float>(0.0f);

    /// <summary> プレイヤーポイントコンポーネント </summary>
    protected PlayerPoint_CannonFight m_playerPoint;

    /// <summary> プレイヤー制御コンポーネント </summary>
    protected PlayerController_CannonFight m_playerController;

    /// <summary> プレイヤー制御コンポーネント </summary>
    protected PlayerImpact_CannonFight m_playerImpact;

    #endregion

    #region ゲッター

    public IReadOnlyReactiveProperty<float> m_SpMovePointRate => m_spMovePointRate;

    public bool m_IsSpMovePointMaxCharge { get { return m_isSpMovePointMaxCharge; } }

    public bool m_IsSpMove { get { return m_isSpMove; } }

    public bool m_IsChangeDuringSpMove_KickStickOutSpeed { get { return m_isChangeDuringSpMove_KickStickOutSpeed; } }

    public float m_DuringSpMove_KickStickOutSpeed { get { return m_duringSpMove_KickStickOutSpeed; } }

    public bool m_IsChangeDuringSpMove_KickRetractSpeed { get { return m_isChangeDuringSpMove_KickRetractSpeed; } }

    public float m_DuringSpMove_KickRetractSpeed { get { return m_duringSpMove_KickRetractSpeed; } }

    public bool m_IsChangeDuringSpMove_KickPlatformPower { get { return m_isChangeDuringSpMove_KickPlatformPower; } }

    public float m_DuringSpMove_KickPlatformPower { get { return m_duringSpMove_KickPlatformPower; } }

    public bool m_IsChangeDuringSpMove_KickPlayerPower { get { return m_isChangeDuringSpMove_KickPlayerPower; } }

    public float m_DuringSpMove_KickPlayerPower { get { return m_duringSpMove_KickPlayerPower; } }

    public bool m_IsChangeDuringSpMove_IsNotPassiveKick { get { return m_isChangeDuringSpMove_IsNotPassiveKick; } }

    public bool m_DuringSpMove_IsNotPassiveKick { get { return m_duringSpMove_IsNotPassiveKick; } }

    public bool m_IsChangeDuringSpMove_NotPassiveTime { get { return m_isChangeDuringSpMove_NotPassiveTime; } }

    public float m_DuringSpMove_NotPassiveTime { get { return m_duringSpMove_NotPassiveTime; } }

    #endregion


    protected virtual void Start()
    {
        m_playerPoint = GetComponent<PlayerPoint_CannonFight>();
        m_playerController = GetComponent<PlayerController_CannonFight>();
        m_playerImpact = GetComponent<PlayerImpact_CannonFight>();
    }

    protected virtual void Update()
    {
        if (m_isSpMove)
        {
            // 必殺技発動残り時間の更新
            UpdateSpMoveTime();
        }
        else
        {
            if (m_playerController.m_State == PlayerController.State.Play)
            {
                // 時間経過による必殺技ゲージ増加処理
                AccumulateTimePattern();
            }
        }
    }

    /// <summary> 蹴られた時によるゲージ増加 </summary>
    public void AccumulateBeKickedPattern()
    {
        if (!m_isUseBeKickedPattern) { return; }

        if (!m_isSpMove && !m_isSpMovePointMaxCharge)
        {
            if (m_IsChargeAddition)
            {
                m_spMovePoint += m_bekickedPatternRateOfUp * m_isChargeAdditionRate;
            }
            else
            {
                m_spMovePoint += m_bekickedPatternRateOfUp;
            }
            CheckSpMovePointMaxCharge();
            m_spMovePointRate.Value = m_spMovePoint / m_spMovePointMax;
        }
    }

    /// <summary> 敵プレイヤーを蹴った時によるゲージ増加 </summary>
    public void AccumulateKickPattern()
    {
        if (!m_isUseKickPattern) { return; }

        if (!m_isSpMove && !m_isSpMovePointMaxCharge)
        {
            if (m_IsChargeAddition)
            {
                m_spMovePoint += m_kickPatternRateOfUp * m_isChargeAdditionRate;
            }
            else
            {
                m_spMovePoint += m_kickPatternRateOfUp;
            }
            CheckSpMovePointMaxCharge();
            m_spMovePointRate.Value = m_spMovePoint / m_spMovePointMax;
        }
    }

    /// <summary> 敵プレイヤーに倒された時によるゲージ増加 </summary>
    public void AccumulateBeKilledPattern()
    {
        if (!m_isUseBeKilledPattern) { return; }

        if (!m_isSpMove && !m_isSpMovePointMaxCharge)
        {
            if (m_IsChargeAddition)
            {
                m_spMovePoint += m_bekilledPatternRateOfUp * m_isChargeAdditionRate;
            }
            else
            {
                m_spMovePoint += m_bekilledPatternRateOfUp;
            }
            CheckSpMovePointMaxCharge();
            m_spMovePointRate.Value = m_spMovePoint / m_spMovePointMax;
        }
    }

    /// <summary> 時間経過によるゲージ増加 </summary>
    private void AccumulateTimePattern()
    {
        if (!m_isUseTimePattern) { return; }

        if (!m_isSpMove && !m_isSpMovePointMaxCharge)
        {
            if (m_IsChargeAddition)
            {
                m_spMovePoint += m_timePatternRateOfUp * (Time.deltaTime / 0.1f) * m_timePatternRankMag[m_playerPoint.m_Rank - 1] * m_isChargeAdditionRate;
            }
            else
            {
                m_spMovePoint += m_timePatternRateOfUp * (Time.deltaTime / 0.1f) * m_timePatternRankMag[m_playerPoint.m_Rank - 1];
            }
            CheckSpMovePointMaxCharge();
            m_spMovePointRate.Value = m_spMovePoint / m_spMovePointMax;
        }
    }

    /// <summary> 必殺技ポイントが溜まったかどうか確認する </summary>
    private void CheckSpMovePointMaxCharge()
    {
        // 必殺技ポイントが溜まりきっているか
        if (m_spMovePoint >= m_spMovePointMax)
        {
            m_spMovePoint = m_spMovePointMax;
            m_isSpMovePointMaxCharge = true;
            m_isCanSpMoveEffect.SetActive(true);
            m_isCanSpMoveEffect.GetComponent<Animator>().SetBool("IsShow", true);
            m_spmoveChargeEffect.GetChild(0).GetComponent<ParticleSystem>().Play();
            AudioManager.Instance.PlaySe(
                "キャノンファイト_必殺ゲージ満タン時",
                false);
            // コントローラー振動
            VibrationManager.Instance.SetVibration(GetComponent<PlayerId>().m_Id, 40, 0.7f);
        }
    }

    /// <summary> 必殺技を発動する </summary>
    public virtual void StartSpMove()
    {
        m_isSpMovePointMaxCharge = false;
        m_isCanSpMoveEffect.SetActive(false);
        m_isCanSpMoveEffect.GetComponent<Animator>().SetBool("IsShow", false);
        m_isSpMove = true;
        m_spMoveActivationTimeCount = m_spMoveActivationTime;
        // 必殺技発動中用パラメータを反映
        m_playerController.SetDuringSpMoveParam();
        m_playerImpact.SetDuringSpMoveParam();
    }

    /// <summary> 必殺技の残り時間を更新 </summary>
    private void UpdateSpMoveTime()
    {
        m_spMoveActivationTimeCount -= Time.deltaTime;

        // 必殺技の残り時間が無くなったら
        if (m_spMoveActivationTimeCount <= 0.0f)
        {
            m_spMovePointRate.Value = 0.0f;
            // 必殺技を終了
            EndSpMove();
            ResetSpMovePoint();
        }
        else
        {
            m_spMovePointRate.Value = m_spMoveActivationTimeCount / m_spMoveActivationTime;
        }
    }

    /// <summary> 必殺技を終了する </summary>
    public virtual void EndSpMove()
    {
        m_isSpMove = false;
        m_spMoveActivationTimeCount = 0.0f;
        // 必殺技発動中用パラメータを反映
        m_playerController.SetDefaultParam();
        m_playerImpact.SetDefaultParam();
    }

    /// <summary> 必殺技ポイントをリセットする </summary>
    public void ResetSpMovePoint()
    {
        m_spMovePoint = 0.0f;
        m_spMovePointRate.Value = 0.0f;
        m_isSpMovePointMaxCharge = false;
        m_isCanSpMoveEffect.SetActive(false);
        m_isCanSpMoveEffect.GetComponent<Animator>().SetBool("IsShow", false);
    }
}
