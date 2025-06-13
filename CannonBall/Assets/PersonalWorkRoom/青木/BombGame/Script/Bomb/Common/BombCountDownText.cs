/*******************************************************************************
*
*	タイトル：	爆弾が持っている残り秒数を表示
*	ファイル：	BombCountDown.cs
*	作成者：	青木 大夢
*	制作日：    2023/09/19
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombCountDownText : MonoBehaviour
{
    //[SerializeField, CustomLabel("最大桁数")]
    //int m_maxDigit;

    [SerializeField, CustomLabel("数字と数字の間の距離")]
    float m_distance;


    [SerializeField, CustomLabel("数字のスプライト")]
    Sprite[] m_numberSprit;

    SpriteRenderer[] m_childrenSpriteRender;

    IBomb m_bomb;

    /// <summary>  </summary>
    int m_nowDigit = 0;

    /// <summary> 
    /// １フレーム前の桁数 
    /// カウントダウンで桁が減ったときのフラグを取る用の変数
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
        // 秒数を回転させない
        transform.localRotation = Quaternion.identity * Quaternion.Inverse(transform.parent.rotation);

        // 現在の桁数を取得
        m_nowDigit = NowDigit(m_bomb.GetAliveTime());

        for (int i = 0; i < m_nowDigit; i++)
        {
            m_childrenSpriteRender[i].gameObject.transform.localPosition = new Vector3(GetDigitPosition(m_nowDigit, i + 1).x, 0.0f, 0.0f);

            m_childrenSpriteRender[i].sprite = m_numberSprit[GetNumber(i, m_bomb.GetAliveTime())];
        }

        //Debug.Log("NowDigit" + m_nowDigit);
        //Debug.Log("m_preDigit" + m_preDigit);

        // 桁が減ったら
        if (m_preDigit != m_nowDigit)
        {
            m_childrenSpriteRender[m_preDigit - 1].enabled = false;
        }

        m_preDigit = m_nowDigit;
    }


    private Vector2 GetDigitPosition(int maxDigit, int targetDigit)
    {
        float posx;

        // 偶数
        if (maxDigit % 2 == 0)
        {
            // 偶数だったら
            posx = ((maxDigit * 0.5f - targetDigit) + 0.5f) * m_distance;
            //Debug.Log("偶数" + (maxDigit * 0.5f - targetDigit) + m_distance * 0.5f);
        }
        // 奇数
        else
        {
            // 奇数だったら
            posx = ((int)(maxDigit * 0.5f) + 1 - targetDigit) * m_distance;

            //Debug.Log("奇数" + ((int)(maxDigit * 0.5f) + 1 - targetDigit));
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
    /// 指定の桁数から、数字を取り出す
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
