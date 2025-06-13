using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombLastSound : MonoBehaviour
{
    [SerializeField, CustomLabel("音量倍率")]
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
        // ゲームの時間が終わっていたら
        if (Timer.Instance.m_TimeCounter <= 0.1f)
        {
            return;
        }

        //if (m_bomb.GetAliveTime() <= 0.4f && !m_isStop)
        //{
        //    AudioManager.Instance.StopSe("爆弾カウントダウン_3");

        //    m_isStop = true;
        //}


        if (m_bomb.GetAliveTime() < 1.0f && !m_isFirstSound)
        {
            AudioManager.Instance.PlaySe("爆発直前音", false, m_soundVolumeRate);

            m_isFirstSound = true;  
        }
        else if (m_bomb.GetAliveTime() < 2.0f && !m_isSecondSound)
        {
            AudioManager.Instance.PlaySe("爆弾カウントダウン", false, m_soundVolumeRate);

            m_isSecondSound = true;
        }
        else if (m_bomb.GetAliveTime() < 3.0f && !m_isThirdSound)
        {
            AudioManager.Instance.PlaySe("爆弾カウントダウン", false, m_soundVolumeRate);

            m_isThirdSound = true;
        }
        else if (m_bomb.GetAliveTime() < 4.0f && !m_isForthSound)
        {
            AudioManager.Instance.PlaySe("爆弾カウントダウン", false, m_soundVolumeRate);

            m_isForthSound = true;
        }
        else if (m_bomb.GetAliveTime() < 5.0f && !m_isFifthSound)
        {
            AudioManager.Instance.PlaySe("爆弾カウントダウン", false, m_soundVolumeRate);

            m_isFifthSound = true;
        }
    }

}
