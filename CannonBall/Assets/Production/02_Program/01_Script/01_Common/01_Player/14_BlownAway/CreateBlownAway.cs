using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBlownAway : MonoBehaviour
{
    /// <summary> ぶっ飛びトレイルエフェクトプレハブ </summary>
    [SerializeField, CustomLabel("ぶっ飛びトレイルエフェクトプレハブ")]
    private BlownAwayEffectMap m_blownawayMap;


    private void Start()
    {
        Instantiate(m_blownawayMap.m_Effects[transform.root.GetComponent<PlayerId>().m_Id], transform);
        Destroy(this);
    }
}
