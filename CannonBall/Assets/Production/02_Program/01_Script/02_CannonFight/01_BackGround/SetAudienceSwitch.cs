using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAudienceSwitch : MonoBehaviour
{
    [SerializeField, CustomLabel("アニメーションの遅延レベル")]
    private int m_delayLevel = 0;


    private void Start()
    {
        GetComponent<Animator>().SetInteger("Switch", m_delayLevel);
    }
}
