using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ResultRank : MonoBehaviour
{
    // �����̂ŏȗ��ϐ���p��
    readonly int DATA_LENGTH = TemporaryData.m_Rank.Length;

    private void Awake()
    {

        // ������
        for (int i = 0; i < DATA_LENGTH; i++)
        {
            // �����l�O
            TemporaryData.m_Rank[i].m_Ranking = 0;
        }


        // �\�[�g���ꂽ�z��
        int[] parameters = new int[4];
        for (int i = 0; i < DATA_LENGTH; i++)
        {
            // �p�����[�^�[��10�{�ɂ��āA�����_���ʂ܂ŏ��ʂ̍l���ɓ����B���ʈȉ��͐؂�̂�
            parameters[i] = Mathf.FloorToInt(TemporaryData.m_Rank[i].m_Parameter * 10.0f);

        }


        // ����
        int ranking = 1;

        // �z��������\�[�g
        Array.Sort(parameters);

        // �f���W�������̏ꍇ�́A���ʂ����]
        if (TemporaryData.m_PreGameMode != SceneNameEnum.DangerRun)
        {
            // �z��̏����𔽓]������
            Array.Reverse(parameters);
        }

        int preParam = parameters[0];


        // ���ʕt�����s����
        for (int i = 0; i < DATA_LENGTH; i++)
        {
            for (int j = 0; j < DATA_LENGTH; j++)
            {
                if (parameters[i] == Mathf.FloorToInt(TemporaryData.m_Rank[j].m_Parameter * 10.0f) && TemporaryData.m_Rank[j].m_Ranking == 0)
                {
                    TemporaryData.m_Rank[j].m_Ranking = (preParam != parameters[i]) ? ++ranking : ranking;
                    break;
                }
            }

            preParam = parameters[i];
        }
    }


}
