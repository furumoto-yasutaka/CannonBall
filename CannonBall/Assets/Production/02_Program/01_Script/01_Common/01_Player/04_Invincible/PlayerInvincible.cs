using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvincible : MonoBehaviour
{
    /// <summary> �}�e���A���擾�R���|�[�l���g </summary>
    [SerializeField, CustomLabel("�}�e���A���擾�R���|�[�l���g")]
    private ModelMaterialGetter m_materialGetter;

    /// <summary> �s�����x </summary>
    [HideInInspector]
    public float m_transparency = 0.0f;

    /// <summary> �A�j���[�^�[�R���|�[�l���g </summary>
    private Animator m_animator;

    /// <summary> ���G�̎c�莞�� </summary>
    private float m_invincibleTimeCount = 0.0f;

    /// <summary> ���G���ǂ��� </summary>
    private bool m_isInvincible = false;

    /// <summary> �}�e���A���̏��������x </summary>
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

    /// <summary> ���G��Ԃ�ݒ肷�� </summary>
    /// <param name="time"> ���G���� </param>
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
