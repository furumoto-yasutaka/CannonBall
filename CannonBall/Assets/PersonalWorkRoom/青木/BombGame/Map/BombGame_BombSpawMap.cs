using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BombInfo
{
    [CustomLabel("    爆弾のプレハブ")]
    public GameObject m_BombPrefab;

    [CustomLabel("    スポーンさせる数")]
    public int m_Volume = 1;

    [CustomLabel("    複数個ｽﾎﾟｰﾝさせた場合のｽﾎﾟｰﾝ間隔(秒)")]
    public float m_SpawDistance;
}

[System.Serializable]
public class BombSpaw
{
    [CustomLabel("◆爆弾の出現頻度(0.0～1.0)")]
    public float m_BombFrequency;

    // 違うタイプの爆弾が登場する場合、配列を使う
    [CustomLabel("爆弾の情報")]
    public BombInfo[] m_BombType;

}

[CreateAssetMenu(fileName = "BombSpawMap", menuName = "CreateBombSpawMap")]


public class BombGame_BombSpawMap : ScriptableObject
{
    /// <summary> マップの名前 </summary>
    public List<BombSpaw> m_SpawBombMap;

}
