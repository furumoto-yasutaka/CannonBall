/*******************************************************************************
*
*	�^�C�g���F	�����X�N���[���̃J�����ړ��o�H�Q�Ɨp�X�N���v�g
*	�t�@�C���F	DangerRun_CameraMoveSource.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/06
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DangerRun_CameraMoveSource : MonoBehaviour
{
    /// <summary> �ړ����x(�b��) </summary>
    [SerializeField, CustomLabel("�ړ����x(�b��)")]
    protected float m_speed = 1.0f;

    /// <summary> �X�s�[�h�A�b�v�C�x���g�͂��邩 </summary>
    [SerializeField, CustomLabel("�X�s�[�h�A�b�v�C�x���g�͂��邩")]
    private bool m_isSpeedEvent = false;

    /// <summary> �Ǐ]���鎀�S������������邩 </summary>
    [SerializeField, CustomLabel("�Ǐ]���鎀�S������������邩")]
    private bool m_isReplaceDeadZone = true;

    /// <summary> �R�[���o�b�N�֐� </summary>
    [SerializeField]
    private UnityEvent m_callback = new UnityEvent();

    /// <summary> ��ԋ��� </summary>
    protected float m_length = 1.0f;

    /// <summary> ��ԃ|�C���g�ԍ� </summary>
    protected int m_sectionNumber = 0;

    /// <summary> �ړ��x�N�g�� </summary>
    protected Vector3 m_moveDistance;


    public int m_SectionNumber { get { return m_sectionNumber; } }

    public bool m_IsSpeedEvent { get { return m_isSpeedEvent; } }
    
    public bool m_IsReplaceDeadZone { get { return m_isReplaceDeadZone; } }


    /// <summary> �p�����[�^������ </summary>
    public void InitParam(int sectionNumber)
    {
        m_sectionNumber = sectionNumber;
        m_moveDistance = transform.parent.GetChild(m_sectionNumber + 1).position - transform.position;
        m_length = m_moveDistance.magnitude;
    }

    /// <summary> ���݈ʒu��Ԃ� </summary>
    /// <param name="time"> �o�ߕb�� </param>
    /// <param name="isFinish"> ���̋�Ԃ�ʂ�I�������ǂ��� </param>
    /// <param name="overtime"> ���������߂����ꍇ�]��̕b����Ԃ� </param>
    public virtual Vector3 GetPosition(float time, ref bool isFinish, ref float overtime)
    {
        // �ʒu���v�Z
        float len = time * m_speed;
        float progress;

        // ��Ԃ�ʂ�I������
        if (len < m_length)
        {
            progress = len / m_length;
        }
        else
        {
            progress = 1.0f;
            isFinish = true;
            // ���ߕ������Ԃɕϊ�
            overtime = (len - m_length) / m_speed;
        }

        Vector3 pos = transform.position;
        pos += m_moveDistance * progress;

        return pos;
    }

    public void InvokeCallback()
    {
        m_callback.Invoke();
    }
}
