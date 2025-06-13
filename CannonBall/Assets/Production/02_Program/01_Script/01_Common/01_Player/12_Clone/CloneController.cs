using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : MonoBehaviour
{
    /// <summary> 足オブジェクト </summary>
    [SerializeField, CustomLabel("足オブジェクト")]
    private GameObject m_leg;
    /// <summary> 足の当たり判定オブジェクト </summary>
    [SerializeField, CustomLabel("足の当たり判定オブジェクト")]
    private Collider2D m_legCollision;
    /// <summary> 足の親オブジェクト </summary>
    [SerializeField, CustomLabel("足の親オブジェクト")]
    private GameObject m_legParent;
    /// <summary> 蹴り時のエフェクトの親オブジェクト </summary>
    [SerializeField, CustomLabel("蹴り時のエフェクトの親オブジェクト")]
    protected Transform m_kickEffectParent;

    private OriginInfo m_originInfo;
    /// <summary> アニメーターコンポーネント </summary>
    private Animator m_animator;
    /// <summary> リジッドボディコンポーネント </summary>
    private Rigidbody2D m_rb;
    /// <summary> プレイヤーの足の判定コールバック </summary>
    private PlayerLegOnCollision m_playerLegOnCollision;
    /// <summary> プレイヤー表情制御コンポーネント </summary>
    private PlayerFaceController m_faceController;
    /// <summary> 蹴る方向 </summary>
    private Vector2 m_kickDir = Vector2.zero;


    private void Start()
    {
        m_originInfo = GetComponent<OriginInfo>();
        m_animator = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody2D>();
        m_playerLegOnCollision = m_legCollision.GetComponent<PlayerLegOnCollision>();
        m_faceController = GetComponent<PlayerFaceController>();

        // 蹴りの速さをアニメーションコントローラーに反映
        m_animator.SetFloat("StickOutSpeed", m_originInfo.m_PlayerController.m_KickStickOutSpeed);
        m_animator.SetFloat("RetractSpeed", m_originInfo.m_PlayerController.m_KickRetractSpeed);
    }

    private void Update()
    {
        // 体の回転に持っていかれないように足の方向を補正する
        m_legParent.transform.rotation =
            Quaternion.identity * Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.down, m_kickDir), Vector3.forward);
    }

    public void Move(float addSpeed)
    {
        // 速度反映
        m_rb.velocity += new Vector2(addSpeed, 0.0f);
    }

    public void Kick(Vector2 kickDir)
    {
        m_kickDir = kickDir;
        // 蹴りを開始
        m_leg.SetActive(true);
        m_legCollision.enabled = true;
        m_animator.SetTrigger("Kick");

        string type = PlayerController.m_TypeStr[(int)m_originInfo.m_PlayerController.m_Type];
        EffectContainer.Instance.EffectPlay(
            "蹴り風切り_" + type,
            m_kickEffectParent.position,
            m_kickEffectParent.rotation,
            m_kickEffectParent);
        m_faceController.SetAngryFace(true);
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
        // 足の衝突済みリストをリセットする
        m_playerLegOnCollision.ResetContactList();
        m_faceController.SetAngryFace(false);
    }

    /// <summary>
    /// 消滅開始
    /// </summary>
    public void StartDestroy()
    {
        transform.GetChild(0).GetComponent<Animator>().SetBool("IsDestroy", true);
    }
}
