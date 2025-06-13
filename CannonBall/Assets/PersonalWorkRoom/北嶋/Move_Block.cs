using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Block : MonoBehaviour
{
    public float lengthX = 0.0f;
    public float length = 1.0f;
    public float speed = 1.0f;
    public float delay = 0.0f;

    private Rigidbody2D rb;
    private Vector3   Pos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Pos = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 v = Pos;
        if (lengthX > 0.0f)
        {
            v.x += Mathf.PingPong(Time.time * speed + delay, lengthX);
        }
        if (length > 0.0f)
        {
            v.y += Mathf.PingPong(Time.time * speed + delay, length);
        }
        
        rb.MovePosition(v);
    }
}
