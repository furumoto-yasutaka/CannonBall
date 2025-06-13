using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFollow : MonoBehaviour
{
    [SerializeField]
    float m_angularVelocity = 0.5f;

    List<Rigidbody2D> m_rbList = new List<Rigidbody2D>();

    Rigidbody2D m_rb;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        Debug.Log("m_rbList.Count" + m_rbList.Count);
        for (int i = 0; i < m_rbList.Count; i++)
        {
            //Debug.Log("m_rbList[i].position" + m_rbList[i].position);
            //Debug.Log(".velocity" + m_rb.velocity);
            //m_rbList[i].position += m_rb.velocity;
            //m_rbList[i].angularDrag = 3.0f;
            m_rbList[i].angularVelocity *= m_angularVelocity;
        }   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            m_rbList.Add(rb);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            m_rbList.Remove(rb);
        }
    }
}
