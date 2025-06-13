using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGame_InBombReaction : SingletonMonoBehaviour<BombGame_InBombReaction>
{
    [SerializeField, CustomLabel("�f�t�H���g�̃}�e���A��")]
    Material m_defaultMaterial;

    [SerializeField, CustomLabel("�G���A���ɑ��݂��Ă��鎞�̃}�e���A��")]
    Material m_inMaterial;

    [SerializeField, CustomLabel("�_���[�W���󂯂��Ƃ��̃}�e���A��")]
    Material m_inExplosionMaterial;

    [SerializeField, CustomLabel("���S���̃}�e���A��")]
    Material m_deathMaterial;

    BombGame_PlayAreaInfo[] m_playAreaInfo = new BombGame_PlayAreaInfo[BombGame_PlayAreaData.GetMaxAreaNumber()];


    public Material m_DefaultMaterial { get { return m_defaultMaterial; } }

    public Material m_InMaterial { get { return m_inMaterial; } }

    public Material m_InExplosionMaterial { get { return m_inExplosionMaterial; } }

    public Material m_DeathMaterial { get { return m_deathMaterial; } }


    protected override void Awake()
    {
        // �V�[�����ׂ��ő��݂��闝�R�Ȃ�
        dontDestroyOnLoad = false;

        // �p�����̌Ăяo��
        base.Awake();
    }

    private void Start()
    {
        for (int i = 0; i < m_playAreaInfo.Length; i++)
        {
            m_playAreaInfo[i] = BombGame_PlayAreaData.Instance.GetStageObject()[i].GetComponent<BombGame_PlayAreaInfo>();
        }
    }

    private void Update()
    {
        List<int> inAreaNumbers = BombGame_PlayAreaData.Instance.GetInAreaNumbers();

        // ���b�V���̐F��ύX����
        // �G���A�S�ĕύX����
        for (int i = 0; i < BombGame_PlayAreaData.GetMaxAreaNumber(); i++)
        {
            bool b;
            switch (m_playAreaInfo[i].m_State)
            {
                case BombGame_PlayAreaInfo.State.None:
                    // ���e���G���A�ɂ��邩�`�F�b�N����
                    if (inAreaNumbers.Exists(x => x == i))
                    {
                        m_playAreaInfo[i].ChangeState(BombGame_PlayAreaInfo.State.InBomb);
                    }
                    break;
                case BombGame_PlayAreaInfo.State.InBomb:
                    // ���e���G���A�ɂ��邩�`�F�b�N����
                    if (!inAreaNumbers.Exists(x => x == i))
                    {
                        m_playAreaInfo[i].ChangeState(BombGame_PlayAreaInfo.State.None);
                    }
                    break;
                case BombGame_PlayAreaInfo.State.InExplosion:
                    // �^�C�}�[�̍X�V������
                    m_playAreaInfo[i].UpdateInExplosionTimer();
                    break;
                case BombGame_PlayAreaInfo.State.Death:
                    break;
            }
        }
    }
}
