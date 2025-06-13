/*******************************************************************************
*
*	�^�C�g���F	�v���C���[���G�ꂽ�玀�S����X�N���v�g
*	�t�@�C���F	DeadZone.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/07
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : AliveZone
{
    protected override void OnTriggerExit2D(Collider2D collision) {}

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBody"))
        {
            OnTriggerEnter_Player(collision);
        }
    }

    /// <summary> �v���C���[���G�ꂽ�ۂ̃C�x���g </summary>
    /// <param name="collision"> �v���C���[�̃R���W���� </param>
    protected virtual void OnTriggerEnter_Player(Collider2D collision)
    {
        OnTriggerExit_Player(collision);
    }
}
