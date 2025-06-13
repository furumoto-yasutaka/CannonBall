using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DetailWindowStarter : MonoBehaviour
{
    [SerializeField, CustomLabel("詳細ｳｨﾝﾄﾞｳ ﾙｰﾄ ｹﾞｰﾑｵﾌﾞｼﾞｪｸﾄ")]
    GameObject m_detailWindow;

    // 詳細ウィンドウで表示する必要がないオブジェクトのルートオブジェクトを選択
    [SerializeField, CustomLabel("勝者発表UI系")]
    GameObject[] m_winnerObjects;

    private void Start()
    {
        ResultSceneController.Instance.m_State.Subscribe(_ =>
        {
            if (_ == ResultSceneController.STATE.DETAIL_WINDOW)
            {
                foreach (var obj in m_winnerObjects)
                {
                    obj.SetActive(false);
                }

                m_detailWindow.SetActive(true);

                ResultSceneController.Instance.SetWaitTime();
            }
        }).AddTo(this);
    }
}
