using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager_DangerRun : RespawnManager
{
    /// <summary> スクロールするカメラ </summary>
    [SerializeField, CustomLabel("スクロールするカメラ")]
    protected Transform m_camera;


    public Transform m_Camera { get { return m_camera; } }


    protected override void StartRespawn(Transform player)
    {
        GameObject truck = Instantiate(m_truckPrefab, m_camera);
        truck.GetComponent<RespawnTruck>().SetPlayer(player);
    }

    public override void EndRespawn(Transform player)
    {
        for (int i = 0; i < m_respownList.Count; i++)
        {
            if (m_respownList[i].m_Player == player)
            {
                m_respownList.RemoveAt(i);
                break;
            }
        }

        SetRespawnAnimation(null);
    }

    public override void AddRespawnPlayer(Transform player)
    {
        m_respownList.Add(new RespownInfo(player, m_respownInterval));
    }

    public override void SetRespawnAnimation(Transform player)
    {
        if (player != null)
        {
            Animator anim = player.GetChild(0).GetComponent<Animator>();
            anim.SetBool("IsRespawn", true);
        }

        int posid = 0;
        List<PlayerController_DangerRun> controllerList = new List<PlayerController_DangerRun>();
        for (int i = 0; i < m_respownList.Count; i++)
        {
            Animator ani = m_respownList[i].m_Player.GetChild(0).GetComponent<Animator>();
            PlayerController_DangerRun controller = m_respownList[i].m_Player.GetComponent<PlayerController_DangerRun>();
            if (ani.GetBool("IsRespawn"))
            {
                controller.m_AnimationFloatParamLerp_RespawnSwitch.SetTarget(posid);
                posid++;
            }
            controllerList.Add(controller);
        }

        foreach (PlayerController_DangerRun controller in controllerList)
        {
            controller.m_AnimationFloatParamLerp_PlayersCount.SetTarget(posid);
        }
    }
}
