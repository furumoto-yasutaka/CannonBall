using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TutorialKickBlock : MonoBehaviour, ITutorialHitBlock
{
    [Header("�A�Z�b�g�ݒ�")]
    [SerializeField, CustomLabel("�Փˌ�̃}�e���A��")]
    Material m_hitMaterial;

    /// <summary> HitBlock�������������H </summary>
    [SerializeField, CustomReadOnly]
    private ReactiveProperty<bool> m_isHit = new ReactiveProperty<bool>(false);

    MeshRenderer m_hitRenderer;

    /// <summary> HitBlock�������������H(Presenter�Q�Ɨp, Interface�p) </summary>
    public IReadOnlyReactiveProperty<bool> GetIsSuccess() { return m_isHit; }

    private void Start()
    {
        m_hitRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �G�t�F�N�g����
        EffectContainer.Instance.EffectPlay("�T���h�o�b�O�R��ꎞ", transform.position);

        if (m_isHit.Value)
        {
            return;
        }
        if (collision.gameObject.name == "LegCollision")
        {
            // �}�e���A���̕ύX
            if (m_hitRenderer)
            {
                m_hitRenderer.material = m_hitMaterial;
            }


            // �ȍ~�������Ȃ��t���O
            m_isHit.Value = true;
        }
    }
}
