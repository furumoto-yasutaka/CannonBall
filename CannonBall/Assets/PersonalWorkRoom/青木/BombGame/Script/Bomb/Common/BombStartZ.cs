using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombStartZ : MonoBehaviour
{

    [SerializeField]
    float m_endPosZ = 0.0f;

    float m_defaultPosZ;

    float m_speed = 1.3f;

    float m_T = 0.0f;


    private void Start()
    {
        m_defaultPosZ = transform.position.z;
    }



    private void Update()
    {
        if (m_T >= 1.0f)
        {
            return;
        }
        m_T += m_speed * Time.deltaTime;
        
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(m_defaultPosZ, m_endPosZ, m_T));
    }
}
