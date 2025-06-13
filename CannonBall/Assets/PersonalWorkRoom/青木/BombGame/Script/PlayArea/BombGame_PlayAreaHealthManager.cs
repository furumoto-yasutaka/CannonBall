/*******************************************************************************
*
*	�^�C�g���F	�e�v���C�G���A�̏����Ǘ����A�X�v���C�g�̕ύX�AHP�̊Ď������s��
*	�t�@�C���F	BombGame_PlayAreaHealthManager .cs
*	�쐬�ҁF	�� �喲
*	������F    2023/10/06
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGame_PlayAreaHealthManager : SingletonMonoBehaviour<BombGame_PlayAreaHealthManager>
{

    BombGame_PlayAreaInfo[] m_stageHealth = new BombGame_PlayAreaInfo[BombGame_PlayAreaData.GetMaxAreaNumber()];

    int m_ranking = BombGame_PlayAreaData.GetMaxAreaNumber();

    public int GetSubNowRanking()
    {
        return --m_ranking;
    }


    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();
    }


    private void Start()
    {
        for (int i = 0; i < m_stageHealth.Length; i++)
        {
            m_stageHealth[i] = BombGame_PlayAreaData.Instance.GetStageObject()[i].GetComponent<BombGame_PlayAreaInfo>();
        }
    }

    public void SubHealth(int areaNum, float _damage)
    {
        m_stageHealth[areaNum].SubStageHealth(_damage);
    }

    public void AddHealth(int areaNum, float _health)
    {
        m_stageHealth[areaNum].AddStageHealth(_health);
    }

}
