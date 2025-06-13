using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TutorialHitBlockPresenter : MonoBehaviour
{
    [Header("���I�u�W�F�N�g���Q��")]
    [SerializeField, CustomLabel("���j�^�[(�����Ă��G���[�o�Ȃ�)")]
    TutorialSuccessMonitor m_successMonitor;

    TutorialHitBlockManager tutorialHitBlockManager;

    ITutorialHitBlock[] m_hitBlocks;

    ReactiveProperty<int> m_success = new ReactiveProperty<int>(0);

    void Start()
    {
        // �擾
        m_hitBlocks = new ITutorialHitBlock[transform.childCount];
        for (int i = 0; i < m_hitBlocks.Length; i++)
        {
            m_hitBlocks[i] = transform.GetChild(i).GetComponent<ITutorialHitBlock>();
        }
        // �擾
        tutorialHitBlockManager = transform.parent.GetComponent<TutorialHitBlockManager>();

        // �ǂɓ���������
        foreach (var item in m_hitBlocks)
        {
            item.GetIsSuccess().Subscribe(_ =>
            {
                if (item.GetIsSuccess().Value == true)
                {
                    // �����J�E���g
                    m_success.Value++;
                }
            }).AddTo(this);
        }

        // �S�Ă̕ǂɓ���������
        m_success.Subscribe(_ =>
        {
            if (m_success.Value >= m_hitBlocks.Length)
            {
                // �Ǘ��N���X�ɃJ�E���g
                tutorialHitBlockManager.AddHitCount();

                // ���j�^�[��ݒ�
                if (m_successMonitor != null)
                {
                    m_successMonitor.SetOKSprite();
                    m_successMonitor.SetColor();
                }

            }
        }).AddTo(this);
        
    }

}
