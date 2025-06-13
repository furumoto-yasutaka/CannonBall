using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TutorialKickBlock : MonoBehaviour, ITutorialHitBlock
{
    [Header("アセット設定")]
    [SerializeField, CustomLabel("衝突後のマテリアル")]
    Material m_hitMaterial;

    /// <summary> HitBlockが当たったか？ </summary>
    [SerializeField, CustomReadOnly]
    private ReactiveProperty<bool> m_isHit = new ReactiveProperty<bool>(false);

    MeshRenderer m_hitRenderer;

    /// <summary> HitBlockが当たったか？(Presenter参照用, Interface用) </summary>
    public IReadOnlyReactiveProperty<bool> GetIsSuccess() { return m_isHit; }

    private void Start()
    {
        m_hitRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // エフェクト生成
        EffectContainer.Instance.EffectPlay("サンドバッグ蹴られ時", transform.position);

        if (m_isHit.Value)
        {
            return;
        }
        if (collision.gameObject.name == "LegCollision")
        {
            // マテリアルの変更
            if (m_hitRenderer)
            {
                m_hitRenderer.material = m_hitMaterial;
            }


            // 以降処理しないフラグ
            m_isHit.Value = true;
        }
    }
}
