using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTruckCallback : MonoBehaviour
{
    public void StartJumpOutCallback()
    {
        transform.root.GetComponent<RespawnTruck>().StartJumpOut();
    }
}
