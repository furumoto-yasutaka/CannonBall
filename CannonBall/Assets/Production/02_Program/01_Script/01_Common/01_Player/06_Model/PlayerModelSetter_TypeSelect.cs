using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelSetter_TypeSelect : MonoBehaviour
{
    [SerializeField, CustomLabel("�v���C���[���f���f�[�^�}�b�v")]
    private PlayerModelMap_CannonFight m_playerModelMap;

    [SerializeField, CustomLabel("�u�����������f���̖��O��ύX����")]
    private bool m_isChangeModelName = false;

    [SerializeField, CustomLabel("�u�����������f���ɕt���閼�O")]
    private string m_changeModelName = "";


    public void ChangeModel(int cursorNum)
    {
        // �v���r���[�p���f�����폜����
        DestroyImmediate(transform.GetChild(1).gameObject);

        // ���f���I�u�W�F�N�g�𐶐�
        PlayerModels m = m_playerModelMap.m_PlayerModelsByType[cursorNum];
        GameObject obj = Instantiate(m.m_Models[transform.parent.parent.GetComponent<PlayerId>().m_Id], transform);
        if (m_isChangeModelName)
        {
            obj.name = m_changeModelName;
        }
    }
}
