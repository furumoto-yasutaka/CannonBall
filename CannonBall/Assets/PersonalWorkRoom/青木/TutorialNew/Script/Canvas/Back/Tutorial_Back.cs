using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Back : InputElement
{

    [SerializeField, CustomLabel("���[�h�I���ɖ߂�{�^�������������Ԃ������摜")]
    private Image m_backPressProgressImage;


    /// <summary> �C���v�b�g�A�N�V����(�߂�) </summary>
    private NewInputAction_Button m_inputAct_Cancel;

    [SerializeField, CustomLabel("�I����ʂ��烂�[�h�I���֖߂�̂ɂ����鎞��")]
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
                // �O�̃V�[���ɖ߂�
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
