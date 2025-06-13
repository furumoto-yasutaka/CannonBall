/*******************************************************************************
*
*	タイトル：	TitleScene　StageSelectSceneの最上位管理
*	内容　　：　Title ⇄ StageSelectの変更する
*	ファイル：	TitleStageSelectManager.cs
*	作成者：	青木 大夢
*	制作日：    2023/09/12
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TitleStageSelectManager : SingletonMonoBehaviour<TitleStageSelectManager>
{
    public enum Scene
    {
        Title,
        Tutorial,
        StageSelect
    }

    [SerializeField]
    private bool m_isTitle = true;

    [Header("タイトルのRootObject")]
    [SerializeField]
    private GameObject m_TitleInGameRootObject;

    [SerializeField]
    private GameObject m_TitleCanvasRootObject;


    [Header("チュートリアルのRootObject")]
    [SerializeField]
    private GameObject m_TutorialRootObject;

    [SerializeField]
    private GameObject m_TutorialCanvasRoot;


    [Header("ステージセレクトのRootObject")]
    [SerializeField]
    private GameObject m_StageSelectInGameRootObject;

    [SerializeField]
    private GameObject m_StageSelectCanvasRootObject;


    // 現在のシーン
    Scene m_CurrentScene = Scene.Title;


    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();

        if (!m_isTitle)
        {
            return;
        }

        m_TitleInGameRootObject.SetActive(true);
        m_TitleCanvasRootObject.SetActive(true);

        m_TutorialRootObject.SetActive(true);
        m_TutorialCanvasRoot.SetActive(true);

        m_StageSelectInGameRootObject.SetActive(true);
        m_StageSelectCanvasRootObject.SetActive(true);

    }

    private void Start()
    {
        if (!m_isTitle)
        {
            return;
        }

        m_TitleInGameRootObject.SetActive(true);
        m_TitleCanvasRootObject.SetActive(true);

        m_TutorialRootObject.SetActive(false);
        m_TutorialCanvasRoot.SetActive(false);

        m_StageSelectInGameRootObject.SetActive(false);
        m_StageSelectCanvasRootObject.SetActive(false);
    }


    // タイトル状態からステージセレクト状態に遷移する
    public void ExchangeStageSelect(Scene _scene)
    {
        if (!m_isTitle)
        {
            return;
        }

        // 同じシーンに遷移しようとしたら直ぐに返す
        if (m_CurrentScene == _scene)
        {
            return;
        }

        switch (_scene)
        {
            case Scene.Title:
                // タイトル
                m_TitleInGameRootObject.SetActive(true);
                m_TitleCanvasRootObject.SetActive(true);

                m_StageSelectInGameRootObject.SetActive(false);
                m_StageSelectCanvasRootObject.SetActive(false);
                break;
            case Scene.Tutorial:
                // タイトル
                m_TitleInGameRootObject.SetActive(false);
                m_TitleCanvasRootObject.SetActive(false);

                // チュートリアル
                m_TutorialRootObject.SetActive(true);
                m_TutorialCanvasRoot.SetActive(true);
                break;
            case Scene.StageSelect:
                // チュートリアル
                m_TutorialRootObject.SetActive(false);
                m_TutorialCanvasRoot.SetActive(false);

                // ステージセレクト
                m_StageSelectInGameRootObject.SetActive(true);
                m_StageSelectCanvasRootObject.SetActive(true);
                break;
        }

        m_CurrentScene = _scene;
    }


}
