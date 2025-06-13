using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerNumberUiMap", menuName = "CreatePlayerNumberUiMap")]
public class PlayerNumberUiMap : ScriptableObject
{
    /// <summary> �}�b�v�̖��O </summary>
    [CustomArrayLabel(typeof(PlayerNumber))]
    public Sprite[] m_PlayerNumberSprites;
}
