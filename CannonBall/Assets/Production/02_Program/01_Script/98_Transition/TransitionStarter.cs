/*******************************************************************************
*
*	�^�C�g���F	�J�ڏ����J�n�X�N���v�g
*	�t�@�C���F	TransitionStarter.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/04/25�@
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionStarter : MonoBehaviour
{
    /// <summary> �g�p����g�����W�V�����v���n�u </summary>
    [SerializeField, CustomLabel("�g�p����g�����W�V�����v���n�u")]
    private GameObject m_transitionPrefab;

    /// <summary> �g�����W�V�����v���n�u���L�����o�X�̎q�Ƃ��邩 </summary>
    [SerializeField, CustomLabel("�g�����W�V�����v���n�u���L�����o�X�̎q�Ƃ��邩")]
    private bool m_isCanvasChildren = true;


    void Start()
    {
        if (m_isCanvasChildren)
        {
            // �������Đe�q�֌W��ݒ肷��
            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            GameObject parent;

            if (canvas == null)
            {
#if UNITY_EDITOR
                Debug.LogError("�L�����o�X��CanvasConteiner�ɑ��݂��Ă��܂���");
#endif
                return;
            }
            else
            {
                parent = canvas.gameObject;
                Instantiate(m_transitionPrefab, parent.transform);
            }
        }
        else
        {
            Instantiate(m_transitionPrefab);
        }

        // ���̌�͕s�v�Ȃ̂ō폜����
        Destroy(gameObject);
    }
}
