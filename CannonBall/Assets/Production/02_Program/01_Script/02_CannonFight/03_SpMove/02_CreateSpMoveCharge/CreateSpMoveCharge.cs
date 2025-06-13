using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSpMoveCharge : MonoBehaviour
{
    /// <summary> �K�E�Z�`���[�W�G�t�F�N�g </summary>
    [SerializeField, CustomLabel("�K�E�Z�`���[�W�G�t�F�N�g")]
    private SpMoveChargeEffectMap m_spmovechargeMap;


    private void Start()
    {
        Instantiate(m_spmovechargeMap.m_Effects[transform.root.GetComponent<PlayerId>().m_Id], transform);
        Destroy(this);
    }
}
