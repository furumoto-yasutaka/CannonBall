using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Health : MonoBehaviour, IBomb
{
    #region フィールドパラメータ

    [SerializeField, CustomLabel("スポーン時の発射威力")]
    float m_spawPower;

    [SerializeField, CustomLabel("爆弾の残り時間(秒)")]
    float m_aliveTime;

    [SerializeField, CustomLabel("爆弾(回復)の威力")]
    float m_bombHealth;

    /// <summary> 爆弾が爆破したかしていないか </summary>
    bool m_isExprosition = false;

    BombCharacter m_bombCharacter;

    CinemachineImpulseSource m_ImpulseSource;

    #endregion

    #region プロパティ


    /// <summary> 爆弾が後どのくらいで爆発するのか </summary>
    /// <returns> 爆弾の残り時間(秒) </returns>
    public float GetAliveTime() { return m_aliveTime; }

    /// <summary> 爆弾が爆発しているのかどうか </summary>
    /// <returns> 爆弾が爆破したかしていないか </returns>
    public bool GetisExprosition() { return m_isExprosition; }

    /// <summary> 爆発でエリアが壊れる威力 </summary>
    /// <returns> 爆弾の威力 </returns>
    public float GetBombDamage() { return m_bombHealth; }


    /// <summary> 自分自身のゲームオブジェクト </summary>
    /// <returns> GameObject </returns>
    public GameObject GetGameObject() { return gameObject; }


    #endregion

    public void StartImpact(Vector3 _target)
    {
        GetComponent<Rigidbody2D>().velocity = _target * m_spawPower;
        //GetComponent<Rigidbody2D>().velocity = new Vector3(Mathf.Cos(_rotation), Mathf.Sin(_rotation), 0.0f) * m_spawPower;
        //Debug.Log("Cos" + Mathf.Cos(_rotation));
        //Debug.Log("Sin" + Mathf.Sin(_rotation));
    }

    private void Start()
    {
        m_bombCharacter = GetComponent<BombCharacter>();

        m_ImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        // すでにシーケンスに爆発していたら
        if (m_isExprosition)
        {
            return;
        }
        // ゲームの時間が終わっていたら
        if (Timer.Instance.m_TimeCounter <= 0.1f)
        {
            return;
        }



        m_aliveTime -= Time.deltaTime * BombManager.Instance.BombMultSpeed;

        if (m_aliveTime < 0.0f)
        {
            m_aliveTime = 0.0f;

            // 爆発した瞬間
            if (!m_isExprosition)
            {
                transform.GetChild(0).gameObject.SetActive(false);

                m_isExprosition = true;

            }
        }


        if (m_isExprosition)
        {
            // エフェクトを出現
            EffectContainer.Instance.EffectPlay("回復爆発", transform.position);

            // エリアのHPを減らす
            BombGame_PlayAreaHealthManager.Instance.AddHealth(m_bombCharacter.m_InAreaNumber, m_bombHealth);

            // 爆弾リストの要素を削除
            BombManager.Instance.RemoveBombList(m_bombCharacter);

            // 爆弾の振動をカメラに伝える
            m_ImpulseSource.GenerateImpulse();

            //// オブジェクトを破壊
            Destroy(gameObject);
        }
    }





}
