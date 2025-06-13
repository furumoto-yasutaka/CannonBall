using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundGravity : MonoBehaviour
{
    [SerializeField]
    float m_gravityScale = 1.0f;

    Rigidbody2D m_rb;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        Vector2 gravity =  transform.position;
        gravity = gravity.normalized * m_gravityScale;

        m_rb.velocity += gravity;

    }


}
