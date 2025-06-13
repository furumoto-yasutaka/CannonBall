using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class StageChangeEventTiming : SingletonMonoBehaviour<StageChangeEventTiming>
{
    /// <summary> �c�@�� </summary>
    private ReactiveProperty<bool> m_ismove = new ReactiveProperty<bool>();

    /// <summary> �c�@��(Presenter�Q�Ɨp) </summary>
    public IReadOnlyReactiveProperty<bool> m_isMove;




    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();

        m_ismove.Value = false;
        m_isMove = m_ismove;
    }



    /// <summary>
    /// �A�j���[�V����������g�p����
    /// </summary>
    private void ChangeBegin()
    {
        m_ismove.Value = true;
    }


    /// <summary>
    /// �A�j���[�V����������g�p����
    /// </summary>
    private void ChangeHalfTiming()
    {
        m_ismove.Value = false;
    }
}
