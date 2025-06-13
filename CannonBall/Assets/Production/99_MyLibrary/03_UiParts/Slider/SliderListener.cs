/*******************************************************************************
*
*	�^�C�g���F	�X���C�_�[���X�i�[�X�N���v�g
*	�t�@�C���F	SliderListener.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/05/07
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class SliderListener : UiListenerBase
{
    public enum ValueTextPattern
    {
        Persent = 0,
        Relative,
        Persent_Select,
        Length,
    }

    #region field

    /// <summary> �l�̕\�L���@ </summary>
    [SerializeField, CustomLabel("�l�̕\�L���@")]
    protected ValueTextPattern m_valueTextPattern;

    /// <summary> �X���C�_�[ </summary>
    [SerializeField, CustomLabel("�X���C�_�[")]
    protected Slider m_slider;

    /// <summary> �l�������e�L�X�g��TMP���g���� </summary>
    [SerializeField, CustomLabel("�l�������e�L�X�g��TMP���g����")]
    protected bool m_isTextUseTMP = true;

    /// <summary> �l�������e�L�X�g�̐e�I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("�l�������e�L�X�g�̐e�I�u�W�F�N�g")]
    protected Transform m_valueTextParent;

    /// <summary> �i�K�̍ő吔 </summary>
    [SerializeField, CustomLabel("�i�K�̍ő吔")]
    protected int m_stepMax = 10;

    /// <summary> �p�[�Z���g�̏���l(Persent_Select�p) </summary>
    [SerializeField, CustomLabel("�p�[�Z���g�̏���l(Persent_Select�p)")]
    protected int m_persentMax = 500;

    /// <summary> ���݂̒i�K�� </summary>
    protected ReactiveProperty<int> m_step = new ReactiveProperty<int>();

    #endregion

    #region property

    public ValueTextPattern m_ValueTextPattern { get { return m_valueTextPattern; } }

    public int m_Step
    {
        get { return m_step.Value; }
        set { m_step.Value = value; }
    }

    public bool m_IsTextUseTMP { get { return m_isTextUseTMP; } }

    public ReactiveProperty<int> m_StepReactiveProperty { get { return m_step; } }

    #endregion

    #region function

    protected virtual void Awake()
    {
        InitUiPattern(UiPattern.Slider);
    }

    /// <summary>
    /// �E�Ƀn���h���̒i�K��ύX
    /// </summary>
    public override void LeftSlide()
    {
        if (m_step.Value > 0)
        {
            ChangeStep((m_step.Value - 1) % (m_stepMax + 1));
        }
    }

    /// <summary>
    /// �E�Ƀn���h���̒i�K��ύX
    /// </summary>
    public override void RightSlide()
    {
        if (m_step.Value < m_stepMax)
        {
            ChangeStep((m_step.Value + 1) % (m_stepMax + 1));
        }
    }

    /// <summary>
    /// �X���C�_�[�̒i�K�ύX
    /// </summary>
    protected virtual void ChangeStep(int step)
    {
        m_step.Value = step;
        SetSliderValue();
    }

    /// <summary>
    /// �X���C�_�[�R���|�[�l���g�̒l���X�V
    /// </summary>
    protected void SetSliderValue()
    {
        m_slider.value = (float)m_step.Value / m_stepMax;
    }

    /// <summary>
    /// �e�L�X�g�p�摜�ݒ�(�p�[�Z���g�\��)
    /// </summary>
    /// <param name="numberSprite"> �����摜 </param>
    /// <param name="percentSprite"> �p�[�Z���g�L���摜 </param>
    public void SetText_Persent(Sprite[] numberSprite, Sprite percentSprite)
    {
        int i = 0;

        // �L��
        SetSprite(i, percentSprite);
        i++;

        // ���l
        int value = m_step.Value * m_stepMax;
        do
        {
            int d = value % 10;
            SetSprite(i, numberSprite[d]);
            value /= 10;
            i++;
        }
        while (i < 4 && value != 0);

        // �c��̌��͂��ׂ�null�Őݒ�
        SetNullSprite(i);
    }

    /// <summary>
    /// �e�L�X�g�p�摜�ݒ�(�p�[�Z���g�\��)
    /// </summary>
    public void SetTMPText_Persent()
    {
        // ���l
        int value = m_step.Value * m_stepMax;
        m_valueTextParent.GetComponent<TextMeshProUGUI>().text = value.ToString() + "%";
    }

    /// <summary>
    /// �e�L�X�g�p�摜�ݒ�(���Βl�\��)
    /// </summary>
    /// <param name="numberSprite"> �����摜 </param>
    /// <param name="plusMinusSprite"> �v���}�C�L���摜 </param>
    /// <param name="plusSprite"> �v���X�L���摜 </param>
    /// <param name="minusSprite"> �}�C�i�X�L���摜 </param>
    public void SetText_Relative(Sprite[] numberSprite,
        Sprite plusMinusSprite, Sprite plusSprite, Sprite minusSprite)
    {
        int i = 0;
        int temp = m_stepMax / 2;
        int relative = m_step.Value - temp;

        // ���l
        SetSprite(i, numberSprite[Mathf.Abs(relative)]);
        i++;

        // �L��
        if (relative < 0)
        {
            SetSprite(i, minusSprite);
        }
        else if (relative > 0)
        {
            SetSprite(i, plusSprite);
        }
        else
        {
            SetSprite(i, plusMinusSprite);
        }
        i++;

        // �c��̌��͂��ׂ�null�Őݒ�
        SetNullSprite(i);
    }

    /// <summary>
    /// �e�L�X�g�p�摜�ݒ�(�p�[�Z���g�\��)
    /// </summary>
    public void SetTMPText_Relative()
    {
        // ���l
        int temp = m_stepMax / 2;
        int relative = m_step.Value - temp;
        string symbol;
        if (relative < 0)
        {
            symbol = "-";
        }
        else if (relative > 0)
        {
            symbol = "+";
        }
        else
        {
            symbol = "�}";
        }
        m_valueTextParent.GetComponent<TextMeshProUGUI>().text = relative.ToString() + symbol;
    }

    /// <summary>
    /// �e�L�X�g�p�摜�ݒ�(�p�[�Z���g�\��)
    /// </summary>
    /// <param name="numberSprite"> �����摜 </param>
    /// <param name="percentSprite"> �p�[�Z���g�L���摜 </param>
    public void SetText_Persent_Select(Sprite[] numberSprite, Sprite percentSprite)
    {
        int i = 0;

        // �L��
        SetSprite(i, percentSprite);
        i++;

        // ���l
        int value = (int)((float)m_step.Value / m_stepMax * m_persentMax);
        do
        {
            int d = value % 10;
            SetSprite(i, numberSprite[d]);
            value /= 10;
            i++;
        }
        while (i < 4 && value != 0);

        // �c��̌��͂��ׂ�null�Őݒ�
        SetNullSprite(i);
    }

    /// <summary>
    /// �e�L�X�g�p�摜�ݒ�(�p�[�Z���g�\��)
    /// </summary>
    public void SetTMPText_Persent_Select()
    {
        // ���l
        int value = (int)((float)m_step.Value / m_stepMax * m_persentMax);
        m_valueTextParent.GetComponent<TextMeshProUGUI>().text = value.ToString() + "%";
    }

    /// <summary>
    /// �摜1����ݒ�
    /// </summary>
    /// <param name="childIndex"> �����ڂ�(���Ԗڂ̎q�I�u�W�F�N�g��) </param>
    /// <param name="sprite"> �ݒ肵�����X�v���C�g </param>
    private void SetSprite(int childIndex, Sprite sprite)
    {
        Image image = m_valueTextParent.GetChild(childIndex).GetComponent<Image>();
        image.sprite = sprite;
        
        // �X�v���C�g��null��������\�����Ȃ��悤�ɂ���
        if (sprite == null)
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
        }
    }

    /// <summary>
    /// �w�肵���C���f�b�N�X�ȍ~�̃X�v���C�g��null�Őݒ肷��
    /// </summary>
    /// /// <param name="i"> �����ڂ�(���̌��ȍ~�����ׂĔ�\���ɂ���) </param>
    private void SetNullSprite(int i)
    {
        for (; i < 4; i++)
        {
            SetSprite(i, null);
        }
    }

    #endregion
}
