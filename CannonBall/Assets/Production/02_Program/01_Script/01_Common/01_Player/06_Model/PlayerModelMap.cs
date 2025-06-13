using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerNumber
{
    _1P = 0,
    _2P,
    _3P,
    _4P,
}

[System.Serializable]
public class PlayerModels
{
    [CustomArrayLabel(typeof(PlayerNumber))]
    public GameObject[] m_Models;
}

[CreateAssetMenu(fileName = "PlayerModelMap", menuName = "CreatePlayerModelMap")]
public class PlayerModelMap : ScriptableObject
{
    /// <summary> マップの名前 </summary>
    public PlayerModels m_PlayerModels;
}
