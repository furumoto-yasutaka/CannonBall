/*******************************************************************************
*
*	�^�C�g���F	���͎�t�v�f�̊��N���X(�f�o�b�O�p)
*	�t�@�C���F	DebugInputElement.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2024/01/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DebugInputElement : MonoBehaviour
{
    [SerializeField, CustomLabel("�C���v�b�g�A�N�V�����}�b�v")]
    private NewInputActionMap m_inputAcitonMap;


    public NewInputActionMap GetActionMap()
    {
        return m_inputAcitonMap;
    }


    protected virtual void Awake()
    {
        m_inputAcitonMap.Init();
        m_inputAcitonMap.Enable();
    }

    protected virtual void Update()
    {
        m_inputAcitonMap.Update();
    }
}
