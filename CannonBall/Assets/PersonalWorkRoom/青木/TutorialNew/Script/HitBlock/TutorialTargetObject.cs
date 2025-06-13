using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TutorialTargetObject : MonoBehaviour, ITutorialHitBlock
{
    [Header("�A�Z�b�g�ݒ�")]
    [SerializeField, CustomLabel("�Փˌ�̃}�e���A��")]
    Material m_hitMaterial;


    [SerializeField, CustomLabel("�ڕW����HP(�L�b�N�����)")]
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
        // �G�t�F�N�g����
        EffectContainer.Instance.EffectPlay("��T���h�o�b�O�R��ꎞ", transform.position);

        if (m_isSuccess.Value)
        {
            return;
        }
        if (collision.gameObject.name == "LegCollision")
        {
            m_kickHeath.Value--;

            if (m_kickHeath.Value <= 0)
            {
                // �}�e���A���̕ύX
                foreach (var p in m_lightMesh)
                {
                    p.material = m_hitMaterial;
                }

                // �ȍ~�������Ȃ��t���O
                m_isSuccess.Value = true;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �Փ˂����Ƃ��̃��A�N�V����
        HItReaction(collision);
    }

}
