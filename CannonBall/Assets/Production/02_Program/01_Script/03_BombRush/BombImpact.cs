/*******************************************************************************
*
*	�^�C�g���F	���e���R��ꂽ�肵�ē����X�N���v�g
*	�t�@�C���F	BombImpact.cs
*	�쐬�ҁF	�� �喲
*	������F    2023/09/19
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombImpact : MonoBehaviour
{
    /// <summary> ���W�b�h�{�f�B </summary>
    private Rigidbody2D m_rb;

    /// <summary> �����̑��x�ɉ��������˂��̉��Z���� </summary>
    [SerializeField, CustomLabel("�����̑��x�ɉ��������˂��̋����̉��Z����")]
    protected float m_headbuttAddRate = 0.2f;

    /// <summary> �Ԃ��Ƃ񂾂Ƃ��ɂ���������x1������̉�]�����x </summary>
    [SerializeField, CustomLabel("�Ԃ��Ƃ񂾂Ƃ��ɂ���������x1������̉�]�����x")]
    protected float m_kickAngularPower = 15.0f;



    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    public void Impact(Vector2 dir, float power, Vector2 vel)
    {
        // ���x���v�Z�E���f
        Vector2 baseVel = dir * power;
        Vector2 resultVel = baseVel;

        resultVel += dir * vel.magnitude;

        m_rb.velocity = resultVel;
    }

    /// <summary>
    /// �v���C���[�ɓ��˂��ꂽ����
    /// </summary>
    public void Headbutted(Vector2 dir, float power, Vector2 vel)
    {
        // ���x���v�Z�E���f
        Vector2 resultVel = dir * power;
        resultVel += dir * vel.magnitude * m_headbuttAddRate;
        m_rb.velocity = resultVel;

        // ���x�����ɉ�]�����x���v�Z�E���f
        m_rb.angularVelocity = m_rb.velocity.x * -m_kickAngularPower;
    }
}
