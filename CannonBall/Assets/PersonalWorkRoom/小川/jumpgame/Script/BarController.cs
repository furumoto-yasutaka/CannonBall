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

    /// <summary> 基準の回転速度 </summary>
    [SerializeField, CustomLabel("基準の回転速度")]
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

    /// <summary> BarのコリジョンON </summary>
    public void BarHitboxOn()
    {
        m_barbox2d.enabled = true;
        m_barRenderer.color = Color.blue;
    }

    /// <summary> BarのコリジョンOFF </summary>
    public void BarHitboxOff()
    {
        m_barbox2d.enabled = false;
        m_barRenderer.color = Color.red;
    }

   
}
