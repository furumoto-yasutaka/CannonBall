using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer_Screen : Timer
{
    /// <summary> �^�C�}�[�̐F��ς���臒l </summary>
    [SerializeField, CustomLabel("�^�C�}�[�̐F��ς���臒l")]
    private int m_timerColorChangeThreshold = 60;

    /// <summary> �w�i�̃}�e���A��1 </summary>
    [SerializeField, CustomLabel("�w�i�̃}�e���A��1")]
    private Material m_backgroundMaterial_1;

    /// <summary> �w�i�̃}�e���A��2 </summary>
    [SerializeField, CustomLabel("�w�i�̃}�e���A��2")]
    private Material m_backgroundMaterial_2;

    /// <summary> �w�i�̃}�e���A��3 </summary>
    [SerializeField, CustomLabel("�w�i�̃}�e���A��3")]
    private Material m_backgroundMaterial_3;

    /// <summary> �w�i�̐F��A�������邩 </summary>
    [SerializeField, CustomLabel("�w�i�̐F��A�������邩")]
    public bool m_isBackgroundEmissionColorControl = true;

    /// <summary> �w�i�̃G�~�b�V�����J���[ </summary>
    [SerializeField, CustomLabel("�w�i�̃G�~�b�V�����J���[")]
    public Vector3 m_backgroundEmissionColor;

    /// <summary> �Q�[���Z�b�g�摜 </summary>
    [SerializeField, CustomLabel("�Q�[���Z�b�g�摜")]
    public GameObject m_gamesetImage;

    /// <summary> �����\�� </summary>
    [SerializeField, CustomLabel("�����\��")]
    public GameObject m_aliveZone;

    private Animator m_animator;


    protected override void Awake()
    {
        base.Awake();

        m_animator = GetComponent<Animator>();

        SetMaterialColor();
    }

    protected override void Update()
    {
        base.Update();

        if (!m_isStopTimer)
        {
            m_animator.SetBool("IsNoTime", m_timeCounter <= m_timerColorChangeThreshold);

            SetMaterialColor();
        }
    }

    private void SetMaterialColor()
    {
        if (!m_isBackgroundEmissionColorControl) { return; }

        Color c;
        c.r = m_backgroundEmissionColor.x;
        c.g = m_backgroundEmissionColor.y;
        c.b = m_backgroundEmissionColor.z;
        c.a = 0.0f;
        m_backgroundMaterial_1.SetColor("_EmissiveColor", c);
        m_backgroundMaterial_2.SetColor("_EmissiveColor", c);
        m_backgroundMaterial_3.SetColor("_EmissiveColor", c);
    }

    /// <summary> ���_�X�v���C�g�ݒ� </summary>
    /// <param name="value"> �ݒ肷��l </param>
    /// <param name="parent"> ���l�\���̐e�I�u�W�F�N�g </param>
    protected override void SetNumberSprite(int value, Transform[] parent)
    {
        for (int i = 0; i < parent.Length; i++)
        {
            int temp = value;
            int j = 0;
            do
            {
                int d = temp % 10;
                parent[i].GetChild(j).GetComponent<MeshRenderer>().material.SetTexture("_MainTex", m_numberSprites[d].texture);
                temp /= 10;
                j++;
            }
            while (j < parent[i].childCount && temp != 0);

            for (; j < parent[i].childCount; j++)
            {
                parent[i].GetChild(j).GetComponent<MeshRenderer>().material.SetTexture("_MainTex", m_numberSprites[0].texture);
            }
        }
    }

    protected override void TimerEndCallback()
    {
        Time.timeScale = 0.1f;
        m_gamesetImage.SetActive(true);
        m_aliveZone.GetComponent<AliveZone_CannonFight>().Stop();
    }
}
