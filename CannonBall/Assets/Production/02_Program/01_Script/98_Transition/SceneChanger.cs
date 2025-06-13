/*******************************************************************************
*
*	�^�C�g���F	�V�[���J�ڏ����X�N���v�g
*	�t�@�C���F	SceneChanger.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/04/25
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    /// <summary> �g�p����g�����W�V�����p�v���n�u </summary>
    [SerializeField, CustomLabel("�g�p����g�����W�V�����v���n�u")]
    private GameObject m_transitionPrefab = null;

    /// <summary> �J�ڐ�V�[�� </summary>
    [SerializeField, CustomLabel("�J�ڐ�V�[��")]
    private SceneNameEnum m_nextScene;

    /// <summary> ��ݼ޼�������ނ���޽�̎q�ɂ��邩 </summary>
    [SerializeField, CustomLabel("��ݼ޼�������ނ���޽�̎q�ɂ��邩")]
    private bool m_isCanvasChildren = true;

    [Header("(�����ɉe���͂Ȃ��J�ڐ�V�[���p�����[�^���g���̂��������邽��)")]
    /// <summary> �V�[���𒼐ڎw�肷�邩 </summary>
    [SerializeField, CustomLabel("�V�[���𒼐ڎw�肷�邩")]
    private bool m_isSceneDirect = false;

    /// <summary> �V�[���J�ڂ̍ۍŒች�b�҂� </summary>
    [SerializeField, CustomLabel("�V�[���J�ڂ̍ۍŒች�b�҂�")]
    private float m_transitionWaitTime = 1.0f;

    private string m_nextSceneName = "";

    private AsyncOperation m_asyncOperation;

    private float m_startTime = 0.0f;


    /// <summary>
    /// �J�ڊJ�n����(�C���X�^���X�ɐݒ肳��Ă���V�[���ɑJ��)
    /// </summary>
    public void StartSceneChange()
    {
        m_nextSceneName = m_nextScene.ToString();

        SetSceneChange();
    }

    /// <summary>
    /// �J�ڊJ�n����(�V�[���𒼐ڎw��)
    /// </summary>
    /// <param name="scene"> �J�ڐ�V�[�� </param>
    public void StartSceneChange(SceneNameEnum scene)
    {
        if (!m_isSceneDirect)
        {
            Debug.LogError("�V�[���𒼐ڎw�肵�Ȃ��ݒ�Ȃ̂ɌĂ΂�܂���");
        }

        m_nextScene = scene;
        StartSceneChange();
    }

    /// <summary>
    /// �J�ڊJ�n����(������w�� ���f�o�b�O�p)
    /// </summary>
    public void StartSceneChange(string scenename)
    {
        m_nextSceneName = scenename;

        SetSceneChange();
    }

    /// <summary>
    /// �J�ڊJ�n����(���̃V�[���ɍđJ��)
    /// </summary>
    public void StartThisSceneChange()
    {
        m_nextSceneName = SceneManager.GetActiveScene().name;

        SetSceneChange();
    }

    /// <summary>
    /// �J�ڗ\�񏈗�
    /// </summary>
    private void SetSceneChange()
    {
        // ���ׂĂ�GUI���͂��Ւf����
        if (InputGroupManager.CheckInstance())
        {
            InputGroupManager.Instance.LockInputGroupOrder(false);
        }

        // �g�����W�V�����I����̃R�[���o�b�N�ɑJ�ڏ�����ݒ�
        TransitionCallBack.SetTransitionCallBack(ChangeScene);

        m_asyncOperation = SceneManager.LoadSceneAsync(m_nextSceneName, LoadSceneMode.Single);
        m_asyncOperation.allowSceneActivation = false;
        m_startTime = Time.realtimeSinceStartup;
        StartCoroutine("ChangeSceneWait");
    }

    /// <summary>
    /// �J�ڏ���
    /// </summary>
    public void ChangeScene()
    {
        Time.timeScale = 1.0f;
        m_asyncOperation.allowSceneActivation = true;
    }

    IEnumerator ChangeSceneWait()
    {
        while (Time.realtimeSinceStartup - m_startTime < m_transitionWaitTime)
        {
            yield return null;
        }

        // �g�����W�V�����I�u�W�F�N�g�L�����o�X�̎q�Ƃ��Đ������邩
        if (m_transitionPrefab != null)
        {
            if (m_isCanvasChildren)
            {
                // �������Đe�q�֌W��ݒ肷��
                Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                GameObject parent;

                if (canvas == null)
                {
#if UNITY_EDITOR
                    Debug.LogError("Canvas�����݂��Ă��܂���");
#endif
                }
                else
                {
                    parent = canvas.gameObject;
                    Instantiate(m_transitionPrefab, parent.transform);
                }
            }
            else
            {
                Instantiate(m_transitionPrefab);
            }
        }
    }
}
