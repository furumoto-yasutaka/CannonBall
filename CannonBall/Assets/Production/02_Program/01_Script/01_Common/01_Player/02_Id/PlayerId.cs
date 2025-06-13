/*******************************************************************************
*
*	�^�C�g���F	�v���C���[Id�ێ��X�N���v�g
*	�t�@�C���F	PlayerId.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/09/18
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerId : MonoBehaviour
{
    [SerializeField, CustomLabelReadOnly("�v���C���[ID")]
    private int m_id;

    public int m_Id { get { return m_id; } }

    public void InitId(int id)
    {
        m_id = id;
    }
}
