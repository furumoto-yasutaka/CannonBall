using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTutorial : MonoBehaviour
{
    [SerializeField]
    PlayerController controller;

    [SerializeField]
    TutorialPlayerLegKick m_tutorialPlayerLegKick;


    Image m_image;

    private void Start()
    {
        m_image = GetComponent<Image>();
    }


    private void Update()
    {
        //// �W�����v �`���[�g���A�� 
        
        //// ���v���Ɋp�x���o��
        //float angle = Vector3.SignedAngle(Vector3.down, controller.m_KickDir, Vector3.forward);

        //if (controller.m_IsKicking)
        //{
        //    Debug.Log("angle" + angle);
        //}

        //Debug.Log("controller.m_IsKicking" + controller.m_IsKicking);
        //Debug.Log("Mathf.Abs(angle) > 30.0f" + (Mathf.Abs(angle) > 30.0f));

        //if (m_tutorialPlayerLegKick.m_IsKickHitPlatform)
        //{
        //    Debug.Log("controller.m_IsKicking" + controller.m_IsKicking);
        //    Debug.Log("Mathf.Abs(angle) > 30.0f" + (Mathf.Abs(angle) > 30.0f));
        //}


        //if (Mathf.Abs(angle) <= 30.0f && controller.m_IsKicking && m_tutorialPlayerLegKick.m_IsKickHitPlatform)
        //{
        //    m_image.fillAmount += 0.333f;
        //    Debug.Log("��K");

        //    m_tutorialPlayerLegKick.m_IsKickHitPlatform = false;
        //}
    }


}
