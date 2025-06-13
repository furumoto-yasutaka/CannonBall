using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkAnimationCallback : MonoBehaviour
{
    public void SinkFinished()
    {
        Transform player = transform.GetChild(0);
        player.SetParent(null);
        player.GetComponent<PlayerController_DangerRun>().StopAndWarp();
        // RespawnManager‚É’Ç‰ÁˆË—Š‚·‚é
        RespawnManager.Instance.AddRespawnPlayer(player);
    }
}
