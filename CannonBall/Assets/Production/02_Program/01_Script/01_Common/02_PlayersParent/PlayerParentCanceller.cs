/*******************************************************************************
*
*	�^�C�g���F	�v���C���[�̃y�A�����g�����X�N���v�g
*	�t�@�C���F	PlayerParentCanceller.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/01
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParentCanceller : MonoBehaviour
{
    private void Awake()
    {
        // �y�A�����g����
        while (transform.childCount > 0)
        {
            transform.GetChild(0).parent = null;
        }

        // �s�v�Ȃ̂ł��̃R���|�[�l���g���폜
        Destroy(this);
    }
}
