using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerModelMap_CannonFight", menuName = "CreatePlayerModelMap_CannonFight")]
public class PlayerModelMap_CannonFight : ScriptableObject
{
    public enum PlayerType
    {
        Normal = 0,
        Power,
        Speed,
    }

    /// <summary> マップの名前 </summary>
    [CustomArrayLabel(typeof(PlayerType))]
    public PlayerModels[] m_PlayerModelsByType;
}
