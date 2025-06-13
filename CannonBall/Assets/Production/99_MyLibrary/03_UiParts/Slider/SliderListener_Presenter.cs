/*******************************************************************************
*
*	�^�C�g���F	�X���C�_�[���X�i�[���̋��n��
*	�t�@�C���F	SliderListener_Presenter.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/05/11
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SliderListener_Presenter : MonoBehaviour
{
    // View & Model -------------------------------------
    /// <summary> �X���C�_�[�e�I�u�W�F�N�g </summary>
    [SerializeField]
    protected Transform m_sliderParent;


    // Resource    -------------------------------------
    /// <summary> ���W�A�C�e���A�C�R��(���擾) </summary>
    [SerializeField, CustomLabel("�p�[�Z���g�摜")]
    protected Sprite m_percentSprite;

    [SerializeField, CustomLabel("���l�摜")]
    protected Sprite[] m_numberSprite;

    [SerializeField, CustomLabel("�v���}�C�摜")]
    protected Sprite m_plusMinusSprite;

    [SerializeField, CustomLabel("�v���X�摜")]
    protected Sprite m_plusSprite;

    [SerializeField, CustomLabel("�}�C�i�X�摜")]
    protected Sprite m_minusSprite;


    protected virtual void Start()
    {
        // �e�X���C�_�[�̒i�K�ω����Ď��E������
        for (int i = 0; i < m_sliderParent.childCount; i++)
        {
            SliderListener listener;
            if (!m_sliderParent.GetChild(i).TryGetComponent(out listener))
            {
                continue;
            }

            listener.m_StepReactiveProperty.Subscribe(v =>
                {
                    SetText(listener);
                })
                .AddTo(this);

            SetText(listener);
        }
    }

    /// <summary>
    /// �X���C�_�[�̒l�e�L�X�g�ݒ�
    /// </summary>
    /// <param name="listener"> �Ώۂ̃X���C�_�[�̃��X�i�[ </param>
    private void SetText(SliderListener listener)
    {
        if (listener.m_ValueTextPattern == SliderListener.ValueTextPattern.Persent)
        {
            if (listener.m_IsTextUseTMP)
            {
                listener.SetTMPText_Persent();
            }
            else
            {
                listener.SetText_Persent(m_numberSprite, m_percentSprite);
            }
        }
        else if (listener.m_ValueTextPattern == SliderListener.ValueTextPattern.Relative)
        {
            if (listener.m_IsTextUseTMP)
            {
                listener.SetTMPText_Relative();
            }
            else
            {
                listener.SetText_Relative(m_numberSprite, m_plusMinusSprite, m_plusSprite, m_minusSprite);
            }
        }
        else if (listener.m_ValueTextPattern == SliderListener.ValueTextPattern.Persent_Select)
        {
            if (listener.m_IsTextUseTMP)
            {
                listener.SetTMPText_Persent_Select();
            }
            else
            {
                listener.SetText_Persent_Select(m_numberSprite, m_percentSprite);
            }
        }
    }
}
