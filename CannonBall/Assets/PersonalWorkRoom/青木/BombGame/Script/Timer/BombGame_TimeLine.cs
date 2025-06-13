/*******************************************************************************
*
*	タイトル：	爆弾ゲームの時間によって変わるイベントをライン上にわかりやすく記述
*	ファイル：	BombGame_TimeLine.cs
*	作成者：	青木　大夢
*	制作日：    2023/10/19
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class BombGame_TimeLine : SingletonMonoBehaviour<BombGame_TimeLine>
{
    [SerializeField]
    Timer m_timer;

    //[Space(20)]

    //[Header("30秒前のイベント")]
    //[SerializeField]
    //UnityEvent m_30secondAction;

    //[Header("60秒前のイベント")]
    //[SerializeField]
    //UnityEvent m_60secondAction;

    [SerializeField, CustomLabel("演出を生成するCanvas")]
    Canvas m_canvas;

    [SerializeField, CustomLabel("ワールド座標用Canvas")]
    Canvas m_worldCanvas;


    [SerializeField, CustomLabel("60秒前の演出")]
    GameObject m_60secondDecorationPrehab;


    bool m_is60second = false;
    bool m_is30second = false;
    

    public bool GetIs60second { get { return m_is60second; } }

    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();
    }


    private void Start()
    {

    }


    private void Update()
    {
        // 残り30秒になったら
        if (m_timer.m_TimeCounter <= 30 && !m_is30second)
        {
            //m_30secondAction.Invoke();

            BombManager.Instance.SetCurrentSpawMapIndex(2);

            //foreach (GameObject go in m_30secondDecorationPrehab)
            //{
            //    GameObject game = Instantiate(go);
            //    game.transform.parent = m_canvas.transform;
            //}

            m_is30second = true;
        }
        // 残り60秒になったら
        if (m_timer.m_TimeCounter <= 60 && !m_is60second)
        {
            //m_60secondAction.Invoke();

            GameObject game = Instantiate(m_60secondDecorationPrehab);
            game.transform.parent = m_worldCanvas.transform;
            game.transform.localPosition = Vector3.zero;

            game.transform.localScale = Vector3.one;



            m_is60second = true;
        }

    }



    //public void TestEvet()
    //{
    //    Debug.Log("TestEvet");
    //}

    //public void a40Event()
    //{
    //    Debug.Log("a40Event");
    //}



}
