using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombLastSound : MonoBehaviour
{
    [SerializeField, CustomLabel("���ʔ{��")]
    private float m_soundVolumeRate = 1.0f;


    IBomb m_bomb;

    bool m_isFirstSound = false;
    bool m_isSecondSound = false;
    bool m_isThirdSound = false;
    bool m_isForthSound = false;
    bool m_isFifthSound = false;

    private void Start()
    {
        m_bomb = GetComponent<IBomb>();
    }


    private void Update()
    {
        // �Q�[���̎��Ԃ��I����Ă�����
        if (Timer.Instance.m_TimeCounter <= 0.1f)
        {
            return;
        }

        //if (m_bomb.GetAliveTime() <= 0.4f && !m_isStop)
        //{
        //    AudioManager.Instance.StopSe("���e�J�E���g�_�E��_3");

        //    m_isStop = true;
        //}


        if (m_bomb.GetAliveTime() < 1.0f && !m_isFirstSound)
        {
            AudioManager.Instance.PlaySe("�������O��", false, m_soundVolumeRate);

            m_isFirstSound = true;  
        }
        else if (m_bomb.GetAliveTime() < 2.0f && !m_isSecondSound)
        {
            AudioManager.Instance.PlaySe("���e�J�E���g�_�E��", false, m_soundVolumeRate);

            m_isSecondSound = true;
        }
        else if (m_bomb.GetAliveTime() < 3.0f && !m_isThirdSound)
        {
            AudioManager.Instance.PlaySe("���e�J�E���g�_�E��", false, m_soundVolumeRate);

            m_isThirdSound = true;
        }
        else if (m_bomb.GetAliveTime() < 4.0f && !m_isForthSound)
        {
            AudioManager.Instance.PlaySe("���e�J�E���g�_�E��", false, m_soundVolumeRate);

            m_isForthSound = true;
        }
        else if (m_bomb.GetAliveTime() < 5.0f && !m_isFifthSound)
        {
            AudioManager.Instance.PlaySe("���e�J�E���g�_�E��", false, m_soundVolumeRate);

            m_isFifthSound = true;
        }
    }

}
