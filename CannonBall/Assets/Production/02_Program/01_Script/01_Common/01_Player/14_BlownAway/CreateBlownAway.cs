using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBlownAway : MonoBehaviour
{
    /// <summary> �Ԃ���уg���C���G�t�F�N�g�v���n�u </summary>
    [SerializeField, CustomLabel("�Ԃ���уg���C���G�t�F�N�g�v���n�u")]
    private BlownAwayEffectMap m_blownawayMap;


    private void Start()
    {
        Instantiate(m_blownawayMap.m_Effects[transform.root.GetComponent<PlayerId>().m_Id], transform);
        Destroy(this);
    }
}
