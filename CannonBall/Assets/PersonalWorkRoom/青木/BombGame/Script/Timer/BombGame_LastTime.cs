/*******************************************************************************
*
*	�^�C�g���F	���X�g�Z�b�I�@�̉��o����
*	�t�@�C���F	BombGame_LastTime.cs
*	�쐬�ҁF	�؁@�喲
*	������F    2023/10/20
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombGame_LastTime : MonoBehaviour
{
    [SerializeField, CustomLabelReadOnly("���b����J�E���g�_�E�����邩")]
    float m_lastTime = 5;


    [SerializeField]
    Sprite[] Sprits;

    Image m_image;
    Animator m_animator;


    private void Awake()
    {
        GetComponent<RectTransform>().localPosition = Vector3.zero;
    }

    private void Start()
    {
        m_image = GetComponent<Image>();
        m_animator = GetComponent<Animator>();

        m_animator.Play("LastTime", 0, 0);
    }

    private void Update()
    {
        // ���Ԃ����炷�O�Ɏ��Ԃ��L�^
        // �����_�؂�̂�
        int time = Mathf.FloorToInt(m_lastTime);

        m_lastTime -= Time.deltaTime;

        // �O�b�ɂȂ�����A�ʂ̉��o�ɐ؂�ւ�邽�߁A������폜
        if (m_lastTime <= 0.0f)
        {
            Destroy(gameObject);
        }
        // ���� < �O
        else if (Mathf.FloorToInt(m_lastTime) < time)
        {
            m_image.sprite = Sprits[time - 1];

            m_animator.Play("LastTime", 0, 0);
        }
    }


}
