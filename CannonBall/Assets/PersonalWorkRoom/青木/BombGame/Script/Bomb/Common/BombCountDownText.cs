/*******************************************************************************
*
*	�^�C�g���F	���e�������Ă���c��b����\��
*	�t�@�C���F	BombCountDown.cs
*	�쐬�ҁF	�� �喲
*	������F    2023/09/19
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombCountDownText : MonoBehaviour
{
    //[SerializeField, CustomLabel("�ő包��")]
    //int m_maxDigit;

    [SerializeField, CustomLabel("�����Ɛ����̊Ԃ̋���")]
    float m_distance;


    [SerializeField, CustomLabel("�����̃X�v���C�g")]
    Sprite[] m_numberSprit;

    SpriteRenderer[] m_childrenSpriteRender;

    IBomb m_bomb;

    /// <summary>  </summary>
    int m_nowDigit = 0;

    /// <summary> 
    /// �P�t���[���O�̌��� 
    /// �J�E���g�_�E���Ō����������Ƃ��̃t���O�����p�̕ϐ�
    /// </summary>
    int m_preDigit = 0;

    [SerializeField]
    GameObject obbb;

    void Start()
    {
        m_bomb = transform.parent.GetComponent<IBomb>();

        int max = NowDigit(m_bomb.GetAliveTime());

        m_childrenSpriteRender = new SpriteRenderer[max];

        for (int i = 0; i < max; i++)
        {
            GameObject obj = Instantiate(obbb, new Vector3(GetDigitPosition(max, i + 1).x, 0.0f, 0.0f), Quaternion.identity);
            obj.transform.parent = transform;

            m_childrenSpriteRender[i] = obj.GetComponent<SpriteRenderer>();
        }

        m_nowDigit = max;
        m_preDigit = max;
    }


    void Update()
    {
        // �b������]�����Ȃ�
        transform.localRotation = Quaternion.identity * Quaternion.Inverse(transform.parent.rotation);

        // ���݂̌������擾
        m_nowDigit = NowDigit(m_bomb.GetAliveTime());

        for (int i = 0; i < m_nowDigit; i++)
        {
            m_childrenSpriteRender[i].gameObject.transform.localPosition = new Vector3(GetDigitPosition(m_nowDigit, i + 1).x, 0.0f, 0.0f);

            m_childrenSpriteRender[i].sprite = m_numberSprit[GetNumber(i, m_bomb.GetAliveTime())];
        }

        //Debug.Log("NowDigit" + m_nowDigit);
        //Debug.Log("m_preDigit" + m_preDigit);

        // ������������
        if (m_preDigit != m_nowDigit)
        {
            m_childrenSpriteRender[m_preDigit - 1].enabled = false;
        }

        m_preDigit = m_nowDigit;
    }


    private Vector2 GetDigitPosition(int maxDigit, int targetDigit)
    {
        float posx;

        // ����
        if (maxDigit % 2 == 0)
        {
            // ������������
            posx = ((maxDigit * 0.5f - targetDigit) + 0.5f) * m_distance;
            //Debug.Log("����" + (maxDigit * 0.5f - targetDigit) + m_distance * 0.5f);
        }
        // �
        else
        {
            // ���������
            posx = ((int)(maxDigit * 0.5f) + 1 - targetDigit) * m_distance;

            //Debug.Log("�" + ((int)(maxDigit * 0.5f) + 1 - targetDigit));
        }


        return new Vector2(posx, 0.0f);
    }


    private int NowDigit(float number)
    {
        int i = 0;
        
        float a = number;

        do
        {
            a *= 0.1f;

            i++;

        } while (a >= 1);

        return i;
    }

    /// <summary>
    /// �w��̌�������A���������o��
    /// </summary>
    /// <param name="digit"></param>
    /// <returns></returns>
    private int GetNumber(int digit, float time)
    {
        int num = 1;

        for (int i = 0; i < digit; i++)
        {
            num *= 10;
        }

        int t = (int)time / num;


        t %= 10;


        return t;
    }
}
