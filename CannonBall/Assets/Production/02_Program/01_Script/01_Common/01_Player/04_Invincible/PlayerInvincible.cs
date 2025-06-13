using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvincible : MonoBehaviour
{
    /// <summary> マテリアル取得コンポーネント </summary>
    [SerializeField, CustomLabel("マテリアル取得コンポーネント")]
    private ModelMaterialGetter m_materialGetter;

    /// <summary> 不透明度 </summary>
    [HideInInspector]
    public float m_transparency = 0.0f;

    /// <summary> アニメーターコンポーネント </summary>
    private Animator m_animator;

    /// <summary> 無敵の残り時間 </summary>
    private float m_invincibleTimeCount = 0.0f;

    /// <summary> 無敵かどうか </summary>
    private bool m_isInvincible = false;

    /// <summary> マテリアルの初期透明度 </summary>
    private List<float> m_defaultTransparency = new List<float>();


    public bool m_IsInvincible { get { return m_isInvincible; } }


    private void Start()
    {
        m_animator = GetComponent<Animator>();

        foreach (Material mat in m_materialGetter.m_Materials)
        {
            m_defaultTransparency.Add(mat.color.a);
        }
    }

    private void Update()
    {
        if (m_isInvincible)
        {
            m_invincibleTimeCount -= Time.deltaTime;

            if (m_invincibleTimeCount <= 0.0f)
            {
                m_invincibleTimeCount = 0.0f;
                m_isInvincible = false;
                m_transparency = 1.0f;
                m_animator.SetBool("IsInvincible", false);
            }

            for (int i = 0; i < m_materialGetter.m_Materials.Count; i++)
            {
                Material mat = m_materialGetter.m_Materials[i];
                Color c = mat.color;
                c.a = m_transparency * m_defaultTransparency[i];
                mat.color = c;
            }
        }
    }

    /// <summary> 無敵状態を設定する </summary>
    /// <param name="time"> 無敵時間 </param>
    public void SetInvincible(float time)
    {
        m_isInvincible = true;
        if (m_invincibleTimeCount < time)
        {
            m_invincibleTimeCount = time;
        }
        m_animator.SetBool("IsInvincible", true);
    }
}
