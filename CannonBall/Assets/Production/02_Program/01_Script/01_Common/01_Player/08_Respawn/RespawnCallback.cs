using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCallback : MonoBehaviour
{
    private PlayerController m_playerController;


    private void Start()
    {
        m_playerController = transform.root.GetComponent<PlayerController>();
    }

    public void PositionAdjust()
    {
        transform.parent.localPosition = Vector3.zero;
        transform.parent.localRotation = Quaternion.identity;
    }

    public void FinishCallback_Respawn()
    {
        m_playerController.JumpOutFinishCallback_Respawn();
    }

    public void FinishCallback_FirstRespawn()
    {
        m_playerController.JumpOutFinishCallback_FirstRespawn();
    }
}
