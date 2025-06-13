/*******************************************************************************
*
*	�^�C�g���F	�v���C���[���������Ă��邩���f����X�N���v�g
*	�t�@�C���F	AliveZone.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/09/18
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveZone : MonoBehaviour
{
    private bool m_isRun = true;


    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!m_isRun) { return; }

        if (collision.CompareTag("PlayerBody"))
        {
            OnTriggerExit_Player(collision);
        }
    }

    protected virtual void OnTriggerExit_Player(Collider2D collision)
    {
        // ��O�ɏo���̂Ń��X�|�[�����X�g�ɒǉ�
        Transform trans = collision.transform.root;
        // �v���C���[�̎��S�������s��
        if (RespawnManager.CheckInstance())
        {
            trans.GetComponent<PlayerController>().Death(RespawnManager.Instance.m_RevivalInterval);
            // RespawnManager�ɒǉ��˗�����
            RespawnManager.Instance.AddRespawnPlayer(trans);
        }
    }

    private void OnApplicationQuit()
    {
        m_isRun = false;
    }
}
