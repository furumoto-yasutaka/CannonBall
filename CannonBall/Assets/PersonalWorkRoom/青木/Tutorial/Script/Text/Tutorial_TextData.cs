/*******************************************************************************
*
*	�^�C�g���F	�`���[�g���A���̕�������
*	�t�@�C���F	Tutorial_TextData.cs
*	�쐬�ҁF	�� �喲
*	������F    2023/10/09
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial_TextData : MonoBehaviour
{
    [SerializeField]
    bool m = false;

    private void Update()
    {
        if (m)
        {
            GetComponent<TextFeed>().NextTextLine();

            m = false;
        }
    }

}
