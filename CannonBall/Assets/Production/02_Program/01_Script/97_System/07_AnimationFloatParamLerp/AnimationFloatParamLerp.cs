using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFloatParamLerp
{
    /// <summary> �A�j���[�^�[�R���|�[�l���g </summary>
    private Animator m_animator;
    /// <summary> �p�����[�^�� </summary>
    private string m_paramName = "";
    /// <summary> ���݂̒l </summary>
    private float m_value = 0.0f;
    /// <summary> �ڕW�l </summary>
    private float m_targetValue = 0.0f;
    /// <summary> ��Ԋ��� </summary>
    private float m_lerpRate = 0.1f;


    public AnimationFloatParamLerp(Animator animator, string paramName, float value, float targetValue, float lerpRate = 0.1f)
    {
        m_animator = animator;
        m_paramName = paramName;
        m_value = value;
        m_targetValue = targetValue;
        m_lerpRate = lerpRate;
    }

    public void Update()
    {
        m_value = Mathf.Lerp(m_value, m_targetValue, m_lerpRate);
        m_animator.SetFloat(m_paramName, m_value);
    }

    public void SetValue(float value)
    {
        m_value = value;
        m_animator.SetFloat(m_paramName, m_value);
    }

    public void SetTarget(float targetValue)
    {
        m_targetValue = targetValue;
    }
}
