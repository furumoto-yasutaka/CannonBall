using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ResultSceneController : SingletonMonoBehaviour<ResultSceneController>
{
	public enum STATE
	{
		//PRE_START,	    // 前段階の準備状態
        RESULT_TITLE,       // 結果発表表示状態
		//SHOW_RANK,	    // 一人ひとりの順位を見せる
        //SHOW_WHOLE,       // 全体の順位を見せる
        SHOW_WINNER,        // 勝者を表示
		DETAIL_WINDOW,	    // 詳細ウィンドウ表示
        NEXT_SCENE_SELECT,  // ｽﾃｰｼﾞｾﾚｸﾄorﾀｲﾄﾙか選ぶ状態
    }

    /// <summary> 残機数 </summary>
    [SerializeField, CustomReadOnly]
    private ReactiveProperty<STATE> m_state = new ReactiveProperty<STATE>();

    /// <summary> 残機数(Presenter参照用) </summary>
    public IReadOnlyReactiveProperty<STATE> m_State => m_state;

    private float m_waittime = 2.0f;

    public float m_waittimeCount = 0.0f;

    public void SetState(STATE _state) { m_state.Value = _state; }

    public void SetWaitTime()
    {
        m_waittimeCount = m_waittime;
    }

    protected override void Awake()
    {
        dontDestroyOnLoad = false;
        base.Awake();


        m_state.Value = STATE.RESULT_TITLE;
    }

    private void Update()
    {
        if (m_waittimeCount > 0.0f)
        {
            m_waittimeCount -= Time.deltaTime;
            if (m_waittimeCount < 0.0f)
            {
                m_waittimeCount = 0.0f;
            }
        }
    }
}
