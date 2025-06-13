using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Tutorial : PlayerController
{
    [Header("�`���[�g���A���p���͐���")]
    [SerializeField, CustomLabel("�X�e�B�b�N�ړ�������")]
    private bool m_isMove = true;
    [SerializeField, CustomLabel("�L�b�N�ړ�������")]
    private bool m_isKick = true;


    public bool m_IsMove { get { return m_isMove; } set { m_isMove = value; } }

    public bool m_IsKick { get { return m_isKick; } set { m_isKick = value; } }

    protected override void PlayAction()
    {
        // �ړ��ɂ��Ă̍X�V
        if (m_isMove)
        {
            MoveUpdate();
        }
        // �R��ɂ��Ă̍X�V
        if (m_isKick)
        {
            KickUpdate();
        }
        // ���˂��ɂ��Ă̍X�V
        HeadbuttUpdate();
        // �̂̉�]�Ɏ����Ă�����Ȃ��悤�ɑ��̕�����␳����
        m_legParent.transform.rotation =
            Quaternion.identity * Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.down, m_kickDir), Vector3.forward);

        if (m_revivalNextFrame)
        {
            // �R���ďo�Ă��铮��
            m_playerImpact.KickPlatform();
            m_revivalNextFrame = false;
        }
    }
}
