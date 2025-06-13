using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DetailWindow_SceneSelectStarter : MonoBehaviour
{
    [SerializeField]
    GameObject m_detailWindowNextSceneSelect;


    private void Start()
    {
        ResultSceneController.Instance.m_State.Subscribe(_ =>
        {
            if (_ == ResultSceneController.STATE.NEXT_SCENE_SELECT)
            {
                m_detailWindowNextSceneSelect.SetActive(true);

                gameObject.SetActive(false);
                ResultSceneController.Instance.SetWaitTime();
            }
        }).AddTo(this);
    }
}
