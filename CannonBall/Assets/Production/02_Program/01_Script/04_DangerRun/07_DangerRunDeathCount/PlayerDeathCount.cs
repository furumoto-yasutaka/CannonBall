using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerDeathCount : MonoBehaviour, ITemporaryDataGetter
{
    private ReactiveProperty<int> m_deathCount = new ReactiveProperty<int>(0);


    public IReadOnlyReactiveProperty<int> m_DeathCount => m_deathCount;


    public void AddCount()
    {
        m_deathCount.Value++;
    }

    public void SubCount()
    {
        m_deathCount.Value--;
    }

    public float GetRankingParameter()
    {
        return m_deathCount.Value;
    }
}
