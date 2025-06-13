/*******************************************************************************
*
*	�^�C�g���F	�v���C���[�`�[�����X�N���v�g
*	�t�@�C���F	SuccerTeamInfo.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/01
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccerTeamInfo : MonoBehaviour
{
    /// <summary> �ǂ̃`�[���� </summary>
    [SerializeField, CustomLabelReadOnly("�ǂ̃`�[����")]
    private SuccerTeamInfoSetter.TeamColor m_teamColor = SuccerTeamInfoSetter.TeamColor.Red;

    /// <summary> �v���C���[�ԍ�UI </summary>
    [SerializeField, CustomLabel("�v���C���[�ԍ�UI")]
    private SpriteRenderer m_playerNumberUi;

    /// <summary> �̂̓����蔻�� </summary>
    [SerializeField, CustomLabel("�̂̓����蔻��")]
    private Collider2D m_bodyCol;

    /// <summary> ���̓����蔻�� </summary>
    [SerializeField, CustomLabel("���̓����蔻��")]
    private Collider2D m_legCol;


    public SuccerTeamInfoSetter.TeamColor m_TeamColor { get { return m_teamColor; } }


    /// <summary> ���_�ݒ� </summary>
    /// <param name="color"> �ǂ̃`�[���� </param>
    /// <param name="setter"> �T�b�J�[�`�[���ݒ�R���|�[�l���g </param>
    public void InitInfo(SuccerTeamInfoSetter.TeamColor color, SuccerTeamInfoSetter setter)
    {
        m_teamColor = color;

        // UI�̐F��ύX����
        m_playerNumberUi.color = setter.m_TeamUiColor[(int)color];

        // �����̃`�[���ȊO�̕ǂƂ̓����蔻����폜����
        // ��
        for (int i = 0; i < setter.m_TeamAreaWallParent.childCount; i++)
        {
            if (i == (int)color) { continue; }

            Transform child = setter.m_TeamAreaWallParent.GetChild(i);
            BoxCollider2D wallCol = child.GetComponent<BoxCollider2D>();
            Physics2D.IgnoreCollision(m_bodyCol, wallCol);
        }
        // ��
        m_legCol.transform.GetComponent<PlayerLegSuccerTeamAreaWallIgnore>().InitWallList(
            setter.m_TeamAreaWallParent,
            m_TeamColor);
    }
}
