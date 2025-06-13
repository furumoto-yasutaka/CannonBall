using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DetailWindowStarter : MonoBehaviour
{
    [SerializeField, CustomLabel("�ڍ׳���޳ ٰ� �ްѵ�޼ު��")]
    GameObject m_detailWindow;

    // �ڍ׃E�B���h�E�ŕ\������K�v���Ȃ��I�u�W�F�N�g�̃��[�g�I�u�W�F�N�g��I��
    [SerializeField, CustomLabel("���Ҕ��\UI�n")]
    GameObject[] m_winnerObjects;

    private void Start()
    {
        ResultSceneController.Instance.m_State.Subscribe(_ =>
        {
            if (_ == ResultSceneController.STATE.DETAIL_WINDOW)
            {
                foreach (var obj in m_winnerObjects)
                {
                    obj.SetActive(false);
                }

                m_detailWindow.SetActive(true);

                ResultSceneController.Instance.SetWaitTime();
            }
        }).AddTo(this);
    }
}
