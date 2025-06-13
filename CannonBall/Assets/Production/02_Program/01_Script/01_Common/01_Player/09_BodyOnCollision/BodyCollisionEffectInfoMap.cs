using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BodyCollisionEffectInfoMapInfoMap",
    menuName = "CreateBodyCollisionEffectInfoMapInfoMap")]
public class BodyCollisionEffectInfoMap : ScriptableObject
{
    /// <summary> �傫�����Ƃ̏�� </summary>
    [CustomArrayLabel(new string[] { "��", "��", "��", })]
    public string[] m_EffectName;

    /// <summary> �G�t�F�N�g������臒l </summary>
    [CustomArrayLabel(new string[] { "��", "��", "��", })]
    public float[] m_Threshold;
}
