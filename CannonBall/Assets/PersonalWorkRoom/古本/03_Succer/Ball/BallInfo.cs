/*******************************************************************************
*
*	�^�C�g���F	�{�[�����X�N���v�g
*	�t�@�C���F	BallInfo.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/05
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInfo : MonoBehaviour
{
    /// <summary> ���A�{�[�����ǂ��� </summary>
    [SerializeField, CustomLabel("���A�{�[�����ǂ���")]
    private bool m_isRareBall = false;


    public bool m_IsRareBall { get { return m_isRareBall; } }
}
