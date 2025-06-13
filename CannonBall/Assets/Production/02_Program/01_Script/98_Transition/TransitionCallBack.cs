/*******************************************************************************
*
*	タイトル：	トランジション終了時コールバック用スクリプト
*	ファイル：	TransitionCallBack.cs
*	作成者：	古本 泰隆
*	制作日：    2023/04/25
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionCallBack : MonoBehaviour
{
    /// <summary> 遷移処理メソッド </summary>
    private static List<System.Action> m_transitionCallBack = new List<System.Action>();


    /// <summary>
    /// コールバック時に呼びたい関数を指定
    /// </summary>
    /// <param name="action"> 関数ポインタ </param>
    public static void SetTransitionCallBack(System.Action action)
    {
        m_transitionCallBack.Add(action);
    }

    /// <summary>
    /// トランジションが終わった時のコールバック
    /// </summary>
    public void EndTransitionCallBack()
    {
        // コールバックが設定されていた場合実行する
        if (m_transitionCallBack.Count > 0)
        {
            foreach (System.Action method in m_transitionCallBack)
            {
                method();
            }
            m_transitionCallBack.Clear();
        }
    }

    /// <summary>
    /// トランジションが終わった時のコールバックと削除
    /// </summary>
    public void EndTransitionCallBackAndDelete()
    {
        EndTransitionCallBack();

        Destroy(gameObject);
    }
}
