using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAudienceSwitch : MonoBehaviour
{
    [SerializeField, CustomLabel("�A�j���[�V�����̒x�����x��")]
    private int m_delayLevel = 0;


    private void Start()
    {
        GetComponent<Animator>().SetInteger("Switch", m_delayLevel);
    }
}
