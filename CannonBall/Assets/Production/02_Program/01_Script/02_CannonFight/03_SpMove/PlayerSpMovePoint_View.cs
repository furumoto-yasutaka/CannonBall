using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpMovePoint_View : MonoBehaviour
{
    /// <summary> �|�C���g����R���|�[�l���g </summary>
    [SerializeField, CustomLabel("�X���C�_�[")]
    private Slider m_slider;

    /// <summary> �K�E�Z�Ñ��A�C�R���̃A�j���[�^�[ </summary>
    [SerializeField, CustomLabel("�K�E�Z�Ñ��A�C�R���̃A�j���[�^�[")]
    private Animator m_pushiconAnimator;

    /// <summary> ���܂����Ƃ��̕K�E�Z�Q�[�W </summary>
    [SerializeField, CustomLabel("���܂����Ƃ��̕K�E�Z�Q�[�W")]
    private GameObject m_chargedGauge;


    /// <summary> �X���C�_�[�̕`�抄���X�V </summary>
    public void SetSliderValue(float value)
    {
        m_slider.value = value;
        m_chargedGauge.SetActive(value == 1.0f);
        m_pushiconAnimator.SetBool("IsShow", value == 1.0f);
    }
}
