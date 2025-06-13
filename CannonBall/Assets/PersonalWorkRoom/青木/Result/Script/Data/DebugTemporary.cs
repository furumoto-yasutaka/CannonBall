using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTemporary : MonoBehaviour
{
    public TemporaryData.Rank[] m_ranks;

    private void Awake()
    {
        TemporaryData.m_Rank = m_ranks;
    }
}
