using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialKickTarget_TargetFollow : MonoBehaviour
{
    [SerializeField]
    Transform m_followTarget;

    Camera m_camera;
    RectTransform m_rect;
    Vector2 m_followTargetScreenSpace;
    
    Vector2 m_distance;

    private void Start()
    {
        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        m_rect = GetComponent<RectTransform>();

        // Transform�i���[���h�j���W���X�N���[�����W�ɕϊ�����
        m_followTargetScreenSpace = RectTransformUtility.WorldToScreenPoint(m_camera, m_followTarget.transform.position);

        // �X�N���[�����W��RectTransform���W�ɕϊ�����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rect, m_followTargetScreenSpace, m_camera, out m_distance);
    }

    

    private void Update()
    {
        // Transform�i���[���h�j���W���X�N���[�����W�ɕϊ�����
        m_followTargetScreenSpace = RectTransformUtility.WorldToScreenPoint(m_camera, m_followTarget.transform.position);

        // �X�N���[�����W��RectTransform���W�ɕϊ�����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rect, m_followTargetScreenSpace, m_camera, out m_distance);

        Debug.Log("m_followTargetScreenSpace" + m_followTargetScreenSpace);

        Debug.Log("m_distance" + m_distance);

        transform.localPosition = m_distance;
    }

}
