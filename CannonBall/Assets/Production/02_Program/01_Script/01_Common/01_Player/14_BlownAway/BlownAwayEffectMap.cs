using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlownAwayEffectMap", menuName = "CreateBlownAwayEffectMap")]
public class BlownAwayEffectMap : ScriptableObject
{
    /// <summary> マップの名前 </summary>
    [CustomArrayLabel(typeof(PlayerNumber))]
    public GameObject[] m_Effects;
}
