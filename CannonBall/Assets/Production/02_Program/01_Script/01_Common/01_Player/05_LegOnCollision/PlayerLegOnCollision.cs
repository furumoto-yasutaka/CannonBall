/*******************************************************************************
*
*	タイトル：	プレイヤーの足の当たり判定のイベントスクリプト
*	ファイル：	PlayerLegOnCollision.cs
*	作成者：	古本 泰隆
*	制作日：    2023/09/12
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLegOnCollision : MonoBehaviour
{
    /// <summary> リジッドボディ </summary>
    protected Rigidbody2D m_rb;

    /// <summary> プレイヤー入力受付 </summary>
    protected PlayerController m_playerController;

    /// <summary> プレイヤー入力受付 </summary>
    protected PlayerImpact m_playerImpact;

    /// <summary> 現在のフレームで地形を蹴ったか </summary>
    protected bool m_isKickPlatform = false;

    /// <summary> 蹴り中に当たったプレイヤー </summary>
    protected List<GameObject> m_contactList = new List<GameObject>();


    protected virtual void Start()
    {
        Transform p = transform.root;
        m_rb = p.GetComponent<Rigidbody2D>();
        m_playerController = p.GetComponent<PlayerController>();
        m_playerImpact = p.GetComponent<PlayerImpact>();
    }

    protected void Update()
    {
        m_isKickPlatform = false;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBody"))
        {// 敵プレイヤーを蹴った
            OnTriggerEnter_Player(other);
        }
        else if (other.CompareTag("Platform"))
        {// 地形を蹴った
            OnTriggerEnter_Platform(other);
        }
    }

    protected virtual void OnTriggerEnter_Player(Collider2D other)
    {
        m_playerImpact.KickPlayerRecoil();
        other.transform.root.GetComponent<PlayerImpact>().Kicked(
            m_playerController.m_KickDir,
            m_playerImpact.m_KickPlayerPower,
            m_rb.velocity);

        // 今回の蹴りで初めて当たったオブジェクトの場合はエフェクトを出す
        bool find = false;
        foreach (GameObject obj in m_contactList)
        {
            if (obj == other.gameObject)
            {
                find = true;
            }
        }
        if (!find)
        {
            m_contactList.Add(other.gameObject);
            int id = transform.root.GetComponent<PlayerId>().m_Id + 1;
            string type = PlayerController.m_TypeStr[(int)m_playerController.m_Type];
            Vector3 offset = m_playerController.m_KickDir * 0.5f;
            EffectContainer.Instance.EffectPlay(
                "ホログラムヒット時エフェクト_" + id + "P_" + type,
                other.ClosestPoint(transform.position + offset));
            AudioManager.Instance.PlaySe(
                "プレイヤー_ヒット音(プレイヤー等)_" + PlayerController.m_TypeStr[(int)m_playerController.m_Type],
                false);
            VibrationManager.Instance.SetVibration(id - 1, 8, 0.6f);
        }

        // 受け身不可状態になる蹴りだったら
        if (m_playerImpact.m_IsNotPassiveKick)
        {
            // 蹴られた相手を受け身不可状態にする
            other.transform.root.GetComponent<PlayerImpact>().SetNotPassive(m_playerImpact.m_NotPassiveTime);
        }
    }

    protected virtual void OnTriggerEnter_Platform(Collider2D other)
    {
        if (m_isKickPlatform) { return; }

        bool find = false;
        foreach (GameObject obj in m_contactList)
        {
            if (obj == other.gameObject)
            {
                find = true;
            }
        }
        if (!find)
        {
            m_contactList.Add(other.gameObject);
            m_isKickPlatform = true;
            m_playerImpact.KickPlatform();
            m_playerController.StartHeadbutt();
            int id = transform.root.GetComponent<PlayerId>().m_Id + 1;
            Vector3 offset = m_playerController.m_KickDir * 0.5f;
            EffectContainer.Instance.EffectPlay(
                "地形蹴り時_" + id + "P",
                other.ClosestPoint(transform.position + offset));
            AudioManager.Instance.PlaySe(
                "プレイヤー_ヒット音(地形)",
                false);
            VibrationManager.Instance.SetVibration(id - 1, 8, 0.3f);
        }
    }

    public void ResetContactList()
    {
        m_contactList.Clear();
    }
}
