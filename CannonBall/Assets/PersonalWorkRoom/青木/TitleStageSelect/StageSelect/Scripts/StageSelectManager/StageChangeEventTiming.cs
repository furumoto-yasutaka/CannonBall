using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class StageChangeEventTiming : SingletonMonoBehaviour<StageChangeEventTiming>
{
    /// <summary> 残機数 </summary>
    private ReactiveProperty<bool> m_ismove = new ReactiveProperty<bool>();

    /// <summary> 残機数(Presenter参照用) </summary>
    public IReadOnlyReactiveProperty<bool> m_isMove;




    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();

        m_ismove.Value = false;
        m_isMove = m_ismove;
    }



    /// <summary>
    /// アニメーション側から使用する
    /// </summary>
    private void ChangeBegin()
    {
        m_ismove.Value = true;
    }


    /// <summary>
    /// アニメーション側から使用する
    /// </summary>
    private void ChangeHalfTiming()
    {
        m_ismove.Value = false;
    }
}
