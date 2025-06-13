using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnBoxSetter : MonoBehaviour
{
    [SerializeField, CustomLabel("���X�|�[���{�b�N�X���f���f�[�^�}�b�v")]
    private PlayerRespawnBoxModelMap m_playerRespawnBoxModelMap;


    private void Start()
    {
        // �v���r���[�p���f�����폜����
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        // ���f���I�u�W�F�N�g�𐶐�
        Instantiate(m_playerRespawnBoxModelMap.m_PlayerRespawnBoxModels.m_Models[transform.root.GetComponent<PlayerId>().m_Id], transform);

        // �s�v�Ȃ̂ł��̃R���|�[�l���g���폜
        Destroy(this);
    }
}
