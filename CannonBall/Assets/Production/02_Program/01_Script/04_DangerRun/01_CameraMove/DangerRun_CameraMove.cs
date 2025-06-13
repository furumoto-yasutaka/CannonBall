/*******************************************************************************
*
*	�^�C�g���F	�����X�N���[���̃J�����ړ��X�N���v�g
*	�t�@�C���F	DangerRun_CameraMove.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/06
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DangerRun_CameraMove : MonoBehaviour
{
    /// <summary> ��ԏ��̐e�I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("��ԏ��̐e�I�u�W�F�N�g")]
    private Transform m_sectionParent;

    /// <summary> �N���s�ǂ̐e�I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("�N���s�ǂ̐e�I�u�W�F�N�g")]
    private Transform m_wallParent;

    /// <summary> �f�b�h�]�[���̐e�I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("�f�b�h�]�[���̐e�I�u�W�F�N�g")]
    private Transform m_deadzoneParent;

    /// <summary> ���݂̋�Ԃ̈ړ����J�n���Čo�������� </summary>
    [SerializeField, CustomLabelReadOnly("����")]
    private float m_sectionTime = 0.0f;

    /// <summary> ��ԃ|�C���g�ԍ�(0~) </summary>
    private int m_sectionNumber = 0;

    /// <summary> ��ԏ�� </summary>
    private DangerRun_CameraMoveSource m_sectionSource;

    private bool m_isMove = false;

    /// <summary> �S�Ă̋�Ԃ�ʂ�I�������ǂ��� </summary>
    private bool m_isFinish = false;

    public bool m_IsFinish { get { return m_isFinish; } }


    private void Start()
    {
        // �e��Ԃ̏�����
        InitSections();
    }

    private void Update()
    {
        if (m_isFinish || !m_isMove) { return; }

        m_sectionTime += Time.deltaTime;

        float overtime = 0.0f;
        bool isSectionFinish = false;
        // ���W�擾
        Vector3 pos = m_sectionSource.GetPosition(m_sectionTime, ref isSectionFinish, ref overtime);

        // ��Ԃ�ʂ�I���A���߂��������璴�ߕ��̍��W�ړ����s��
        while (isSectionFinish && !m_isFinish)
        {
            isSectionFinish = false;

            // ���̋�Ԃɐ؂�ւ��鏀��
            m_sectionNumber++;

            if (m_sectionNumber < m_sectionParent.childCount - 1)
            {// ���̋�Ԃɐ؂�ւ�
                m_sectionTime = overtime;
                overtime = 0.0f;
                // ���̋�ԏ����擾
                ChangeSection();
                // ���W�擾
                pos = m_sectionSource.GetPosition(m_sectionTime, ref isSectionFinish, ref overtime);
            }
            else
            {// �S�Ă̋�Ԃ�ʂ�I����
                m_isFinish = true;
            }
        }

        // �J�����̍��W�ɔ��f
        transform.position = pos;
    }

    public void StartMove()
    {
        m_isMove = true;
    }

    /// <summary> �e��Ԃ̏����� </summary>
    private void InitSections()
    {
        for (int i = 0; i < m_sectionParent.childCount - 1; i++)
        {
            m_sectionParent.GetChild(i).GetComponent<DangerRun_CameraMoveSource>().InitParam(i);
        }

        m_sectionSource = m_sectionParent.GetChild(m_sectionNumber).GetComponent<DangerRun_CameraMoveSource>();
        // ���̋�Ԃ̕ǂ̃y�A�����g���J������
        Transform temp = m_sectionSource.transform.GetChild(0);
        while (temp.childCount > 0)
        {
            temp.GetChild(0).parent = transform.GetChild(2);
        }
        // ���̋�Ԃ̎��S���L�������A�y�A�����g���J������
        temp = m_sectionSource.transform.GetChild(1);
        while (temp.childCount > 0)
        {
            temp.GetChild(0).gameObject.SetActive(true);
            temp.GetChild(0).parent = transform.GetChild(3);
        }
    }

    /// <summary> ��ԏ����擾 </summary>
    private void ChangeSection()
    {
        if (m_sectionSource.m_SectionNumber != m_sectionNumber)
        {
            DangerRun_CameraMoveSource next = m_sectionParent.GetChild(m_sectionNumber).GetComponent<DangerRun_CameraMoveSource>();

            // �O�̋�Ԃ̕ǂ��폜
            for (int i = 0; i < m_wallParent.childCount; i++)
            {
                Destroy(m_wallParent.GetChild(0).gameObject);
            }
            // �O�̋�Ԃ̎��S��̃y�A�����g�����ɖ߂�
            if (next.m_IsReplaceDeadZone)
            {
                for (int i = 0; i < m_deadzoneParent.childCount; i++)
                {
                    m_deadzoneParent.GetChild(0).parent = m_sectionSource.transform.GetChild(1);
                }
            }

            // ��ԏ��X�V
            m_sectionSource = next;
            m_sectionSource.InvokeCallback();

            // ���̋�Ԃ̕ǂ̃y�A�����g���J������
            Transform temp = m_sectionSource.transform.GetChild(0);
            for (int i = 0; i < temp.childCount; i++)
            {
                temp.GetChild(0).gameObject.SetActive(true);
                temp.GetChild(0).parent = m_wallParent;
            }
            // ���̋�Ԃ̎��S���L�������A�y�A�����g���J������
            temp = m_sectionSource.transform.GetChild(1);
            for (int i = 0; i < temp.childCount; i++)
            {
                temp.GetChild(0).gameObject.SetActive(true);
                temp.GetChild(0).parent = m_deadzoneParent;
            }
            if (m_sectionSource.m_IsSpeedEvent)
            {
                SpeedUpController.Instance.Play();
                AudioManager.Instance.PlaySe("�f���W������_�X�s�[�h�A�b�v", false);
            }
        }
    }
}
