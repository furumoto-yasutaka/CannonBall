using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvasGuide : MonoBehaviour
{
    [SerializeField]
    TextFeed m_textFeed;

    CanvasGroup m_canvasGroup;

    private void Start()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();

        // �e�L�X�g������~�߂�
        m_textFeed.enabled = false;
    }

    private void Update()
    {
        if (m_canvasGroup.alpha >= 0.999f)
        {
            // �e�L�X�g������n�߂�
            m_textFeed.enabled = true;

            // �ȍ~���̃X�N���v�g�͋N�����Ȃ��̂ŁA�폜
            this.enabled = false;
        }
    }

}
