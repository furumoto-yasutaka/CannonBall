using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone_DangerRun : DeadZone
{
    public enum SinkDir
    { 
        Top = 0,
        Bottom,
        Left,
        Right,
    }


    [SerializeField, CustomLabel("���ޕ���")]
    private SinkDir m_sinkDir = SinkDir.Bottom;

    private static readonly Vector3[] m_sinkDirVec = new Vector3[4]
    {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right,
    };

    private Animator[] m_animators = new Animator[4];


    private void Start()
    {
        for (int i = 0; i < m_animators.Length; i++)
        {
            m_animators[i] = transform.GetChild(0).GetChild(i).GetComponent<Animator>();
            m_animators[i].SetInteger("DirSwitch", (int)m_sinkDir);
        }
    }

    /// <summary> �v���C���[���G�ꂽ�ۂ̃C�x���g </summary>
    /// <param name="collision"> �v���C���[�̃R���W���� </param>
    protected override void OnTriggerEnter_Player(Collider2D collision)
    {
        OnTriggerExit_Player(collision);
    }

    protected override void OnTriggerExit_Player(Collider2D collision)
    {
        Transform trans = collision.transform.root;

        trans.GetComponent<PlayerDeathCount>().AddCount();

        // ���݃A�j���[�V�����p�̐e�I�u�W�F�N�g��T��
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            if (transform.GetChild(0).GetChild(i).childCount == 0)
            {
                if (RespawnManager.CheckInstance())
                {
                    EffectContainer.Instance.EffectPlay(
                        "�f���W������_��i�ɒ��񂾏u��",
                        trans.position,
                        Quaternion.identity * Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.down, m_sinkDirVec[(int)m_sinkDir]), Vector3.forward),
                        transform.GetChild(1));
                    EffectContainer.Instance.EffectPlay(
                        "�f���W������_��i�ɒ���ł���Ƃ��̉�",
                        trans.position,
                        Quaternion.identity * Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.down, m_sinkDirVec[(int)m_sinkDir]), Vector3.forward),
                        transform.GetChild(1));
                    AudioManager.Instance.PlaySe("�f���W������_���މ�", false);
                    // �R���g���[���[�U��
                    VibrationManager.Instance.SetVibration(trans.GetComponent<PlayerId>().m_Id, 45, 0.8f);

                    // ���S����
                    trans.GetComponent<PlayerController>().Death(RespawnManager.Instance.m_RevivalInterval);
                    // ��U�v���C���[�̐e�ɐݒ�
                    trans.SetParent(transform.GetChild(0).GetChild(i));
                    // ���݃A�j���[�V�������J�n
                    m_animators[i].SetTrigger("Sink");
                }

                break;
            }
        }
    }
}
