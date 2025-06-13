using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BombRush_PlayerHP : MonoBehaviour, ITemporaryDataGetter
{
    [SerializeField]
    BombGame_PlayAreaInfo _info;

    public float GetRankingParameter()
    {
        return _info.m_Health;
    }
}
