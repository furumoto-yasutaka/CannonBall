using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerRunSectionChangeCallback : MonoBehaviour
{
    [SerializeField, CustomLabel("�A�N�e�B�u�ɂ���I�u�W�F�N�g")]
    private GameObject[] m_acriveObj;

    [SerializeField, CustomLabel("�������t�ɂ���x���g�R���x�A")]
    private Transform[] m_changeBeltDirObj;

    [SerializeField, CustomLabel("�A�j���[�V�������쓮������S�[���Q�[�g")]
    private Animator m_goalgate;

    [SerializeField, CustomLabel("�S�[���Q�[�g���J����")]
    private bool m_isOpenGoalgate = true;


    public void Callback()
    {
        foreach (GameObject obj in m_acriveObj)
        {
            obj.SetActive(true);
        }
        
        foreach (Transform belt in m_changeBeltDirObj)
        {
            SurfaceEffector2D surfaceEffector = belt.GetChild(0).GetComponent<SurfaceEffector2D>();
            surfaceEffector.speed = -surfaceEffector.speed;
            Vector3 s = belt.localScale;
            s.x = -s.x;
            belt.localScale = s;
        }

        if (m_goalgate != null)
        {
            m_goalgate.SetBool("IsOpen", m_isOpenGoalgate);
            AudioManager.Instance.PlaySe("�f���W������_�Q�[�g�J��", false);
        }
    }
}
