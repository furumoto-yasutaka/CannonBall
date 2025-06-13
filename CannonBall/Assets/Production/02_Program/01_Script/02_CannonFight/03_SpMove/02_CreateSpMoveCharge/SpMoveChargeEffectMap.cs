using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpMoveChargeEffectMap", menuName = "CreateSpMoveChargeEffectMap")]
public class SpMoveChargeEffectMap : ScriptableObject
{
    /// <summary> マップの名前 </summary>
    [CustomArrayLabel(typeof(PlayerNumber))]
    public GameObject[] m_Effects;
}
