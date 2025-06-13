using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStick : MonoBehaviour
{
    [SerializeField, CustomLabel("�v���C���[�̃X�e�B�b�N�ł̈ړ���")]
    float m_maxMove = 100.0f;

    [SerializeField]
    private PlayerInputController m_playerInputController;


    //GameObject[] m_players;

    bool m_isFinish = false;

    [SerializeField]
    float[] m_move = new float[4];


    private void Update()
    {
        if (m_isFinish)
        {
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            m_move[i] += Mathf.Abs(m_playerInputController.GetMove(i).x);

            Debug.Log("GetStickValue" + m_playerInputController.GetMove(i).x);

        }

        for (int i = 0; i < 4; i++)
        {
            Debug.Log("m_move[" + i + "]  " + m_move[i]);
        }

        if (Check())
        {
            // ���̃`���[�g���A�����I��������A���̃`���[�g���A���̏���������
            TutorialManager.Instance.ChangeNextLevel();

            m_isFinish = true;
        }
    }

    private bool Check()
    {
        for (int i = 0; i < 4; i++)
        {
            // ��ł����ʖ�����������A�`���[�g���A�������Ƃ݂Ȃ��Ȃ�
            if (m_move[i] < m_maxMove)
            {
                return false;
            }
        }
        return true;
    }



}
