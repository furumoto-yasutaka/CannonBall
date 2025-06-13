using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TutorialHitBlock : MonoBehaviour, ITutorialHitBlock
{

    [Header("���b�V���ݒ�")]
    [SerializeField]
    MeshRenderer[] m_renders;

    [Header("�A�Z�b�g�ݒ�")]
    [SerializeField, CustomLabel("�Փˌ�̃}�e���A��")]
    Material m_hitMaterial;

    /// <summary> HitBlock�������������H </summary>
    [SerializeField, CustomReadOnly]
    private ReactiveProperty<bool> m_isHit = new ReactiveProperty<bool>(false);

    /// <summary> HitBlock�������������H(Presenter�Q�Ɨp, Interface�p) </summary>
    public IReadOnlyReactiveProperty<bool> GetIsSuccess() { return m_isHit; }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_isHit.Value)
        {
            return;
        }
        
        if (collision.gameObject.CompareTag("Player"))
        {

            // �}�e���A���̕ύX
            foreach (MeshRenderer mesh in m_renders)
            {
                mesh.material = m_hitMaterial;
            }

            // SE
            AudioManager.Instance.PlaySe("�`���[�g���A��_���ڕW��", false);


            // �ȍ~�������Ȃ��t���O
            m_isHit.Value = true;
        }
    }
}
