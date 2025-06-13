/*******************************************************************************
*
*	�^�C�g���F	�^�C�}�[�V���O���g���X�N���v�g
*	�t�@�C���F	Timer.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : SingletonMonoBehaviour<Timer>
{
    /// <summary> ���l�̉摜�f�� </summary>
    [SerializeField, CustomLabel("���l�̉摜�f��")]
    protected Sprite[] m_numberSprites;

    /// <summary> ��������(�b) </summary>
    [SerializeField, CustomLabel("��������(�b)")]
    protected int m_timelimit = 180;

    /// <summary> ���\���̐e�I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("���\���̐e�I�u�W�F�N�g")]
    protected Transform[] m_minutesParent;

    /// <summary> ���\���̐e�I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("�b�\���̐e�I�u�W�F�N�g")]
    protected Transform[] m_secondsParent;

    /// <summary> �^�C�}�[�𓮂����Ȃ� </summary>
    [SerializeField, CustomLabel("�^�C�}�[�𓮂����Ȃ�")]
    public bool m_isStopTimer = true;

    /// <summary> �^�C�}�[�̎c�莞�� </summary>
    protected float m_timeCounter = 9999.0f;

    protected bool m_isTimerEnd = false;


    public float m_TimeCounter { get { return m_timeCounter; } }


    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();

        m_timeCounter = m_timelimit;

        // �^�C�}�[�̒l��������
        m_timeCounter += Time.deltaTime;
        TimeUpdate();
    }

    protected virtual void Update()
    {
        if (m_isStopTimer)
        {
            if (ReadyGoAnimationCallback.CheckInstance())
            {
                if (ReadyGoAnimationCallback.Instance.m_IsFinish)
                {
                    m_isStopTimer = false;
                }
            }
        }
        else
        {
            TimeUpdate();
        }
    }

    private void TimeUpdate()
    {
        if (m_timeCounter > 0.0f)
        {
            // �o�ߕb�������Z
            m_timeCounter -= Time.deltaTime;

            if (m_timeCounter <= 0.0f)
            {
                m_isTimerEnd = true;
                m_timeCounter = 0.0f;
                TimerEndCallback();
            }

            int count = (int)m_timeCounter;
            // �����_�ȉ��̒[�������݂���ꍇ1�b���Z���ĕ\������
            if (m_timeCounter - count > 0.0f)
            {
                count++;
            }
            int minute = count / 60;
            int second = count % 60;

            // ���̉摜�ݒ�
            SetNumberSprite(minute, m_minutesParent);

            // �b�̉摜�ݒ�
            SetNumberSprite(second, m_secondsParent);
        }
    }

    /// <summary> ���_�X�v���C�g�ݒ� </summary>
    /// <param name="value"> �ݒ肷��l </param>
    /// <param name="parent"> ���l�\���̐e�I�u�W�F�N�g </param>
    protected virtual void SetNumberSprite(int value, Transform[] parent)
    {
        for (int i = 0; i < parent.Length; i++)
        {
            int temp = value;
            int j = 0;
            do
            {
                int d = temp % 10;
                parent[i].GetChild(j).GetComponent<Image>().sprite = m_numberSprites[d];
                temp /= 10;
                j++;
            }
            while (j < parent[i].childCount && temp != 0);

            for (; j < parent[i].childCount; j++)
            {
                parent[i].GetChild(j).GetComponent<Image>().sprite = m_numberSprites[0];
            }
        }
    }

    protected virtual void TimerEndCallback()
    {

    }
}
