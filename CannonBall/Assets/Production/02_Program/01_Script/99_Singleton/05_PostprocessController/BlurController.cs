/*******************************************************************************
*
*	�^�C�g���F	�u���[����p�V���O���g���N���X
*	�t�@�C���F	BlurController.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/05/09
*	�X�V�����F�@
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class BlurController : SingletonMonoBehaviour<BlurController>
{
//    /// <summary> �u���[�̏�� </summary>
//    public enum State
//    {
//        BlurOn = 0,
//        ChangeBlurOn,
//        BlurOff,
//        ChangeBlurOff,
//        Length,
//    }

//    #region field

//    /// <summary> ��ʊE�[�x�̃C���X�^���X </summary>
//    private DepthOfField m_depthOfField;

//    /// <summary> �s���g���� </summary>
//    private float m_toFoculLength = 0.0f;
//    /// <summary> �s���g���� </summary>
//    private float m_focalLengthPerFrame = 0.0f;
//    /// <summary> �C���X�y�N�^�[������͉\�ɂ��邽�߂̈ꎞ�ϐ� </summary>
//    private int m_toFrame = 0;
//    /// <summary> �C���X�y�N�^�[������͉\�ɂ��邽�߂̈ꎞ�ϐ� </summary>
//    private int m_frameCount = 0;

//    /// <summary> �C���X�y�N�^�[������͉\�ɂ��邽�߂̈ꎞ�ϐ� </summary>
//    private State m_state = State.BlurOff;

//    #endregion

//    #region function

//    protected override void Awake()
//    {
//        dontDestroyOnLoad = false;

//        base.Awake();

//        if (!GetComponent<Volume>().profile.TryGet(out m_depthOfField))
//        {
//#if UNITY_EDITOR
//            Debug.LogError("DepthOfField��Volume�ɒǉ�����Ă��܂���");
//#endif
//        }
//    }

//    private void Update()
//    {
//        if (m_state == State.ChangeBlurOn)
//        {
//            UpdateBlurOn();
//        }
//        else if (m_state == State.ChangeBlurOff)
//        {
//            UpdateBlurOff();
//        }
//    }

//    /// <summary>
//    /// �u���[�L��������
//    /// </summary>
//    /// <param name="focesDistance"> �œ_���� </param>
//    /// <param name="focalLength"> �œ_���� </param>
//    /// <param name="frame"> �t���[���� </param>
//    public void BlurOn(float focesDistance, float focalLength, int frame)
//    {
//        // ���łɎ��s���܂��͑J�ڍς݂̏ꍇ��������
//        if (m_state == State.BlurOn)
//        {
//            return;
//        }

//        // �p�����[�^�ݒ�
//        m_state = State.ChangeBlurOn;
//        m_depthOfField.focusDistance.value = focesDistance;
//        m_toFoculLength = focalLength;
//        m_toFrame = frame;
//        m_focalLengthPerFrame = (m_toFoculLength - m_depthOfField.focusDistance.value) / m_toFrame;
//    }

//    /// <summary>
//    /// �u���[�L��������(�����ύX)
//    /// </summary>
//    /// <param name="focesDistance"> �œ_���� </param>
//    /// <param name="focalLength"> �œ_���� </param>
//    public void BlurOnFast(float focesDistance, float focalLength)
//    {
//        // �p�����[�^�ݒ�
//        m_state = State.BlurOn;
//        m_depthOfField.focusDistance.value = focesDistance;
//        m_depthOfField.focalLength.value = focalLength;
//    }

//    /// <summary>
//    /// �u���[����������
//    /// </summary>
//    /// <param name="frame"> �t���[���� </param>
//    public void BlurOff(int frame)
//    {
//        // ���łɎ��s���܂��͑J�ڍς݂̏ꍇ��������
//        if (m_state == State.BlurOff)
//        {
//            return;
//        }

//        // �p�����[�^�ݒ�
//        m_state = State.ChangeBlurOff;
//        m_toFoculLength = 0;
//        m_toFrame = frame;
//        m_focalLengthPerFrame = m_depthOfField.focalLength.value / m_toFrame;
//    }

//    /// <summary>
//    /// �u���[����������(�����ύX)
//    /// </summary>
//    public void BlurOffFast()
//    {
//        // �p�����[�^�ݒ�
//        m_state = State.BlurOff;
//        m_depthOfField.focusDistance.value = 0.0f;
//        m_depthOfField.focalLength.value = 0.0f;
//    }

//    /// <summary>
//    /// �u���[�X�V����(�L����)
//    /// </summary>
//    private void UpdateBlurOn()
//    {
//        m_frameCount++;
//        m_depthOfField.focalLength.value += m_focalLengthPerFrame;

//        if (m_frameCount == m_toFrame)
//        {
//            m_state = State.BlurOn;
//            m_frameCount = 0;
//        }
//    }

//    /// <summary>
//    /// �u���[�X�V����(������)
//    /// </summary>
//    private void UpdateBlurOff()
//    {
//        m_frameCount++;
//        m_depthOfField.focalLength.value -= m_focalLengthPerFrame;

//        if (m_frameCount == m_toFrame)
//        {
//            m_state = State.BlurOff;
//            m_frameCount = 0;
//        }
//    }

//    #endregion
}
