using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetController_DangerRun : SingletonMonoBehaviour<GameSetController_DangerRun>
{
    /// <summary> �J�����ړ��R���|�[�l���g </summary>
    [SerializeField, CustomLabel("�J�����ړ��R���|�[�l���g")]
    private DangerRun_CameraMove m_cameraMove;

    private Animator m_animator;

    private bool m_isFinish = false;

    public bool m_IsFinish { get { return m_isFinish; } }


    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();

        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!m_animator.enabled)
        {
            if (m_cameraMove.m_IsFinish)
            {
                m_animator.enabled = true;
            }
        }
    }

    public void Finish()
    {
        m_isFinish = true;
    }

    public void PlayGameSetSe()
    {
        AudioManager.Instance.StopBgmAll();
        AudioManager.Instance.PlaySe(
            "�����I��_�Q�[���Z�b�g",
            false);
    }
}
