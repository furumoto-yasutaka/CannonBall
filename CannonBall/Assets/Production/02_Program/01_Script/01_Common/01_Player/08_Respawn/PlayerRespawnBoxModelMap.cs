using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRespawnBoxModelMap", menuName = "CreatePlayerRespawnBoxModelMap")]
public class PlayerRespawnBoxModelMap : ScriptableObject
{
    /// <summary> マップの名前 </summary>
    public PlayerModels m_PlayerRespawnBoxModels;
}
