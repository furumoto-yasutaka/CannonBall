using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSpMoveCharge : MonoBehaviour
{
    /// <summary> 必殺技チャージエフェクト </summary>
    [SerializeField, CustomLabel("必殺技チャージエフェクト")]
    private SpMoveChargeEffectMap m_spmovechargeMap;


    private void Start()
    {
        Instantiate(m_spmovechargeMap.m_Effects[transform.root.GetComponent<PlayerId>().m_Id], transform);
        Destroy(this);
    }
}
