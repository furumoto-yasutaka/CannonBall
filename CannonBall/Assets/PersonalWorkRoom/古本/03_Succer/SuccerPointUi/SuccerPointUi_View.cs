/*******************************************************************************
*
*	�^�C�g���F	�T�b�J�[�̓��_�\������X�N���v�g
*	�t�@�C���F	SuccerPointUi_View.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SuccerPointUi_View : MonoBehaviour
{
    /// <summary> ���_�ݒ� </summary>
    /// <param name="value"> �ݒ肷��l </param>
    public void SetValue(int value)
    {
        TextMeshPro text = transform.GetChild(0).GetComponent<TextMeshPro>();
        text.text = value.ToString();
    }
}
