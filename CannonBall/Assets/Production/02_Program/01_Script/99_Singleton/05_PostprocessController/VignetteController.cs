/*******************************************************************************
*
*	�^�C�g���F	�r�l�b�g����p�V���O���g���N���X
*	�t�@�C���F	VignetteController.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/05/31
*	�X�V�����F�@
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class VignetteController : SingletonMonoBehaviour<VignetteController>
{
    /// <summary> �r�l�b�g�̏�� </summary>
    public enum State
    {
        VignetteOn = 0,
        ChangeVignetteOn,
        VignetteOff,
        ChangeVignetteOff,
        Length,
    }

    #region field

    /// <summary> �r�l�b�g�̃C���X�^���X </summary>
    private Vignette m_vignette;

    /// <summary> �r�l�b�g�̋��� </summary>
    private float m_toIntensity = 0.0f;
    /// <summary> �s���g���� </summary>
    private float m_intensityPerFrame = 0.0f;
    /// <summary> �C���X�y�N�^�[������͉\�ɂ��邽�߂̈ꎞ�ϐ� </summary>
    private int m_toFrame = 0;
    /// <summary> �C���X�y�N�^�[������͉\�ɂ��邽�߂̈ꎞ�ϐ� </summary>
    private int m_frameCount = 0;

    /// <summary> �C���X�y�N�^�[������͉\�ɂ��邽�߂̈ꎞ�ϐ� </summary>
    private State m_state = State.VignetteOff;

    #endregion

    #region function

    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();

        if (!GetComponent<Volume>().profile.TryGet(out m_vignette))
        {
#if UNITY_EDITOR
            Debug.LogError("Vignette��Volume�ɒǉ�����Ă��܂���");
#endif
        }
    }

    private void Update()
    {
        if (m_state == State.ChangeVignetteOn)
        {
            UpdateVignetteOn();
        }
        else if (m_state == State.ChangeVignetteOff)
        {
            UpdateVignetteOff();
        }
    }

    /// <summary>
    /// �r�l�b�g�L��������
    /// </summary>
    /// <param name="intensity"> ���� </param>
    /// <param name="frame"> �t���[���� </param>
    public void VignetteOn(float intensity, int frame)
    {
        // ���łɎ��s���܂��͑J�ڍς݂̏ꍇ��������
        if (m_state == State.VignetteOn)
        {
            return;
        }

        // �p�����[�^�ݒ�
        m_state = State.ChangeVignetteOn;
        m_vignette.intensity.value = 0.0f;
        m_toIntensity = intensity;
        m_toFrame = frame;
        m_intensityPerFrame = (m_toIntensity - m_vignette.intensity.value) / m_toFrame;
    }

    /// <summary>
    /// �r�l�b�g�L��������(�����ύX)
    /// </summary>
    /// <param name="intensity"> ���� </param>
    public void VignetteOnFast(float intensity)
    {
        // �p�����[�^�ݒ�
        m_state = State.VignetteOn;
        m_vignette.intensity.value = intensity;
    }

    /// <summary>
    /// �r�l�b�g����������
    /// </summary>
    /// <param name="frame"> �t���[���� </param>
    public void VignetteOff(int frame)
    {
        // ���łɎ��s���܂��͑J�ڍς݂̏ꍇ��������
        if (m_state == State.VignetteOff)
        {
            return;
        }

        // �p�����[�^�ݒ�
        m_state = State.ChangeVignetteOff;
        m_toFrame = frame;
        m_intensityPerFrame = m_vignette.intensity.value / m_toFrame;
    }

    /// <summary>
    /// �r�l�b�g����������(�����ύX)
    /// </summary>
    public void VignetteOffFast()
    {
        // �p�����[�^�ݒ�
        m_state = State.VignetteOff;
        m_vignette.intensity.value = 0.0f;
    }

    /// <summary>
    /// �r�l�b�g�X�V����(�L����)
    /// </summary>
    private void UpdateVignetteOn()
    {
        m_frameCount++;
        m_vignette.intensity.value += m_intensityPerFrame;

        if (m_frameCount == m_toFrame)
        {
            m_state = State.VignetteOn;
            m_frameCount = 0;
        }
    }

    /// <summary>
    /// �r�l�b�g�X�V����(������)
    /// </summary>
    private void UpdateVignetteOff()
    {
        m_frameCount++;
        m_vignette.intensity.value -= m_intensityPerFrame;

        if (m_frameCount == m_toFrame)
        {
            m_state = State.VignetteOff;
            m_frameCount = 0;
        }
    }

    #endregion
}
