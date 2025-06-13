/*******************************************************************************
*
*	タイトル：	爆弾のデータ　インターフェースに変更するかも？？？
*	ファイル：	BombCharacter.cs
*	作成者：	青木 大夢
*	制作日：    2023/09/21
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Bomb_CountDown : MonoBehaviour, IBomb
{
    #region フィールドパラメータ

    [SerializeField, CustomLabel("スポーン時の発射威力")]
    float m_spawPower;

    [SerializeField, CustomLabel("爆弾の残り時間(秒)")]
    float m_aliveTime;

    [SerializeField, CustomLabel("爆弾の威力")]
    float m_bombDamage;

    /// <summary> 爆弾が爆破したかしていないか </summary>
    bool m_isExprosition = false;

    BombCharacter m_bombCharacter;

    CinemachineImpulseSource m_ImpulseSource;

    [SerializeField]
    string m_exprosionEffectName = "爆弾爆発_小";

    [SerializeField]
    string m_exprosionSoundName = "爆弾爆発_小";

    [SerializeField, CustomLabel("爆発の振動時間")]
    private int m_vibrationFrame;

    [SerializeField, CustomLabel("爆発の振動の強さ")]
    private float m_vibrationPower;

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
    public float GetBombDamage() { return m_bombDamage; }


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

        float timer = Timer.Instance.m_TimeCounter;
        float bombSpeed = BombManager.Instance.BombMultSpeed;

        // ゲーム終了時間ー爆発時間が少なくなりすぎないようにする
        float amountTime = 13.0f;
        if (timer - m_aliveTime <= amountTime)    // amountTime秒以上減らさない
        {
            m_aliveTime = amountTime * bombSpeed;
        }


        // この爆弾が爆発した場合、残り時間が1秒とか短すぎてしまう場合、爆弾の残り時間を増やす
        float endSpawTime = 20 * bombSpeed;         // 最後にスポーンさせる限界時間
        if ((timer * bombSpeed)  <= endSpawTime)   // endSpawTimeより、時間の方が小さかったら、時間分爆弾の残り時間を増やす
        {
            m_aliveTime = timer * bombSpeed - 0.1f;
        }


        //// 爆発時間をゲーム終了時間に合わせる
        //if ((timer * bombSpeed) < m_aliveTime)
        //{
        //    m_aliveTime = (timer * bombSpeed) - 0.1f;
        //}

    }

    private void Update()
    {
        if (m_isExprosition)
        {
            return;
        }

        // ゲームの時間が終わっていたら
        if (Timer.Instance.m_TimeCounter <= 0.01f)
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
            EffectContainer.Instance.EffectPlay(m_exprosionEffectName, transform.position);

            // サウンド
            AudioManager.Instance.PlaySe(m_exprosionSoundName, false);

            // コントローラー振動
            VibrationManager.Instance.SetVibration(m_bombCharacter.m_InAreaNumber, m_vibrationFrame, m_vibrationPower);

            // エリアのHPを減らす
            BombGame_PlayAreaHealthManager.Instance.SubHealth(m_bombCharacter.m_InAreaNumber, m_bombDamage);

            //// 次の爆弾をスポーン
            //BombManager.Instance.m_spawFlag = true;

            // 爆弾リストの要素を削除
            BombManager.Instance.RemoveBombList(m_bombCharacter);

            // 爆弾の振動をカメラに伝える
            m_ImpulseSource.GenerateImpulse();

            //// オブジェクトを破壊
            Destroy(gameObject);
        }
    }


}
