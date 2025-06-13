using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypePanelController : MonoBehaviour
{
    [SerializeField, CustomLabelReadOnly("カーソル番号")]
    private int m_cursorNum = 0;

    [SerializeField, CustomLabel("プレイヤーモデル設定コンポーネント")]
    private PlayerModelSetter_TypeSelect m_modelSetter;

    [SerializeField, CustomLabel("詳細表示制御コンポーネント")]
    private DetailController m_detailController;

    private Animator m_animator;

    private RectTransform m_showPanelParent;
    private RectTransform m_leftPanelParent;
    private RectTransform m_rightPanelParent;

    private RectTransform m_showPanel;
    private RectTransform m_leftPanel;
    private RectTransform m_rightPanel;

    private bool m_isAnimation = false;

    public int m_CursorNum { get { return m_cursorNum; } }


    private void Start()
    {
        m_animator = GetComponent<Animator>();

        m_showPanelParent = transform.GetChild(0).GetComponent<RectTransform>();
        m_leftPanelParent = transform.GetChild(1).GetComponent<RectTransform>();
        m_rightPanelParent = transform.GetChild(2).GetComponent<RectTransform>();

        m_showPanel = m_showPanelParent.GetChild(0).GetComponent<RectTransform>();
        m_leftPanel = m_leftPanelParent.GetChild(0).GetComponent<RectTransform>();
        m_rightPanel = m_rightPanelParent.GetChild(0).GetComponent<RectTransform>();

        m_modelSetter.ChangeModel(m_cursorNum);
    }

    public void LeftCursor()
    {
        if (m_isAnimation) { return; }

        // 現在表示中のパネルを左へアニメーションさせ、
        // 右で待機中のパネルを左へアニメーションさせる
        m_animator.SetTrigger("MoveLeft");

        m_cursorNum = (m_cursorNum + 1) % 3;
        m_isAnimation = true;

        m_modelSetter.ChangeModel(m_cursorNum);
        m_detailController.LeftCursor();

        AudioManager.Instance.PlaySe("タイプ選択_カーソル移動", false);
    }

    public void RightCursor()
    {
        if (m_isAnimation) { return; }

        // 現在表示中のパネルを右へアニメーションさせ、
        // 左で待機中のパネルを右へアニメーションさせる
        m_animator.SetTrigger("MoveRight");

        m_cursorNum = (m_cursorNum - 1 + 3) % 3;
        m_isAnimation = true;

        m_modelSetter.ChangeModel(m_cursorNum);
        m_detailController.RightCursor();

        AudioManager.Instance.PlaySe("タイプ選択_カーソル移動", false);
    }

    public void LeftCursorCompleted()
    {
        // 表示中だったパネルを左で待機中にする
        m_showPanel.SetParent(m_leftPanelParent);
        m_showPanel.anchoredPosition = Vector2.zero;
        m_showPanel.localScale = Vector3.one;

        // 右で待機中だったパネルを表示中にする
        m_rightPanel.SetParent(m_showPanelParent);
        m_rightPanel.anchoredPosition = Vector2.zero;
        m_rightPanel.localScale = Vector3.one;

        // 右に新しいパネルを待機させる
        m_leftPanel.SetParent(m_rightPanelParent);
        m_leftPanel.anchoredPosition = Vector2.zero;
        m_leftPanel.localScale = Vector3.one;

        RectTransform temp = m_leftPanel;
        m_leftPanel = m_showPanel;
        m_showPanel = m_rightPanel;
        m_rightPanel = temp;

        m_isAnimation = false;
    }

    public void RightCursorCompleted()
    {
        // 表示中だったパネルを右で待機中にする
        m_showPanel.SetParent(m_rightPanelParent);
        m_showPanel.anchoredPosition = Vector2.zero;
        m_showPanel.localScale = Vector3.one;

        // 左で待機中だったパネルを表示中にする
        m_leftPanel.SetParent(m_showPanelParent);
        m_leftPanel.anchoredPosition = Vector2.zero;
        m_leftPanel.localScale = Vector3.one;

        // 左に新しいパネルを待機させる
        m_rightPanel.SetParent(m_leftPanelParent);
        m_rightPanel.anchoredPosition = Vector2.zero;
        m_rightPanel.localScale = Vector3.one;

        RectTransform temp = m_rightPanel;
        m_rightPanel = m_showPanel;
        m_showPanel = m_leftPanel;
        m_leftPanel = temp;

        m_isAnimation = false;
    }
}
