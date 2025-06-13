using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailController : MonoBehaviour
{
    private Animator m_animator;

    private bool m_isShow = false;

    private RectTransform m_showDetailParent;
    private RectTransform m_leftDetailParent;
    private RectTransform m_rightDetailParent;

    private RectTransform m_showDetail;
    private RectTransform m_leftDetail;
    private RectTransform m_rightDetail;


    public bool m_IsShow { get { return m_isShow; } }


    private void Start()
    {
        m_animator = GetComponent<Animator>();

        m_showDetailParent = transform.GetChild(0).GetComponent<RectTransform>();
        m_leftDetailParent = transform.GetChild(1).GetComponent<RectTransform>();
        m_rightDetailParent = transform.GetChild(2).GetComponent<RectTransform>();

        m_showDetail = m_showDetailParent.GetChild(0).GetComponent<RectTransform>();
        m_leftDetail = m_leftDetailParent.GetChild(0).GetComponent<RectTransform>();
        m_rightDetail = m_rightDetailParent.GetChild(0).GetComponent<RectTransform>();
    }

    public void LeftCursor()
    {
        // 現在表示中のパネルを左へアニメーションさせ、
        // 右で待機中のパネルを左へアニメーションさせる
        m_animator.SetTrigger("MoveLeft");
    }

    public void RightCursor()
    {
        // 現在表示中のパネルを右へアニメーションさせ、
        // 左で待機中のパネルを右へアニメーションさせる
        m_animator.SetTrigger("MoveRight");
    }

    public void LeftCursorCompleted()
    {
        // 表示中だったパネルを左で待機中にする
        m_showDetail.SetParent(m_leftDetailParent);
        m_showDetail.anchoredPosition = Vector2.zero;
        m_showDetail.localScale = Vector3.one;

        // 右で待機中だったパネルを表示中にする
        m_rightDetail.SetParent(m_showDetailParent);
        m_rightDetail.anchoredPosition = Vector2.zero;
        m_rightDetail.localScale = Vector3.one;

        // 右に新しいパネルを待機させる
        m_leftDetail.SetParent(m_rightDetailParent);
        m_leftDetail.anchoredPosition = Vector2.zero;
        m_leftDetail.localScale = Vector3.one;

        RectTransform temp = m_leftDetail;
        m_leftDetail = m_showDetail;
        m_showDetail = m_rightDetail;
        m_rightDetail = temp;
    }

    public void RightCursorCompleted()
    {
        // 表示中だったパネルを右で待機中にする
        m_showDetail.SetParent(m_rightDetailParent);
        m_showDetail.anchoredPosition = Vector2.zero;
        m_showDetail.localScale = Vector3.one;

        // 左で待機中だったパネルを表示中にする
        m_leftDetail.SetParent(m_showDetailParent);
        m_leftDetail.anchoredPosition = Vector2.zero;
        m_leftDetail.localScale = Vector3.one;

        // 左に新しいパネルを待機させる
        m_rightDetail.SetParent(m_leftDetailParent);
        m_rightDetail.anchoredPosition = Vector2.zero;
        m_rightDetail.localScale = Vector3.one;

        RectTransform temp = m_rightDetail;
        m_rightDetail = m_showDetail;
        m_showDetail = m_leftDetail;
        m_leftDetail = temp;
    }

    public void Show()
    {
        m_animator.SetBool("IsShow", true);
        m_isShow = true;
    }

    public void Hide()
    {
        m_animator.SetBool("IsShow", false);
        m_isShow = false;
    }
}
