/*******************************************************************************
*
*	タイトル：	シーン遷移処理スクリプト
*	ファイル：	SceneChanger.cs
*	作成者：	古本 泰隆
*	制作日：    2023/04/25
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    /// <summary> 使用するトランジション用プレハブ </summary>
    [SerializeField, CustomLabel("使用するトランジションプレハブ")]
    private GameObject m_transitionPrefab = null;

    /// <summary> 遷移先シーン </summary>
    [SerializeField, CustomLabel("遷移先シーン")]
    private SceneNameEnum m_nextScene;

    /// <summary> ﾄﾗﾝｼﾞｼｮﾝﾌﾟﾚﾊﾌﾞをｷｬﾝﾊﾞｽの子にするか </summary>
    [SerializeField, CustomLabel("ﾄﾗﾝｼﾞｼｮﾝﾌﾟﾚﾊﾌﾞをｷｬﾝﾊﾞｽの子にするか")]
    private bool m_isCanvasChildren = true;

    [Header("(処理に影響はなく遷移先シーンパラメータを使うのか明示するため)")]
    /// <summary> シーンを直接指定するか </summary>
    [SerializeField, CustomLabel("シーンを直接指定するか")]
    private bool m_isSceneDirect = false;

    /// <summary> シーン遷移の際最低何秒待つか </summary>
    [SerializeField, CustomLabel("シーン遷移の際最低何秒待つか")]
    private float m_transitionWaitTime = 1.0f;

    private string m_nextSceneName = "";

    private AsyncOperation m_asyncOperation;

    private float m_startTime = 0.0f;


    /// <summary>
    /// 遷移開始処理(インスタンスに設定されているシーンに遷移)
    /// </summary>
    public void StartSceneChange()
    {
        m_nextSceneName = m_nextScene.ToString();

        SetSceneChange();
    }

    /// <summary>
    /// 遷移開始処理(シーンを直接指定)
    /// </summary>
    /// <param name="scene"> 遷移先シーン </param>
    public void StartSceneChange(SceneNameEnum scene)
    {
        if (!m_isSceneDirect)
        {
            Debug.LogError("シーンを直接指定しない設定なのに呼ばれました");
        }

        m_nextScene = scene;
        StartSceneChange();
    }

    /// <summary>
    /// 遷移開始処理(文字列指定 ※デバッグ用)
    /// </summary>
    public void StartSceneChange(string scenename)
    {
        m_nextSceneName = scenename;

        SetSceneChange();
    }

    /// <summary>
    /// 遷移開始処理(今のシーンに再遷移)
    /// </summary>
    public void StartThisSceneChange()
    {
        m_nextSceneName = SceneManager.GetActiveScene().name;

        SetSceneChange();
    }

    /// <summary>
    /// 遷移予約処理
    /// </summary>
    private void SetSceneChange()
    {
        // すべてのGUI入力を遮断する
        if (InputGroupManager.CheckInstance())
        {
            InputGroupManager.Instance.LockInputGroupOrder(false);
        }

        // トランジション終了後のコールバックに遷移処理を設定
        TransitionCallBack.SetTransitionCallBack(ChangeScene);

        m_asyncOperation = SceneManager.LoadSceneAsync(m_nextSceneName, LoadSceneMode.Single);
        m_asyncOperation.allowSceneActivation = false;
        m_startTime = Time.realtimeSinceStartup;
        StartCoroutine("ChangeSceneWait");
    }

    /// <summary>
    /// 遷移処理
    /// </summary>
    public void ChangeScene()
    {
        Time.timeScale = 1.0f;
        m_asyncOperation.allowSceneActivation = true;
    }

    IEnumerator ChangeSceneWait()
    {
        while (Time.realtimeSinceStartup - m_startTime < m_transitionWaitTime)
        {
            yield return null;
        }

        // トランジションオブジェクトキャンバスの子として生成するか
        if (m_transitionPrefab != null)
        {
            if (m_isCanvasChildren)
            {
                // 生成して親子関係を設定する
                Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                GameObject parent;

                if (canvas == null)
                {
#if UNITY_EDITOR
                    Debug.LogError("Canvasが存在していません");
#endif
                }
                else
                {
                    parent = canvas.gameObject;
                    Instantiate(m_transitionPrefab, parent.transform);
                }
            }
            else
            {
                Instantiate(m_transitionPrefab);
            }
        }
    }
}
