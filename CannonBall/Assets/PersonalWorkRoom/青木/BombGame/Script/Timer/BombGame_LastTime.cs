/*******************************************************************************
*
*	タイトル：	ラスト〇秒！　の演出処理
*	ファイル：	BombGame_LastTime.cs
*	作成者：	青木　大夢
*	制作日：    2023/10/20
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombGame_LastTime : MonoBehaviour
{
    [SerializeField, CustomLabelReadOnly("何秒からカウントダウンするか")]
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
        // 時間を減らす前に時間を記録
        // 小数点切り捨て
        int time = Mathf.FloorToInt(m_lastTime);

        m_lastTime -= Time.deltaTime;

        // ０秒になったら、別の演出に切り替わるため、これを削除
        if (m_lastTime <= 0.0f)
        {
            Destroy(gameObject);
        }
        // 現在 < 前
        else if (Mathf.FloorToInt(m_lastTime) < time)
        {
            m_image.sprite = Sprits[time - 1];

            m_animator.Play("LastTime", 0, 0);
        }
    }


}
