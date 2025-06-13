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

public class Bomb_BubbleCountDown : MonoBehaviour, IBomb
{
    #region フィールドパラメータ
  
    [SerializeField, CustomLabel("スポーン時の発射威力")]
    float m_spawPower;

    [SerializeField, CustomLabel("爆弾の残り時間(秒)")]
    private float m_aliveTime;

    [SerializeField, CustomLabel("爆弾の威力")]
    private float m_bombDamage;

    /// <summary> 爆弾が爆破したかしていないか </summary>
    private bool m_isExprosition = false;

    private BombCharacter m_bombCharacter;

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
    public float GetBombDamage() {  return m_bombDamage; }

    #endregion

    public void StartImpact(Vector3 _target)
    {
        //float randAngle = 5.0f;
        //
        //float rand = Random.Range(-randAngle, randAngle);
        //rand *= Mathf.Deg2Rad;


        GetComponent<Rigidbody2D>().velocity = _target * m_spawPower;
        //GetComponent<Rigidbody2D>().velocity = new Vector3(Mathf.Cos(_rotation), Mathf.Sin(_rotation), 0.0f) * m_spawPower;
    }


    private void Start()
    {
        m_bombCharacter = GetComponent<BombCharacter>();
    }

    private void Update()
    {
        if (m_isExprosition)
        {
            return;
        }



        m_aliveTime -= Time.deltaTime;

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
            EffectContainer.Instance.EffectPlay("爆発", transform.position);

            // エリアのHPを減らす
            BombGame_PlayAreaHealthManager.Instance.SubHealth(m_bombCharacter.m_InAreaNumber, m_bombDamage);

            // 爆弾リストの要素を削除
            BombManager.Instance.RemoveBombList(m_bombCharacter);

            //// オブジェクトを破壊
            Destroy(gameObject);
        }
    }


}
