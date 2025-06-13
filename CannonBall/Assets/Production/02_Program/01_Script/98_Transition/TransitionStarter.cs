/*******************************************************************************
*
*	タイトル：	遷移処理開始スクリプト
*	ファイル：	TransitionStarter.cs
*	作成者：	古本 泰隆
*	制作日：    2023/04/25　
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionStarter : MonoBehaviour
{
    /// <summary> 使用するトランジションプレハブ </summary>
    [SerializeField, CustomLabel("使用するトランジションプレハブ")]
    private GameObject m_transitionPrefab;

    /// <summary> トランジションプレハブをキャンバスの子とするか </summary>
    [SerializeField, CustomLabel("トランジションプレハブをキャンバスの子とするか")]
    private bool m_isCanvasChildren = true;


    void Start()
    {
        if (m_isCanvasChildren)
        {
            // 生成して親子関係を設定する
            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            GameObject parent;

            if (canvas == null)
            {
#if UNITY_EDITOR
                Debug.LogError("キャンバスがCanvasConteinerに存在していません");
#endif
                return;
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

        // この後は不要なので削除する
        Destroy(gameObject);
    }
}
