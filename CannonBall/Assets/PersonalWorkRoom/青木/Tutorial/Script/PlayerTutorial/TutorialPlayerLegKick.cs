using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayerLegKick : MonoBehaviour
{
    //bool m_isKickHitPlatform = false;

    public bool m_IsKickHitPlatform { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("�Ђ���");
        //m_isKickHitPlatform = true;
    }

}
