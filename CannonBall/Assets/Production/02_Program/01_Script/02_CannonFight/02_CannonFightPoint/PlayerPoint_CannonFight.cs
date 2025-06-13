/*******************************************************************************
*
*	�^�C�g���F	�v���C���[�̃|�C���g����X�N���v�g
*	�t�@�C���F	PlayerPoint_CannonFight.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/08
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerPoint_CannonFight : MonoBehaviour, ITemporaryDataGetter
{
    /// <summary> ���� </summary>
    private int m_rank = 1;

    /// <summary> ���_ </summary>
    private ReactiveProperty<int> m_point = new ReactiveProperty<int>(0);

    /// <summary> �O��̓��_�����ő��������ǂ��� </summary>
    private bool m_isAdd = true;

    /// <summary> �R��ɂ��}�[�N���󂯂Ă��邩 </summary>
    private bool m_isKickMark = true;

    /// <summary> �}�[�N����Ă���v���C���[ </summary>
    private PlayerPoint_CannonFight m_markPlayer = null;

    /// <summary> �}�[�N�̎c�莞�� </summary>
    private float m_markTimeCount = 0.0f;

    /// <summary> �L�b�N���ꂽ���̃}�[�N���� </summary>
    private static float m_fromKickMarkTime = 6.0f;

    /// <summary> ���˂��ꂽ���̃}�[�N���� </summary>
    private static float m_fromContactMarkTime = 3.0f;

    /// <summary> ���˂����Ƃ���x�N�g���̋��� </summary>
    private static float m_contactMarkThreshold = 1.0f;


    public IReadOnlyReactiveProperty<int> m_Point => m_point;

    public PlayerPoint_CannonFight m_MarkPlayer { get { return m_markPlayer; } }

    public bool m_IsAdd { get { return m_isAdd; } }

    public bool m_IsKickMark { get { return m_isKickMark; } }

    public int m_Rank { get { return m_rank; } }


    private void Update()
    {
        // �}�[�N���Ԃ��X�V
        if (m_markPlayer != null)
        {
            if (m_markTimeCount <= 0.0f)
            {
                m_markTimeCount = 0.0f;
                m_markPlayer = null;
                m_isKickMark = false;
            }
            else
            {
                m_markTimeCount -= Time.deltaTime;
            }
        }
    }

    /// <summary> �L�����ꂽ�ۂ̃|�C���g���Z </summary>
    public void KilledDividePoint()
    {
        SubPoint();

        if (m_markPlayer != null)
        {
            m_markPlayer.AddPoint();
        }
    }

    /// <summary> �|�C���g���Z </summary>
    public void AddPoint()
    {
        m_point.Value++;
        m_isAdd = true;
    }

    /// <summary> �|�C���g���� </summary>
    public void SubPoint()
    {
        m_point.Value--;
        m_isAdd = false;
    }

    /// <summary> �R��ɂ��}�[�N���s�� </summary>
    /// <param name="target"> �v���C���[�̃R���W���� </param>
    public void RequestKickMark(PlayerPoint_CannonFight target)
    {
        m_markPlayer = target;
        m_markTimeCount = m_fromKickMarkTime;
        m_isKickMark = true;
    }

    /// <summary> ���˂��ɂ��}�[�N���s�� </summary>
    /// <param name="target"> �Ώۂ̃v���C���[ </param>
    /// <param name="vel"> ���˂��̃x�N�g�� </param>
    public void RequestContactMark(PlayerPoint_CannonFight target, Vector2 vel)
    {
        // ���˂��̋��������ȏォ��
        // ���ݎc���Ă���}�[�N���Ԃ����˂��ŕt�^�����}�[�N���Ԃ�菬������
        if (vel.sqrMagnitude >= m_contactMarkThreshold * m_contactMarkThreshold &&
            m_markTimeCount <= m_fromContactMarkTime)
        {
            if (target.m_IsKickMark)
            {
                m_markPlayer = target.m_MarkPlayer;
                m_markTimeCount = m_fromContactMarkTime;
            }
            else
            {
                m_markPlayer = target;
                m_markTimeCount = m_fromContactMarkTime;
            }

            m_isKickMark = false;
        }
    }

    /// <summary> ���ʐݒ� </summary>
    /// <param name="rank"> ���� </param>
    public void SetRank(int rank)
    {
        m_rank = rank;
    }

    public float GetRankingParameter()
    {
        return m_point.Value;
    }
}
