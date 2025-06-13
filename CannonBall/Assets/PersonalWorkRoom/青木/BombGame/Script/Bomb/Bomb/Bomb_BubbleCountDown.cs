/*******************************************************************************
*
*	�^�C�g���F	���e�̃f�[�^�@�C���^�[�t�F�[�X�ɕύX���邩���H�H�H
*	�t�@�C���F	BombCharacter.cs
*	�쐬�ҁF	�� �喲
*	������F    2023/09/21
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_BubbleCountDown : MonoBehaviour, IBomb
{
    #region �t�B�[���h�p�����[�^
  
    [SerializeField, CustomLabel("�X�|�[�����̔��ˈЗ�")]
    float m_spawPower;

    [SerializeField, CustomLabel("���e�̎c�莞��(�b)")]
    private float m_aliveTime;

    [SerializeField, CustomLabel("���e�̈З�")]
    private float m_bombDamage;

    /// <summary> ���e�����j���������Ă��Ȃ��� </summary>
    private bool m_isExprosition = false;

    private BombCharacter m_bombCharacter;

    #endregion

    #region �v���p�e�B


    /// <summary> ���e����ǂ̂��炢�Ŕ�������̂� </summary>
    /// <returns> ���e�̎c�莞��(�b) </returns>
    public float GetAliveTime() { return m_aliveTime; }

    /// <summary> ���e���������Ă���̂��ǂ��� </summary>
    /// <returns> ���e�����j���������Ă��Ȃ��� </returns>
    public bool GetisExprosition() { return m_isExprosition; }

    /// <summary> �����ŃG���A������З� </summary>
    /// <returns> ���e�̈З� </returns>
    public float GetBombDamage() {  return m_bombDamage; }

    #endregion

    public void StartImpact(Vector3 _target)
    {
        //float randAngle = 5.0f;
        //
        //float rand = Random.Range(-randAngle, randAngle);
        //rand *= Mathf.Deg2Rad;


        GetComponent<Rigidbody2D>().velocity = _target * m_spawPower;
        //GetComponent<Rigidbody2D>().velocity = new Vector3(Mathf.Cos(_rotation), Mathf.Sin(_rotation), 0.0f) * m_spawPower;
    }


    private void Start()
    {
        m_bombCharacter = GetComponent<BombCharacter>();
    }

    private void Update()
    {
        if (m_isExprosition)
        {
            return;
        }



        m_aliveTime -= Time.deltaTime;

        if (m_aliveTime < 0.0f)
        {
            m_aliveTime = 0.0f;

            // ���������u��
            if (!m_isExprosition)
            {
                transform.GetChild(0).gameObject.SetActive(false);

                m_isExprosition = true;

            }
        }


        if (m_isExprosition)
        {
            // �G�t�F�N�g���o��
            EffectContainer.Instance.EffectPlay("����", transform.position);

            // �G���A��HP�����炷
            BombGame_PlayAreaHealthManager.Instance.SubHealth(m_bombCharacter.m_InAreaNumber, m_bombDamage);

            // ���e���X�g�̗v�f���폜
            BombManager.Instance.RemoveBombList(m_bombCharacter);

            //// �I�u�W�F�N�g��j��
            Destroy(gameObject);
        }
    }


}
