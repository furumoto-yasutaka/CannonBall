using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TutorialTargetObject : MonoBehaviour, ITutorialHitBlock
{
    [Header("アセット設定")]
    [SerializeField, CustomLabel("衝突後のマテリアル")]
    Material m_hitMaterial;


    [SerializeField, CustomLabel("目標物のHP(キックする回数)")]
    int m_heathCount = 30;

    [SerializeField]
    MeshRenderer[] m_lightMesh;


    private ReactiveProperty<int> m_kickHeath = new ReactiveProperty<int>(0);
    public IReadOnlyReactiveProperty<int> m_KickHeath;


    [SerializeField, CustomReadOnly]
    private ReactiveProperty<bool> m_isSuccess = new ReactiveProperty<bool>(false);

    public IReadOnlyReactiveProperty<bool> GetIsSuccess() { return m_isSuccess; }

    private void Awake()
    {
        m_kickHeath.Value = m_heathCount;
        m_KickHeath = m_kickHeath;
    }


    private void HItReaction(Collider2D collision)
    {
        // エフェクト生成
        EffectContainer.Instance.EffectPlay("大サンドバッグ蹴られ時", transform.position);

        if (m_isSuccess.Value)
        {
            return;
        }
        if (collision.gameObject.name == "LegCollision")
        {
            m_kickHeath.Value--;

            if (m_kickHeath.Value <= 0)
            {
                // マテリアルの変更
                foreach (var p in m_lightMesh)
                {
                    p.material = m_hitMaterial;
                }

                // 以降処理しないフラグ
                m_isSuccess.Value = true;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 衝突したときのリアクション
        HItReaction(collision);
    }

}
