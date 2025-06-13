using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTargetSelector : MonoBehaviour
{
    // �o�[�`�����J����
    [SerializeField] private CinemachineVirtualCamera m_virtualCamera;

    // �Ǐ]�Ώۃ��X�g
    [SerializeField] private Transform[] m_targetTransform;


    [SerializeField] private bool m_isChange = false;


    // �I�𒆂̃^�[�Q�b�g�̃C���f�b�N�X
    private int m_currentTarget = 0;

    // �t���[���X�V
    private void Update()
    {
        // �Ǐ]�Ώۏ�񂪐ݒ肳��Ă��Ȃ���΁A�������Ȃ�
        if (m_targetTransform == null || m_targetTransform.Length <= 0)
            return;

        // �}�E�X�N���b�N���ꂽ��
        if (m_isChange)
        {
            // �Ǐ]�Ώۂ����Ԃɐ؂�ւ�
            if (++m_currentTarget >= m_targetTransform.Length)
                m_currentTarget = 0;

            // �Ǐ]�Ώۂ��X�V
            var info = m_targetTransform[m_currentTarget];
            m_virtualCamera.LookAt = info;


            m_isChange = false;
        }
    }

    public void ChangeViewTarget(int _targetNum)
    {
        // �Ǐ]�Ώۂ����Ԃɐ؂�ւ�
        m_currentTarget = _targetNum;
        m_currentTarget %= m_targetTransform.Length;

        // �Ǐ]�Ώۂ��X�V
        var info = m_targetTransform[m_currentTarget];
        m_virtualCamera.LookAt = info;

    }

}
