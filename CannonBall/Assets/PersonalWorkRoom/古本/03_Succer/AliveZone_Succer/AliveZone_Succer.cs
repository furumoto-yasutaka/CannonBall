/*******************************************************************************
*
*	�^�C�g���F	�v���C���[���������Ă��邩���f����X�N���v�g(�T�b�J�[���[�hver)
*	�t�@�C���F	AliveZone_Succer.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveZone_Succer : AliveZone
{
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if (collision.CompareTag("Ball"))
        {
            OnTriggerExit_Ball(collision);
        }
    }

    /// <summary> �{�[�������ꂽ�ۂ̃R���W�����C�x���g </summary>
    /// <param name="collision"> �{�[���̃R���W���� </param>
    private void OnTriggerExit_Ball(Collider2D collision)
    {
        // ��O�ɏo���̂Ń{�[���}�l�[�W���[����폜�˗�
        Transform trans = collision.transform.root;
        BallManager.Instance.DestroyBall(trans.gameObject);
    }
}
