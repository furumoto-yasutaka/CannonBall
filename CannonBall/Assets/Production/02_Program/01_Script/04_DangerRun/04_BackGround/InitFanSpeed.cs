using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitFanSpeed : MonoBehaviour
{
    [SerializeField, CustomLabel("ƒtƒ@ƒ“‚Ì‰ñ“]‘¬“x")]
    private float m_fanSpeed = 1.0f;


    void Start()
    {
        GetComponent<Animator>().SetFloat("RunSpeed", m_fanSpeed);
    }
}
