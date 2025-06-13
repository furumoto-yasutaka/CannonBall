/*******************************************************************************
*
*	�^�C�g���F	�v���C���[�`�[�����ݒ�X�N���v�g
*	�t�@�C���F	SuccerTeamInfoSetter.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/01
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccerTeamInfoSetter : MonoBehaviour
{
    /// <summary> �`�[���̐F </summary>
    public enum TeamColor
    {
        Red = 0,
        Blue,

        Length,
    }


    /// <summary> �e�`�[���̔z�F </summary>
    [SerializeField, CustomArrayLabel(typeof(TeamColor))]
    private Color[] m_teamUiColor;

    /// <summary> �`�[���̈ړ��͈͂𐧌�����ǂ̐e�I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("�`�[���̈ړ��͈͂𐧌�����ǂ̐e�I�u�W�F�N�g")]
    private Transform m_teamAreaWallParent;


    public Color[] m_TeamUiColor { get { return m_teamUiColor; } }

    public Transform m_TeamAreaWallParent { get { return m_teamAreaWallParent; } }


    // PlayerIdSetter�Ƀy�A�����g�����̖���������̂ŁA
    // �������s�����悤�D��x��-200�ɂ����Ă��܂�
    private void Awake()
    {
        // �`�[������t�^
        // ���`�[���I����ʂ�����ꍇ�����ŏ�������Ă���
        // �v���C���[�Ƀ`�[����������U��A�v���C���[�̈ʒu�������̐w�n�̕��Ɉړ�������
        // �Ƃ肠��������͑��݂���v���C���[�̔�����ԁA�ɂ��邾��
        int i = 0;
        for (; i < transform.childCount / 2; i++)
        {
            Transform t = transform.GetChild(i);
            t.GetComponent<SuccerTeamInfo>().InitInfo(TeamColor.Red, this);
        }
        for (; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            t.GetComponent<SuccerTeamInfo>().InitInfo(TeamColor.Blue, this);
        }
    }
}
