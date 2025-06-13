using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarController3d : MonoBehaviour
{
    [SerializeField] GameObject m_3bar;

    private BoxCollider2D m_3barbox2d;

    private Animator m_animator;

    /// <summary> 基準の回転速度 </summary>
    [SerializeField, CustomLabel("基準の回転速度")]
    private float m_barspeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_3barbox2d = m_3bar.GetComponent<BoxCollider2D>();

        m_animator = GetComponent<Animator>();
        m_animator.SetFloat("speed", m_barspeed);
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary> BarのコリジョンON </summary>
    public void BarHitboxOn3d()
    {
        m_3barbox2d.enabled = true;
    }

    /// <summary> BarのコリジョンOFF </summary>
    public void BarHitboxOff3d()
    {
        m_3barbox2d.enabled = false;
    }

   
}
