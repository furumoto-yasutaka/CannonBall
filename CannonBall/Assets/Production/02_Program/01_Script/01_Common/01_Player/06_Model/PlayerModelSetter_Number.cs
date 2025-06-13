using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelSetter_Number : MonoBehaviour
{
    [SerializeField, CustomLabel("�v���C���[���f���f�[�^�}�b�v")]
    private PlayerModelMap m_playerModelMap;

    [SerializeField, CustomLabel("�����𐶐����ꂽ�u�ԍs����")]
    private bool m_isQuickExec = true;

    [SerializeField, CustomLabel("�u�����������f���̖��O��ύX����")]
    private bool m_isChangeModelName = false;

    [SerializeField, CustomLabel("�u�����������f���ɕt���閼�O")]
    private string m_changeModelName = "";


    // �D��I�ɏ������s�����ߗD��x��-190�ʂɂ��Ă���܂�
    private void Awake()
    {
        if (!m_isQuickExec) { return; }

        ChangeModel();
    }

    private void Start()
    {
        if (m_isQuickExec) { return; }

        ChangeModel();
    }

    private void ChangeModel()
    {
        // �v���r���[�p���f�����폜����
        if (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        // ���f���I�u�W�F�N�g�𐶐�
        GameObject obj = Instantiate(m_playerModelMap.m_PlayerModels.m_Models[transform.root.GetComponent<PlayerId>().m_Id], transform);
        if (m_isChangeModelName)
        {
            obj.name = m_changeModelName;
        }

        // �s�v�Ȃ̂ł��̃R���|�[�l���g���폜
        Destroy(this);
    }
}
