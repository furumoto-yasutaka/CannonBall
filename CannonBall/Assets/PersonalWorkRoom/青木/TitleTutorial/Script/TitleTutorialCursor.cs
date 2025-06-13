using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleTutorialCursor : InputElement
{
	enum STATE
	{
		YES,
		NO,
	}



    private NewInputAction_Button m_inputActSubmit;
    private NewInputAction_Button m_inputActRight;
    private NewInputAction_Button m_inputActLeft;

    private STATE m_state = STATE.YES;
    private Image[] m_CorsorImages;
    private SceneChanger m_sceneChanger;
    private TitleTutorialView m_titleTutorialView;
    private Color m_DarkColor = new Color(0.4f, 0.4f, 0.4f, 1.0f);
    private bool m_isSubmit = false;


    private void Start()
    {
        // 入力
        m_inputActSubmit = GetActionMap().GetAction_Button(NewInputActionName_Cursor.Button.CursorSubmit.ToString());
        m_inputActRight = GetActionMap().GetAction_Button(NewInputActionName_Cursor.Button.CursorRight.ToString());
        m_inputActLeft = GetActionMap().GetAction_Button(NewInputActionName_Cursor.Button.CursorLeft.ToString());


        // 参照取得
        m_sceneChanger = GetComponent<SceneChanger>();
        m_titleTutorialView = transform.parent.GetComponent<TitleTutorialView>();


        // 画像
        m_CorsorImages = new Image[transform.childCount];
        for (int i = 0; i < m_CorsorImages.Length; i++)
        {
            m_CorsorImages[i] = transform.GetChild(i).GetComponent<Image>();
        }

        // 画像色変更
        m_CorsorImages[((int)STATE.YES)].color = Color.white;
        m_CorsorImages[((int)STATE.NO)].color = m_DarkColor;

    }

    private void Update()
    {
        // チュートリアルするかどうか決まったら、カーソル移動とかの処理をさせないようにするため
        if (m_isSubmit)
        {
            return;
        }

        // 決定
        if (m_inputActSubmit.GetDownAll())
        {
            switch (m_state)
            {
                case STATE.YES:
                    // シーンを変える
                    // ※※※※現状適当なシーンを選択中※※※※
                    m_sceneChanger.StartSceneChange(SceneNameEnum.Tutorial_RightMove);
                    break;
                case STATE.NO:
                    m_titleTutorialView.m_State = TitleTutorialView.STATE.HIDE;
                    break;
                default:
                    break;
            }


            m_isSubmit = true;
        }
        // カーソル移動
        if (m_inputActRight.GetDownAll() || m_inputActLeft.GetDownAll())
        {
            // YES→NO
            // NO→YES
            if (m_inputActRight.GetDownAll())
            {
                m_state = STATE.NO;
            }
            else if (m_inputActLeft.GetDownAll())
            {
                m_state = STATE.YES;
            }

            //m_state = m_state == STATE.YES ? STATE.NO : STATE.YES;

            // 画像色変更
            switch (m_state)
            {
                case STATE.YES:
                    m_CorsorImages[((int)STATE.YES)].color = Color.white;
                    m_CorsorImages[((int)STATE.NO)].color = m_DarkColor;
                    break;
                case STATE.NO:
                    m_CorsorImages[((int)STATE.YES)].color = m_DarkColor;
                    m_CorsorImages[((int)STATE.NO)].color = Color.white;
                    break;
                default:
                    break;
            }
        }
    }
}
