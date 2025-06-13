using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerBumper : MonoBehaviour
{
    [SerializeField]
    protected float m_power;

    [SerializeField]
    protected Vector2 m_ImpactVec;

    /// <summary> �o���p�[�ڐG���̃G�t�F�N�g�̐e�I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("�o���p�[�ڐG���̃G�t�F�N�g�̐e�I�u�W�F�N�g")]
    protected Transform m_contactEffParent;

    protected Animator m_animator;


    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bomb"))
        {
            collision.transform.root.GetComponent<Rigidbody2D>().velocity = m_power * m_ImpactVec;

            m_animator.SetTrigger("IsBounce");
            CreateContactEffect();
            PlayContactSound();
        }
        else if (collision.collider.CompareTag("PlayerBody"))
        {
            Transform root = collision.transform.root;
            if (!root.GetComponent<PlayerController>().m_BumperIgnore)
            {
                root.GetComponentInParent<Rigidbody2D>().velocity = m_power * m_ImpactVec;

                m_animator.SetTrigger("IsBounce");
                CreateContactEffect();
                PlayContactSound();
                SetVibration(root.GetComponent<PlayerId>().m_Id);
            }
        }
    }

    protected void CreateContactEffect()
    {
        EffectContainer.Instance.EffectPlay(
            "�o���p�[�ڐG",
            transform.GetChild(0).GetChild(0).position,
            default,
            m_contactEffParent,
            transform.localScale);
    }

    protected void PlayContactSound()
    {
        AudioManager.Instance.PlaySe("�o���p�[", false);
    }

    protected void SetVibration(int id)
    {
        VibrationManager.Instance.SetVibration(id, 8, 0.3f);
    }
}
