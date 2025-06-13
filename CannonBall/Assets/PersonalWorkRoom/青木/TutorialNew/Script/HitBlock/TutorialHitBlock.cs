using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TutorialHitBlock : MonoBehaviour, ITutorialHitBlock
{

    [Header("メッシュ設定")]
    [SerializeField]
    MeshRenderer[] m_renders;

    [Header("アセット設定")]
    [SerializeField, CustomLabel("衝突後のマテリアル")]
    Material m_hitMaterial;

    /// <summary> HitBlockが当たったか？ </summary>
    [SerializeField, CustomReadOnly]
    private ReactiveProperty<bool> m_isHit = new ReactiveProperty<bool>(false);

    /// <summary> HitBlockが当たったか？(Presenter参照用, Interface用) </summary>
    public IReadOnlyReactiveProperty<bool> GetIsSuccess() { return m_isHit; }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_isHit.Value)
        {
            return;
        }
        
        if (collision.gameObject.CompareTag("Player"))
        {

            // マテリアルの変更
            foreach (MeshRenderer mesh in m_renders)
            {
                mesh.material = m_hitMaterial;
            }

            // SE
            AudioManager.Instance.PlaySe("チュートリアル_矢印目標物", false);


            // 以降処理しないフラグ
            m_isHit.Value = true;
        }
    }
}
