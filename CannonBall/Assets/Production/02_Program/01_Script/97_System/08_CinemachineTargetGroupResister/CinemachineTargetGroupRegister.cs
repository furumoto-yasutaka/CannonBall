using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineTargetGroupRegister : MonoBehaviour
{
    [SerializeField, CustomLabel("�v���C���[�o�^���̃E�F�C�g")]
    private float m_resistWeight = 5;

    [SerializeField, CustomLabel("�v���C���[�o�^���͈̔�")]
    private float m_resistRadius = 3;

    private CinemachineTargetGroup m_targetGroup;


    private void Awake()
    {
        m_targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    public void Resist(Transform player)
    {
        m_targetGroup.AddMember(player, m_resistWeight, m_resistRadius);
    }

    public void Delete(Transform player)
    {
        m_targetGroup.RemoveMember(player);
    }
}
