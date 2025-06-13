/*******************************************************************************
*
*	�^�C�g���F	�T�b�J�[�S�[���̃R���W�����C�x���g�X�N���v�g
*	�t�@�C���F	SuccerGoalGateOnCollition.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/05
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccerGoalGateOnCollition : MonoBehaviour
{
    /// <summary> �ԃ`�[�����̃S�[���Q�[�g���ǂ��� </summary>
    [SerializeField, CustomLabel("�ԃ`�[�����̃S�[���Q�[�g���ǂ���")]
    private bool m_isRedTeam = true;


    private void OnTriggerExit2D(Collider2D collision)
    {
        // �S�[���Q�[�g�������Ƃ������Ă��邩�m�F
        if (!IsGoal(collision.transform.position)) { return; }

        if (collision.CompareTag("PlayerBody"))
        {// �v���C���[
            OnTriggerExit_Player(collision);
        }
        else if (collision.CompareTag("Ball"))
        {// �{�[��
            OnTriggerExit_Ball(collision);
        }
    }

    /// <summary> �S�[�����Ă��邩�ǂ������f </summary>
    /// <param name="position"> �{�[���̍��W </param>
    private bool IsGoal(Vector3 position)
    {
        if (m_isRedTeam)
        {
            // �S�[���Q�[�g���O����������S�[��
            if (transform.position.x > position.x)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            // �S�[���Q�[�g���O����������S�[��
            if (transform.position.x < position.x)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary> �v���C���[�����ꂽ�ۂ̃R���W�����C�x���g </summary>
    /// <param name="collision"> �{�[���̍��W </param>
    private void OnTriggerExit_Player(Collider2D collision)
    {
        // ���v���C���[���Ƃɂǂ̃`�[���ɏ������邩�w��ł���悤�ɂ���ꍇ������ύX����
        int playerId = collision.transform.root.GetComponent<PlayerId>().m_Id;
        if (playerId < 2)
        {
            SuccerTeamPointManager.Instance.PlayerGoalIn_Blue();
        }
        else
        {
            SuccerTeamPointManager.Instance.PlayerGoalIn_Red();
        }
    }

    /// <summary> �{�[�������ꂽ�ۂ̃R���W�����C�x���g </summary>
    /// <param name="collision"> �{�[���̍��W </param>
    private void OnTriggerExit_Ball(Collider2D collision)
    {
        if (!collision.transform.root.GetComponent<BallInfo>().m_IsRareBall)
        {// �ʏ�{�[��
            if (m_isRedTeam)
            {
                SuccerTeamPointManager.Instance.BallGoalIn_Blue();
            }
            else
            {
                SuccerTeamPointManager.Instance.BallGoalIn_Red();
            }
        }
        else
        {// ���A�{�[��
            if (m_isRedTeam)
            {
                SuccerTeamPointManager.Instance.RareBallGoalIn_Blue();
            }
            else
            {
                SuccerTeamPointManager.Instance.RareBallGoalIn_Red();
            }
        }
    }
}
