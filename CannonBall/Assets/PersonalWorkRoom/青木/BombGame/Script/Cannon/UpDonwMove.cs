using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDonwMove : MonoBehaviour
{
    [SerializeField]
    float m_width;


    [SerializeField]
    Vector2 m_moveSpeed;

    Vector3 m_defaultTransform;


    private void Start()
    {
        m_defaultTransform = transform.position;
    }


    private void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x + m_moveSpeed.x, transform.position.y + m_moveSpeed.y);

        Vector3 vec = m_defaultTransform - transform.position;
        if (vec.magnitude > m_width)
        {
           m_moveSpeed *= -1;

           transform.position = new Vector2(transform.position.x + m_moveSpeed.x, transform.position.y + m_moveSpeed.y);
        }
    }

}
