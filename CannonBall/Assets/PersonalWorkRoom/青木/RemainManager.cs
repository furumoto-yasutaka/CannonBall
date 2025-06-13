using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RemainManager : SingletonMonoBehaviour<RemainManager>
{
    [SerializeField, CustomLabel("残機最大値")]
    private int m_remainMax = 99;

    /// <summary> 残機数 </summary>
    private ReactiveProperty<int> m_remain = new ReactiveProperty<int>();

    /// <summary> 残機数(Presenter参照用) </summary>
    public IReadOnlyReactiveProperty<int> m_Remain;


    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();

        //m_remain = new ReactiveProperty<int>(SaveDataManager.Instance.m_SaveData.Value.Remain);
        m_Remain = m_remain;
    }

    // 残機を1増やす関数
    public void AddRemain()
    {
        if (m_remain.Value < m_remainMax)
        {
            m_remain.Value++;
            //SaveDataManager.Instance.SetRemain(m_remain.Value);
        }
    }

    // 残機を1減らす関数
    public void DecRemain()
    {
        if (m_remain.Value > 0)
        {
            m_remain.Value--;
            //SaveDataManager.Instance.SetRemain(m_remain.Value);
        }
    }
}
