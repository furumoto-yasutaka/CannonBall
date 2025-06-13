/*******************************************************************************
*
*	�^�C�g���F	�v���C���[�̑��̃R���W�����C�x���g�X�N���v�g(�嗐�����[�hver)
*	�t�@�C���F	PlayerLegOnCollision_CannonFight.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/08
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLegOnCollision_CannonFight : PlayerLegOnCollision
{
    /// <summary> �v���C���[�|�C���g�R���|�[�l���g </summary>
    [SerializeField, CustomLabel("�v���C���[�|�C���g�R���|�[�l���g")]
    private PlayerPoint_CannonFight m_playerPoint;

    /// <summary> �v���C���[�K�E�Z�|�C���g�R���|�[�l���g </summary>
    [SerializeField, CustomLabel("�v���C���[�K�E�Z�|�C���g�R���|�[�l���g")]
    private PlayerSpMove m_playerSpMovePoint;


    /// <summary> �v���C���[���G�ꂽ�ۂ̃C�x���g </summary>
    /// <param name="other"> �v���C���[�̃R���W���� </param>
    protected override void OnTriggerEnter_Player(Collider2D other)
    {
        base.OnTriggerEnter_Player(other);

        other.transform.root.GetComponent<PlayerPoint_CannonFight>().RequestKickMark(m_playerPoint);

        // �K�E�Z�|�C���g����
        other.transform.root.GetComponent<PlayerSpMove>().AccumulateBeKickedPattern();
        m_playerSpMovePoint.AccumulateKickPattern();
    }

    /// <summary> �n�`���G�ꂽ�ۂ̃C�x���g </summary>
    /// <param name="other"> �n�`�̃R���W���� </param>
    protected override void OnTriggerEnter_Platform(Collider2D other)
    {
        //// ���̊p�x�Ɠ��������n�`�̖@���̕���������Ă���ꍇ�R�����Ɣ��f���Ȃ�
        //List<ContactPoint2D> points = new List<ContactPoint2D>();
        //other.GetContacts(points);
        //foreach (ContactPoint2D point in points)
        //{
        //    if (Mathf.Abs(Vector2.Dot(point.normal, m_playerController.m_KickDir)) <= 0.05f)
        //    {
        //        return;
        //    }
        //}

        base.OnTriggerEnter_Platform(other);
    }
}
