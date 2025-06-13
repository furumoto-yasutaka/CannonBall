using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeSelectWindow : Window
{
    [System.Serializable]
    public struct Controllers
    {
        public TypePanelController PanelController;
        public SubmitIconController SubmitIconController;
        public RectTransform Triangle;
        public RectTransform SubmitOparation;
        public RectTransform CancelOparation;
        public DetailController DetailController;
    }


    [SerializeField, CustomLabel("パネルの制御コンポーネント")]
    private Controllers[] m_controllers;

    [SerializeField, CustomLabel("確認画面制御コンポーネント")]
    private ConfirmController m_confirmController;

    [SerializeField, CustomLabel("選択画面からモード選択へ戻るのにかかる時間")]
    private float m_backPressTime = 3.0f;

    [SerializeField, CustomLabel("モード選択に戻るボタンを押した時間を示す画像")]
    private Image m_backPressProgressImage;

    /// <summary> インプットアクション(カーソル左) </summary>
    private NewInputAction_Button m_inputAct_Cursor_Left;
    /// <summary> インプットアクション(カーソル右) </summary>
    private NewInputAction_Button m_inputAct_Cursor_Right;
    /// <summary> インプットアクション(決定) </summary>
    private NewInputAction_Button m_inputAct_Submit;
    /// <summary> インプットアクション(戻る) </summary>
    private NewInputAction_Button m_inputAct_Cancel;
    /// <summary> インプットアクション(詳細表示) </summary>
    private NewInputAction_Button m_inputAct_Detail;
    /// <summary> インプットアクション(蹴りの瞬間 ※Padスティック) </summary>
    private NewInputAction_Button m_inputAct_Kick_Pad;
    /// <summary> インプットアクション(蹴りの方向 ※Padスティック) </summary>
    private NewInputAction_Vector2 m_inputAct_KickDir_Pad;

    private bool m_isSubmitAll = false;

    private bool m_isSceneEnd = false;

    private float m_backPressTimeCount;


    private void Start()
    {
        m_inputAct_Cursor_Left = GetActionMap().GetAction_Button(NewInputActionName_TypeSelect.Button.Cursor_Left.ToString());
        m_inputAct_Cursor_Right = GetActionMap().GetAction_Button(NewInputActionName_TypeSelect.Button.Cursor_Right.ToString());
        m_inputAct_Submit = GetActionMap().GetAction_Button(NewInputActionName_TypeSelect.Button.Submit.ToString());
        m_inputAct_Cancel = GetActionMap().GetAction_Button(NewInputActionName_TypeSelect.Button.Cancel.ToString());
        m_inputAct_Detail = GetActionMap().GetAction_Button(NewInputActionName_TypeSelect.Button.Detail.ToString());
        m_inputAct_Kick_Pad = GetActionMap().GetAction_Button(NewInputActionName_TypeSelect.Button.Kick_Pad.ToString());
        m_inputAct_KickDir_Pad = GetActionMap().GetAction_Vec2(NewInputActionName_TypeSelect.Vector2.KickDir_Pad.ToString());

        m_backPressTimeCount = m_backPressTime;
    }

    private void Update()
    {
        if (m_isSceneEnd) { return; }

        if (m_isSubmitAll)
        {
            ConfirmUpdate();
        }
        else
        {
            SelectUpdate();
        }
    }

    private void SelectUpdate()
    {
        bool isAllSubmit = true;

        for (int i = 0; i < 4; i++)
        {
            if (!m_controllers[i].SubmitIconController.m_IsSubmit)
            {
                // カーソル移動
                if (m_inputAct_Cursor_Left.GetPress(i))
                {
                    m_controllers[i].PanelController.LeftCursor();
                }
                else if (m_inputAct_Cursor_Right.GetPress(i))
                {
                    m_controllers[i].PanelController.RightCursor();
                }

                // 決定入力
                if (m_inputAct_Submit.GetDown(i))
                {
                    m_controllers[i].SubmitIconController.Submit(i);
                    SubmitSetScale(i);
                    AudioManager.Instance.PlaySe("タイプ選択_決定", false);
                    // コントローラー振動
                    VibrationManager.Instance.SetVibration(i, 10, 0.7f);
                }
            }
            else
            {
                // 決定解除
                if (m_inputAct_Submit.GetDown(i))
                {
                    m_controllers[i].SubmitIconController.Cancel();
                    CancelSetScale(i);
                    AudioManager.Instance.PlaySe("タイプ選択_決定キャンセル", false);
                }
            }

            // 詳細表示
            if (m_inputAct_Detail.GetDown(i))
            {
                if (m_controllers[i].DetailController.m_IsShow)
                {
                    m_controllers[i].DetailController.Hide();
                    AudioManager.Instance.PlaySe("タイプ選択_決定キャンセル", false);
                }
                else
                {
                    m_controllers[i].DetailController.Show();
                    AudioManager.Instance.PlaySe("タイプ選択_詳細ウィンドウ表示", false);
                }
            }

            if (!m_controllers[i].SubmitIconController.m_IsSubmit)
            {
                isAllSubmit = false;
            }
        }

        // 全員が決定状態になったら確認画面を出す
        if (isAllSubmit)
        {
            m_isSubmitAll = true;
            m_confirmController.Show();

            // 選択画面が終わるのでカウントをリセットする
            m_backPressTimeCount = m_backPressTime;
            m_backPressProgressImage.fillAmount = 0.0f;
        }
        else if (m_inputAct_Cancel.GetPressAll())
        {
            m_backPressTimeCount -= Time.deltaTime;

            if (m_backPressTimeCount < 0.0f)
            {
                // 前のシーンに戻る
                GetComponent<SceneChanger>().StartSceneChange();

                m_isSceneEnd = true;
            }
            else
            {
                m_backPressProgressImage.fillAmount = m_backPressTimeCount / m_backPressTime - 0.01f;
            }
        }
        else
        {
            m_backPressTimeCount = m_backPressTime;
            m_backPressProgressImage.fillAmount = 0.0f;
        }
    }

    private void ConfirmUpdate()
    {
        if (m_inputAct_Submit.GetDownAll())
        {
            for (int i = 0; i < 4; i++)
            {
                CreatePlayer.SetInitType(i, (PlayerController.Type)m_controllers[i].PanelController.m_CursorNum);
            }

            m_confirmController.Submit();
            AudioManager.Instance.PlaySe("タイプ選択_最終確認", false);

            // コントローラー振動
            VibrationManager.Instance.SetVibrationAll(10, 0.9f);

            m_isSceneEnd = true;
        }
        else if (m_inputAct_Cancel.GetDownAll())
        {
            // 確認画面から選択画面に戻る
            m_isSubmitAll = false;
            m_confirmController.Hide();
            AudioManager.Instance.PlaySe("タイプ選択_決定キャンセル", false);

            // 全員の決定状態を解除する
            for (int i = 0; i < 4; i++)
            {
                m_controllers[i].SubmitIconController.Cancel();
                CancelSetScale(i);
            }
        }
    }

    private void SubmitSetScale(int i)
    {
        m_controllers[i].Triangle.localScale = Vector3.zero;
        m_controllers[i].SubmitOparation.localScale = Vector3.zero;
        m_controllers[i].CancelOparation.localScale = Vector3.one;
    }

    private void CancelSetScale(int i)
    {
        m_controllers[i].Triangle.localScale = Vector3.one;
        m_controllers[i].SubmitOparation.localScale = Vector3.one;
        m_controllers[i].CancelOparation.localScale = Vector3.zero;
    }
}
