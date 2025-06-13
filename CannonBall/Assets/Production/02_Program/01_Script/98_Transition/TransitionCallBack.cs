/*******************************************************************************
*
*	�^�C�g���F	�g�����W�V�����I�����R�[���o�b�N�p�X�N���v�g
*	�t�@�C���F	TransitionCallBack.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/04/25
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCallBack : MonoBehaviour
{
    /// <summary> �J�ڏ������\�b�h </summary>
    private static List<System.Action> m_transitionCallBack = new List<System.Action>();


    /// <summary>
    /// �R�[���o�b�N���ɌĂт����֐����w��
    /// </summary>
    /// <param name="action"> �֐��|�C���^ </param>
    public static void SetTransitionCallBack(System.Action action)
    {
        m_transitionCallBack.Add(action);
    }

    /// <summary>
    /// �g�����W�V�������I��������̃R�[���o�b�N
    /// </summary>
    public void EndTransitionCallBack()
    {
        // �R�[���o�b�N���ݒ肳��Ă����ꍇ���s����
        if (m_transitionCallBack.Count > 0)
        {
            foreach (System.Action method in m_transitionCallBack)
            {
                method();
            }
            m_transitionCallBack.Clear();
        }
    }

    /// <summary>
    /// �g�����W�V�������I��������̃R�[���o�b�N�ƍ폜
    /// </summary>
    public void EndTransitionCallBackAndDelete()
    {
        EndTransitionCallBack();

        Destroy(gameObject);
    }
}
