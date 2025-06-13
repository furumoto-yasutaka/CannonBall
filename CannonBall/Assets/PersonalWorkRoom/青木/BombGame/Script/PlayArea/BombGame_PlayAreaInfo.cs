using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class BombGame_PlayAreaInfo : MonoBehaviour
{
    public enum State
    {
        None,
        InBomb,
        InExplosion,
        Death,
    }


    #region �t�B�[���h


    [SerializeField]
    private MeshRenderer m_meshRenderer;

    [SerializeField]
    private MeshRenderer[] m_meshRenderers;

    [SerializeField, CustomArrayLabel(new string[] { "DefaultMaterial", "LightMaterial"})]
    private Material[] m_materials;

    [SerializeField]
    private SpriteRenderer m_spriteRenderer;

    [SerializeField, CustomLabel("�t�B�[���h���b�V��")]
    private MeshRenderer m_fieldRenderer;

    [SerializeField]
    private ParticleSystem m_smallParticle;

    [SerializeField]
    private ParticleSystem m_bigGlassParticle;

    [SerializeField]
    private ParticleSystem[] m_deathSparks;

    [SerializeField, CustomLabel("�_���[�W�}�e���A��")]
    Material[] m_damageMaterials;

    [SerializeField, CustomLabel("�_���[�W�X�v���C�g")]
    Sprite[] m_damageSprite;

    [SerializeField, CustomLabel("�����L���O�X�v���C�g")]
    Sprite m_rankingSprite;

    private State m_state = State.None;

    private float m_inExplosionTime = 2.0f;

    private float m_inExplosionTimeCount = 0.0f;



    //Animator m_animator;

    #endregion




    #region �v���p�e�B

    //// �X�e�[�W��HP�������Ȃ�����true
    //public bool m_IsDeath { get; set; }

    public State m_State { get { return m_state; } }

    public float m_Health { get; set; }


    public MeshRenderer GetMeshRenderer() { return m_meshRenderer; }


    #endregion



    #region �֐�


    private void Awake()
    {
        m_Health = m_damageSprite.Length;
    }


    private void Start()
    {
        //m_animator = GetComponent<Animator>();
    }

    public void ChangeState(State next)
    {
        switch (next)
        {
            case State.None:
                ChangeMeshMaterial(BombGame_InBombReaction.Instance.m_DefaultMaterial);
                break;
            case State.InBomb:
                ChangeMeshMaterial(BombGame_InBombReaction.Instance.m_InMaterial);
                break;
            case State.InExplosion:
                ChangeMeshMaterial(BombGame_InBombReaction.Instance.m_InExplosionMaterial);
                m_inExplosionTimeCount = m_inExplosionTime;
                break;
            case State.Death:
                ChangeMeshMaterial(BombGame_InBombReaction.Instance.m_DeathMaterial);
                break;
        }

        m_state = next;
    }

    public void UpdateInExplosionTimer()
    {
        if (m_inExplosionTimeCount < 0.0f)
        {
            m_inExplosionTimeCount = 0.0f;
            ChangeState(State.None);
        }
        else
        {
            m_inExplosionTimeCount -= Time.deltaTime;
        }
    }

    public void AddStageHealth(float _health)
    {
        // ����ł����ꍇ�A�S�ăX�L�b�v
        if (m_state == State.Death) { return; }

        int preHP = (int)m_Health;

        m_Health += _health;

        int hp = (int)m_Health;

        // �X�v���C�g�̒i�K��������ꍇ
        if (preHP != hp)
        {
            // �X�v���C�g�̕ύX
            m_fieldRenderer.material = m_damageMaterials[(int)Mathf.Clamp(m_damageSprite.Length - m_Health, 0.0f, m_damageSprite.Length - 1)];
            //m_spriteRenderer.sprite = m_damageSprite[(int)Mathf.Clamp(m_damageSprite.Length - m_Health, 0.0f, m_damageSprite.Length - 1)];

        }
    }


    public void SubStageHealth(float _damage)
    {
        // ����ł����ꍇ�A�S�ăX�L�b�v
        if (m_state == State.Death) { return; }

        int preHP = (int)m_Health;

        m_Health -= _damage;

        int hp = (int)m_Health;

        // �X�v���C�g�̕ύX������ꍇ
        if (m_Health <= 0.0f)
        {
            ChangeState(State.Death);

            //  �����L���O�\��
            m_spriteRenderer.gameObject.SetActive(true);
            m_spriteRenderer.sprite = m_rankingSprite;
            m_fieldRenderer.gameObject.SetActive(false);

            // �T�E���h
            AudioManager.Instance.PlaySe("�K���X�Ђъ���_�E��", false);

            // �G�t�F�N�g
            m_bigGlassParticle.Play();
            foreach (var item in m_deathSparks)
            {
                item.Play();
            }
        }
        else
        {
            ChangeState(State.InExplosion);

            // �X�v���C�g�̒i�K������ꍇ
            if (preHP != hp)
            {
                // �X�v���C�g�̕ύX
                m_fieldRenderer.material = m_damageMaterials[(int)Mathf.Clamp(m_damageSprite.Length - m_Health, 0.0f, m_damageSprite.Length - 1)];
                //m_spriteRenderer.sprite = m_damageSprite[(int)Mathf.Clamp(m_damageSprite.Length - m_Health, 0.0f, m_damageSprite.Length - 1)];

                // �G�t�F�N�g
                m_bigGlassParticle.Play();

                // �T�E���h
                AudioManager.Instance.PlaySe("�K���X�Ђъ���_��_���[�W", false);
            }
            else
            {
                // �G�t�F�N�g
                m_smallParticle.Play();

                // �T�E���h
                AudioManager.Instance.PlaySe("�K���X�Ђъ���_���_���[�W", false);
            }

            //m_animator.SetTrigger("Damage");
        }
    }


    private void ChangeMeshMaterial(Material _material)
    {
        foreach (var item in m_meshRenderers)
        {
            item.material = _material;
        }
    }

    private void ChangeSprite(Sprite _sprite)
    {
        m_spriteRenderer.sprite = _sprite;
    }



    // �A�j���[�V�����Ŏg�p
    private void DefaultMaterial()
    {
        foreach (var item in m_meshRenderers)
        {
            item.material = m_materials[0];
        }
    }

    private void LightMaterial()
    {
        foreach (var item in m_meshRenderers)
        {
            item.material = m_materials[1];
        }
    }




    #endregion


}
