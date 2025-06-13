/*******************************************************************************
*
*	�^�C�g���F	�v���C���[�̃|�C���g�\���p�f�[�^
*	�t�@�C���F	PlayerPoint_CannonFight_Data.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/08
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPoint_CannonFight_View : MonoBehaviour
{
    private int m_tempValue = 0;

    private Animator m_animator;

    private TextMeshProUGUI m_tmp;


    public void Init()
    {
        m_tmp = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        m_animator = GetComponent<Animator>();
    }

    /// <summary> ���_�ݒ� </summary>
    /// <param name="value"> �ݒ肷��l </param>
    public void SetValue(int value, bool isAdd)
    {
        m_tempValue = value;
        
        if (isAdd)
        {
            m_animator.SetTrigger("Add");
        }
        else
        {
            m_animator.SetTrigger("Dec");
        }
    }

    public void ChangeText()
    {
        m_tmp.text = m_tempValue.ToString();
    }
}
