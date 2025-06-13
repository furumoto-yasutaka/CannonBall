using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNumberUiSetter : MonoBehaviour
{
    [SerializeField, CustomLabel("�v���C���[�ԍ�UI�f�[�^�}�b�v")]
    private PlayerNumberUiMap m_numberUiMap;


    private void Start()
    {
        // �v���C���[�ԍ��ɉ������X�v���C�g��ݒ�
        GetComponent<SpriteRenderer>().sprite = 
            m_numberUiMap.m_PlayerNumberSprites[transform.root.GetComponent<PlayerId>().m_Id];

        // �s�v�Ȃ̂ł��̃R���|�[�l���g���폜
        Destroy(this);
    }
}
