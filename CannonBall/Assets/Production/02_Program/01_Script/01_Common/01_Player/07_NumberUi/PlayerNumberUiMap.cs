using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerNumberUiMap", menuName = "CreatePlayerNumberUiMap")]
public class PlayerNumberUiMap : ScriptableObject
{
    /// <summary> マップの名前 </summary>
    [CustomArrayLabel(typeof(PlayerNumber))]
    public Sprite[] m_PlayerNumberSprites;
}
