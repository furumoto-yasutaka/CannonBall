using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerFaceMap", menuName = "CreatePlayerFaceMap")]
public class PlayerFaceMap : ScriptableObject
{
    public enum Face
    {
        Normal = 0,
        Hit,
        Angry,
        Pien,
        Sad,

        Length,
    }

    /// <summary> �\��Ƃ̔z�� </summary>
    [CustomArrayLabel(typeof(Face))]
    public Texture2D[] m_Faces;
}
