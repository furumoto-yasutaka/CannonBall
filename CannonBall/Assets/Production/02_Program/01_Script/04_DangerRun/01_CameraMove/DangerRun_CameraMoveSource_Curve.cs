/*******************************************************************************
*
*	�^�C�g���F	�����X�N���[���̃J�����ړ��o�H�Q�Ɨp�X�N���v�g(�ړ����x���J�[�u�w��)
*	�t�@�C���F	DangerRun_CameraMoveSource_Curve.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/06
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerRun_CameraMoveSource_Curve : DangerRun_CameraMoveSource
{
    /// <summary> �ړ����x�̖ڕW�l </summary>
    [SerializeField, CustomLabel("�ړ����x�̖ڕW�l")]
    private float m_speedTarget = 1.0f;

    /// <summary> �ړ����x�̕�ԑ��x </summary>
    [SerializeField, CustomLabel("�ړ����x�̕�ԑ��x")]
    private float m_speedLarpRate = 0.1f;

    private float m_nowSpeed;


    private void Start()
    {
        m_nowSpeed = m_speed;
    }

    /// <summary> ���݈ʒu��Ԃ� </summary>
    /// <param name="time"> �o�ߕb�� </param>
    /// <param name="overtime"> ���������߂����ꍇ�]��̕b����Ԃ� </param>
    public override Vector3 GetPosition(float time, ref bool isFinish, ref float overtime)
    {
        m_nowSpeed += (m_speedTarget - m_nowSpeed) * m_speedLarpRate;

        // �ʒu���v�Z
        float len = time * m_nowSpeed;
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
            overtime = (len - m_length) / m_nowSpeed;
        }

        Vector3 pos = transform.position;
        pos += m_moveDistance * progress;

        return pos;
    }
}
