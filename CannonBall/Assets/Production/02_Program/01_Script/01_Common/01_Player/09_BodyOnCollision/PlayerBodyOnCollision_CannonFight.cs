/*******************************************************************************
*
*	�^�C�g���F	�v���C���[�̑̂̃R���W�����C�x���g�X�N���v�g(�嗐�����[�hver)
*	�t�@�C���F	PlayerBodyOnCollision_CannonFight.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/08
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyOnCollision_CannonFight : PlayerBodyOnCollision
{
    public struct ImpactEffectInfo
    {
        public string EffectName;
        public float Threshold;
    }


    [SerializeField, CustomLabel("�v���C���[�|�C���g�R���|�[�l���g")]
    private PlayerPoint_CannonFight m_playerPoint;

    [SerializeField, CustomArrayLabel(new string[] { "��", "��", "��", })]
    private ImpactEffectInfo[] m_effectInfo;


    protected override void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            OnCollisionExit_Player(collision);
        }
    }

    /// <summary> �v���C���[�����ꂽ�ۂ̃C�x���g </summary>
    /// <param name="collision"> �v���C���[�̃R���W���� </param>
    private void OnCollisionExit_Player(Collision2D collision)
    {
        // �G�v���C���[�Ɏ������}�[�N����悤���N�G�X�g����
        m_playerPoint.RequestContactMark(
            collision.transform.root.GetComponent<PlayerPoint_CannonFight>(),
            collision.transform.GetComponent<Rigidbody2D>().velocity);
    }
}
