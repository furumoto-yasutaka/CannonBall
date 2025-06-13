using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BodyCollisionEffectInfoMapInfoMap",
    menuName = "CreateBodyCollisionEffectInfoMapInfoMap")]
public class BodyCollisionEffectInfoMap : ScriptableObject
{
    /// <summary> 大きさごとの情報 </summary>
    [CustomArrayLabel(new string[] { "大", "中", "小", })]
    public string[] m_EffectName;

    /// <summary> エフェクト発生の閾値 </summary>
    [CustomArrayLabel(new string[] { "大", "中", "小", })]
    public float[] m_Threshold;
}
