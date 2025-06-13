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

        // テキスト送りを止める
        m_textFeed.enabled = false;
    }

    private void Update()
    {
        if (m_canvasGroup.alpha >= 0.999f)
        {
            // テキスト送りを始める
            m_textFeed.enabled = true;

            // 以降このスクリプトは起動しないので、削除
            this.enabled = false;
        }
    }

}
