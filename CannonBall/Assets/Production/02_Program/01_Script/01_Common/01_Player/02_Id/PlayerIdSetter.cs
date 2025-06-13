/*******************************************************************************
*
*	�^�C�g���F	�v���C���[ID�ݒ�X�N���v�g
*	�t�@�C���F	PlayerIdSetter.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/09/18
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdSetter : MonoBehaviour
{
    private void Awake()
    {
        // �v���C���[��ID��t�^
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<PlayerId>().InitId(i);
        }

        // �s�v�Ȃ̂ł��̃R���|�[�l���g���폜
        Destroy(this);
    }
}
