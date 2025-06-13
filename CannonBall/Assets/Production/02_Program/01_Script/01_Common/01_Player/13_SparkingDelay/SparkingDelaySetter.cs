using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkingDelaySetter : MonoBehaviour
{
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<MeshRenderer>().material.SetFloat("_SparkingDelay", i * 100 + Random.Range(0.0f, 1.0f));
        }
    }
}
