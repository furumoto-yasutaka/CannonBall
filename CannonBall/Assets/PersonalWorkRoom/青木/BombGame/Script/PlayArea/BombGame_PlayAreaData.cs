/*******************************************************************************
*
*	�^�C�g���F	�v���C���[���ړ��ł����Ԃ̊Ǘ��Ɏg���f�[�^�u����
*	�t�@�C���F	BombGame_PlayAreaData.cs
*	�쐬�ҁF	�� �喲
*	������F    2023/10/05
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGame_PlayAreaData : SingletonMonoBehaviour<BombGame_PlayAreaData>
{

    #region �t�B�[���h

    [SerializeField, CustomLabel("�X�e�[�W��HP�X�N���v�g��12���̕��p���甽���v���")]
    GameObject[] m_stageObject;



    // �v���C���[���ړ��ł���G���A�̐�
    static readonly int m_AREA_NUM = 4;

    // ���e�����݂��Ă���G���A�̔ԍ����i�[�����
    List<int> m_inAreaNumbers = new List<int>();

    #endregion


    #region �v���p�e�B

    /// <summary> �v���C�G���A�̐��@�S�ŌŒ� </summary>
    /// <returns></returns>
    public static int GetMaxAreaNumber() { return m_AREA_NUM; }

    /// <summary> �X�e�[�W�̃I�u�W�F�N�g�@���̃I�u�W�F�N�g�ɃX�N���v�g������ </summary>
    /// <returns></returns>
    public GameObject[] GetStageObject() { return m_stageObject; }

    /// <summary> ���e�����݂��Ă���G���A�̔ԍ��̐擪��Ԃ� </summary>
    /// <returns></returns>
    public int GetSpawAreaNumber() { return m_inAreaNumbers[0]; }

    /// <summary> ���e�������Ă���ԍ��̃��X�g </summary>
    /// <returns></returns>
    public List<int> GetInAreaNumbers() { return m_inAreaNumbers; }


    #endregion


    protected override void Awake()
    {
        // �V�[�����ׂ��ő��݂��闝�R�Ȃ�
        dontDestroyOnLoad = false;

        // �p�����̌Ăяo��
        base.Awake();

        //// �����ʒu�̔ԍ������
        //m_inAreaNumbers.Add(0);
    }

    private void Update()
    {

        // �O�t���[���̃f�[�^�����t���b�V��
        m_inAreaNumbers.Clear();


        // �v���C�G���A�̊p�x�̑傫�����v�Z
        float areaDistanceAngle = 360.0f / m_AREA_NUM;

        List<BombCharacter> bombCharacters = BombManager.Instance.GetNowExistBombCharacters();

        // ���e���̌��݂̋��ꏊ��T��
        for (int i = 0; i < bombCharacters.Count; i++)
        {
            Vector2 bombPos = bombCharacters[i].transform.position;

            // ���v���Ɋp�x���o��
            float angle = Vector3.SignedAngle(Vector3.up, bombPos, Vector3.forward);

            // �p�x���O�`360�x�͈̔͂ɂɐ��K������
            angle = Mathf.Repeat(angle, 360.0f);


            // ���e�̌��݂̋��ꏊ��T��
            for (int j = 0; j < m_AREA_NUM; j++)
            {
                if (angle <= areaDistanceAngle * (j + 1))
                {
                    m_inAreaNumbers.Add(j);
                    bombCharacters[i].m_InAreaNumber = j;

                    break;
                }
            }

        }
    }

}
