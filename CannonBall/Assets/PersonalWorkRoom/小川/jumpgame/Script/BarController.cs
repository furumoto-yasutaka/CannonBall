using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarController : MonoBehaviour
{
    [SerializeField] GameObject m_bar;

    private BoxCollider2D m_barbox2d;

    private SpriteRenderer m_barRenderer;

    private Animator m_animator;

    /// <summary> ��̉�]���x </summary>
    [SerializeField, CustomLabel("��̉�]���x")]
    private float m_barspeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_barbox2d = m_bar.GetComponent<BoxCollider2D>();

        m_barRenderer = m_bar.GetComponent<SpriteRenderer>();

        m_animator = GetComponent<Animator>();
        m_animator.SetFloat("speed", m_barspeed);
    }

    // Update is called once per frame
    void Update()
    {
        m_bar.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    /// <summary> Bar�̃R���W����ON </summary>
    public void BarHitboxOn()
    {
        m_barbox2d.enabled = true;
        m_barRenderer.color = Color.blue;
    }

    /// <summary> Bar�̃R���W����OFF </summary>
    public void BarHitboxOff()
    {
        m_barbox2d.enabled = false;
        m_barRenderer.color = Color.red;
    }

   
}
