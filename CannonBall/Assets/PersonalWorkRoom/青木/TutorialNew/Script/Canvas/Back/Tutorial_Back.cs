using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Back : InputElement
{

    [SerializeField, CustomLabel("モード選択に戻るボタンを押した時間を示す画像")]
    private Image m_backPressProgressImage;


    /// <summary> インプットアクション(戻る) </summary>
    private NewInputAction_Button m_inputAct_Cancel;

    [SerializeField, CustomLabel("選択画面からモード選択へ戻るのにかかる時間")]
    private float m_backPressTime = 3.0f;

    float m_backPressTimeCount = 0.0f;

    bool m_isSceneEnd = true;

    private void Start()
    {
        m_inputAct_Cancel = GetActionMap().GetAction_Button(NewInputActionName_ResultPlayer.Button.CursorRight.ToString());

    }
    private void Update()
    {
        if (m_inputAct_Cancel.GetPressAll())
        {
            m_backPressTimeCount -= Time.deltaTime;

            if (m_backPressTimeCount < 0.0f)
            {
                // 前のシーンに戻る
                GetComponent<SceneChanger>().StartSceneChange();

                m_isSceneEnd = true;
            }
            else
            {
                m_backPressProgressImage.fillAmount = m_backPressTimeCount / m_backPressTime - 0.01f;
            }
        }
        else
        {
            m_backPressTimeCount = m_backPressTime;
            m_backPressProgressImage.fillAmount = 0.0f;
        }

    }
}
