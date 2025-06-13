using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class TutorialTargetObject_UIPresenter : MonoBehaviour
{
    [SerializeField]
    TutorialTargetObject m_targetObject;

    TextMeshProUGUI m_textMeshProUGUI;

    private void Start()
    {
        m_textMeshProUGUI = GetComponent<TextMeshProUGUI>();


        m_targetObject.m_KickHeath.Subscribe(_=>
        {
            // �e�L�X�g�ɑ��
            m_textMeshProUGUI.text = m_targetObject.m_KickHeath.Value.ToString();

        }).AddTo(this);
        
    }

    private void Update()
    {
        // ��]�����Ȃ�
        transform.localRotation = Quaternion.identity * Quaternion.Inverse(transform.parent.rotation);
    }
}
