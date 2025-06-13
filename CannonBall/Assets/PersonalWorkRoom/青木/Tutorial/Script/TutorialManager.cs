using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TutorialManager : SingletonMonoBehaviour<TutorialManager>
{
    // �`���[�g���A���̒i�K�̐���
    // 0�F�E�̕ǂɓ�����
    // 1�F���̓V��ɓ�����
    // 2�F��̓V��ɓ�����
    int m_tutorialLevel = 0;

    [SerializeField]
    TextFeed m_textFeed;

    [SerializeField, CustomLabel("�e�L�X�g�ƌ���ׂāA���s�ڂɃC�x���g���K�v�Ȃ̂�")]
    int[] m_eventTextLine;

    bool m_isEventing = false;

    GameObject[] m_tutorialWalls;

    private void Start()
    {
        m_tutorialWalls = new GameObject[transform.childCount];
        for (int i = 0; i < m_tutorialWalls.Length; i++)
        {
            m_tutorialWalls[i] = transform.GetChild(i).gameObject;

            // ��ԍŏ��ȊO��OFF�ɂ���
            //if (i != 0)
            {
                m_tutorialWalls[i].gameObject.SetActive(false);
            }
        }


    }

    private void Update()
    {
        if (m_isEventing)
        {
            return;
        }


        if (Event())
        {
            Debug.Log("��");

            //m_textFeed.NextTextLine();
            //StartCoroutine(NextText());
            if (m_textFeed.GetIsEndFeed)
            {
                m_textFeed.m_IsStop = true;
            }


        }

        if (m_textFeed.IsEventChar)
        {
            m_tutorialWalls[m_tutorialLevel].gameObject.SetActive(true);
        }
        ////else if (StopEvent())
        ////{

        ////    m_isEventing = true;

        ////}
        //else if (NoEvent())
        //{
        //    Debug.Log("����");
        //    m_textFeed.NextTextLine();
        //    //StartCoroutine(NextText());
        //}
    }


    IEnumerator NextText()
    {
        yield return new WaitForSeconds(0.5f);

        m_textFeed.NextTextLine();
    }


    private bool Event()
    {
        for (int i = 0; i < m_eventTextLine.Length; i++)
        {
            // �i�s���̃e�L�X�g���I����� �� ���̃e�L�X�g�ԍ�����������̃C�x���g������̂Ȃ�
            if (m_textFeed.IsEventChar && m_textFeed.GetTextLine == m_eventTextLine[i])
            {
                return true;
            }
        }
        return false;
    }


    private bool StopEvent()
    {
        for (int i = 0; i < m_eventTextLine.Length; i++)
        {
            // �i�s���̃e�L�X�g���I����� �� ���̃e�L�X�g�ԍ�����������̃C�x���g������̂Ȃ�
            if (m_textFeed.GetIsEndFeed && m_textFeed.GetTextLine == m_eventTextLine[i])
            {
                return true;
            }
        }
        return false;
    }

    private bool NoEvent()
    {
        // �Ȃ񂩂߂��Ꮘ���͒m�����Ⴄ



        // �e�L�X�g���I�����Ă��Ȃ�������A�����������̃e�L�X�g�ɑ���Ȃ�
        if (!m_textFeed.GetIsEndFeed)
        {
            return false;
        }

        for (int i = 0; i < m_eventTextLine.Length; i++)
        {
            // ���̃e�L�X�g�ԍ�����������̃C�x���g���Ȃ�
            if (m_textFeed.GetTextLine + 1 == m_eventTextLine[i])
            {
                return false;
            }
        }

        return true;
    }


    /// <summary>
    /// �S������i�K�`���[�g���A�����I�������
    /// </summary>
    public void ChangeNextLevel()
    {
        m_tutorialWalls[m_tutorialLevel].gameObject.SetActive(false);

        m_tutorialLevel++;

        m_isEventing = false;

        m_textFeed.NextTextLine();

        m_textFeed.EndEvent();
    }



}
