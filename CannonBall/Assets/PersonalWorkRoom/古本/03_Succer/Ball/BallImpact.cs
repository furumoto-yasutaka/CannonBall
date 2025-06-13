/*******************************************************************************
*
*	�^�C�g���F	�{�[���̏Ռ��𐧌䂷��X�N���v�g(�T�b�J�[���[�hver)
*	�t�@�C���F	BallImpact.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/04
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallImpact : MonoBehaviour
{
    /// <summary> �R�����Ƃ��ɂ���������x1������̉�]�����x </summary>
    [SerializeField, CustomLabel("�����x1������̏R��ꎞ�̒ǉ����x����")]
    protected Vector2 m_inertiaPowerRate;

    /// <summary> �R��ꂽ�Ƃ��ɂ���������x1������̉�]�����x </summary>
    [SerializeField, CustomLabel("�R��ꂽ�Ƃ��ɂ���������x1������̉�]�����x")]
    protected float m_kickedAngularPower = 100.0f;

    /// <summary> ���W�b�h�{�f�B </summary>
    protected Rigidbody2D m_rb;


    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    /// <summary> �R��ꂽ�Ռ���t�^���� </summary>
    /// <param name="dir"> �R��ꂽ���� </param>
    /// <param name="power"> �R��̈З� </param>
    /// <param name="vel"> �R�����v���C���[�̉����x </param>
    public void Kicked(Vector2 dir, float power, Vector2 vel)
    {
        // ���x���v�Z�E���f
        Vector2 impact = dir * power;
        impact += dir * vel.magnitude * m_inertiaPowerRate;
        m_rb.velocity = impact;

        // ���x�����ɉ�]�����x���v�Z�E���f
        m_rb.angularVelocity = m_rb.velocity.x * -m_kickedAngularPower;
    }
}
