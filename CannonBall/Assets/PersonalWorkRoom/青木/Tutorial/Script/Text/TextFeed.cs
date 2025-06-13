/*******************************************************************************
*
*	�^�C�g���F	��������
*	�t�@�C���F	TextFeed.cs
*	�쐬�ҁF	�� �喲
*	������F    2023/10/09
*
*******************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;



public class TextFeed : MonoBehaviour
{

    [SerializeField]
    private TextAsset m_textAsset;

    [SerializeField, CustomLabel("��������̃X�s�[�h(�P�ʁFf)")]
    private int m_textFeedFrameSpeed = 1;

    private int m_textFeedFrameCount = 0;


    [SerializeField, CustomLabel("�s�ԃX�s�[�h(�P�ʁFf)")]
    private int m_textWait = 5;

    [SerializeField, CustomLabel("3�_���[�_�[���󎚂��邩")]
    private bool m_3MarkOutput = true;

    private int m_textWaitCount = 0;

    // TextAsset������s���ɋ�؂��Ċi�[
    private string[] m_text;

    private int m_textLine = 0;

    public int GetTextLine { get { return m_textLine; } }

    private TextMeshProUGUI m_textMeshPro;

    private bool m_isFeed = true;

    private bool m_isEventChar = false;

    public bool IsEventChar { get { return m_isEventChar; } set { m_isEventChar = value; } }

    public bool GetIsFeed { get { return m_isFeed; } }

    private bool m_isEndFeed = false;
    public bool GetIsEndFeed { get { return m_isEndFeed; } }

    private int m_charCount = 0;

    // ��Ǔ_�̑҂��ԁi�P�ʁF�b�j
    private readonly float m_PunctuationMark = 0.1f;

    // �O�_���[�_�[�̑҂��ԁi�P�ʁF�b�j
    private readonly float m_3Mark = 0.45f;

    // ��Ǔ_�A��_�Ȃǂ̑҂���
    private float m_waitTime;


    // ��������̃C�x���g���N���āA�e�L�X�g���~�܂鏈��
    private bool m_isStop = false;

    public bool m_IsStop { get { return m_isStop; } set { m_isStop = value; } }

    // �e�L�X�g�𗬂��Ă����Œ��Ɋ֐������s����p�̎d�g��
    [SerializeField]
    private UnityEvent[] m_textActions;
    // �e�L�X�g�𗬂��Ă����Œ��Ɏ��s����֐��̓Y����
    int m_textActionIndex = 0;



    private void Start()
    {

        string text = m_textAsset.text;
        m_text = text.Split(char.Parse("\n"));


        m_textMeshPro = GetComponent<TextMeshProUGUI>();

        InitializeText();

    }


    private void Update()
    {
        if (m_isStop)
        {
            return;
        }

        if (m_isFeed && !m_isEndFeed)
        {


            if (m_waitTime >= 0.0f)
            {
                m_waitTime -= Time.deltaTime;
            }
            else if (ContainsMark('�A'))
            {
                WaitFeed(m_PunctuationMark, true);
            }
            else if (ContainsMark('�c'))
            {
                WaitFeed(m_3Mark, m_3MarkOutput);
            }
            else
            {

                m_textFeedFrameCount++;
                if (m_textFeedFrameCount >= m_textFeedFrameSpeed)
                {
                    if (m_text[m_textLine][m_charCount] == '#')
                    {
                        m_isStop = true;
                        m_isEventChar = true;
                    }
                    else if (ContainsMark('@'))
                    {
                        // �o�^����Ă���֐������s
                        m_textActions[m_textActionIndex++].Invoke();
                    }
                    else
                    {
                        m_textMeshPro.text += m_text[m_textLine][m_charCount];

                        AudioManager.Instance.PlaySe("�`���[�g���A��_��b", false);

                    }


                    m_charCount++;

                    m_textFeedFrameCount = 0;


                    // m_manualText[m_textLine]�̍Ō�̕����܂ŕ\��������
                    if (m_charCount >= m_text[m_textLine].Length)
                    {
                        //// �C�x���g���l�܂��Ă�����A�����Ŏ~�܂�
                        //if (m_isEventChar)
                        //{
                        //    m_isStop = true;
                        //}
                        m_isFeed = false;

                        m_isEndFeed = true;
                    }
                }
            }
        }
        else if (m_isEndFeed)
        {
            // �s�Ԃ�҂�
            if (m_textWaitCount > m_textWait)
            {
                // ���̍s��ݒ肷��
                NextTextLine();

                m_textWaitCount = 0;
            }
            else
            {
                m_textWaitCount++;
            }
        }
    }

    // ���ɗ��������������̕������ǂ����𔻕�
    private bool ContainsMark(char _char)
    {
        return m_text[m_textLine][m_charCount] == _char && m_waitTime < 0.0f;
    }

    private void WaitFeed(float _waitTime, bool _isOutput)
    {
        m_waitTime = m_3Mark;

        if (_isOutput)
        {
            m_textMeshPro.text += m_text[m_textLine][m_charCount];
        }

        m_charCount++;
    }


    public bool NextTextLine()
    {
        if (m_text.Length - 1 > m_textLine)
        {
            m_textLine++;

            // �e�L�X�g�̏�����
            InitializeText();

            return true;
        }

        return false;
    }


    public void EndEvent()
    {
        m_isEventChar = false;
        m_isStop = false;
    }

    private void InitializeText()
    {
        // �ԍ����Z�b�g
        m_charCount = 0;

        // ���݂̃e�L�X�g���N���A
        m_textMeshPro.text = "";

        // �e�L�X�g�̏c����ς���
        m_textMeshPro.rectTransform.sizeDelta = new Vector2(m_textMeshPro.fontSize * m_text[m_textLine].Length, m_textMeshPro.fontSize);

        // �e�L�X�g����̃X�C�b�`��ON�ɂ���
        m_isFeed = true;

        // �����~�߂�t���O��OFF
        m_isStop = false;

        // �s���t���O��OFF
        m_isEndFeed = false;
    }
}
