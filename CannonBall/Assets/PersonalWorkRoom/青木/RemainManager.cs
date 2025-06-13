using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RemainManager : SingletonMonoBehaviour<RemainManager>
{
    [SerializeField, CustomLabel("�c�@�ő�l")]
    private int m_remainMax = 99;

    /// <summary> �c�@�� </summary>
    private ReactiveProperty<int> m_remain = new ReactiveProperty<int>();

    /// <summary> �c�@��(Presenter�Q�Ɨp) </summary>
    public IReadOnlyReactiveProperty<int> m_Remain;


    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();

        //m_remain = new ReactiveProperty<int>(SaveDataManager.Instance.m_SaveData.Value.Remain);
        m_Remain = m_remain;
    }

    // �c�@��1���₷�֐�
    public void AddRemain()
    {
        if (m_remain.Value < m_remainMax)
        {
            m_remain.Value++;
            //SaveDataManager.Instance.SetRemain(m_remain.Value);
        }
    }

    // �c�@��1���炷�֐�
    public void DecRemain()
    {
        if (m_remain.Value > 0)
        {
            m_remain.Value--;
            //SaveDataManager.Instance.SetRemain(m_remain.Value);
        }
    }
}
