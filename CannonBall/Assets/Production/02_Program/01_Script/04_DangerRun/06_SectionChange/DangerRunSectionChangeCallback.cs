using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerRunSectionChangeCallback : MonoBehaviour
{
    [SerializeField, CustomLabel("アクティブにするオブジェクト")]
    private GameObject[] m_acriveObj;

    [SerializeField, CustomLabel("向きを逆にするベルトコンベア")]
    private Transform[] m_changeBeltDirObj;

    [SerializeField, CustomLabel("アニメーションを作動させるゴールゲート")]
    private Animator m_goalgate;

    [SerializeField, CustomLabel("ゴールゲートを開くか")]
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
            AudioManager.Instance.PlaySe("デンジャラン_ゲート開閉", false);
        }
    }
}
