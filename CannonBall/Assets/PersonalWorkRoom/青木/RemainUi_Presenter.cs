using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RemainUi_Presenter : MonoBehaviour
{
    // View     -------------------------------------
    /// <summary> �l�e�L�X�g </summary>
    [SerializeField]
    private RemainUi_View_ValueText m_viewValueText;

    /// <summary> ���o�p�e�L�X�g </summary>
    //[SerializeField]
   // private RemainUpUi_View_Staging m_viewStaging;


    // Model    -------------------------------------
    /// <summary> �v���C���[ </summary>
    [SerializeField]
    private PlayerController m_player;


    /// <summary> �������ɂ���ăI�u�U�[�o�[���������v��Ȃ����������Ȃ����߂̃t���O </summary>
    private bool m_isOnce = true;


    void Start()
    {
        // �ϐ����Ď����ĕύX���ɓK�X�X�V���s�킹��

        // Model -> View_Gauge�EView_ValueText
        RemainManager.Instance.m_Remain.Subscribe(v =>
            {
                m_viewValueText.SetText(v);

                // �c�@�������o�͍ŏ��͕\�����Ȃ�
                if (m_isOnce)
                {
                    m_isOnce = false;
                    return;
                }

                //m_viewStaging.CreateUi();
            })
            .AddTo(this);
    }
}
