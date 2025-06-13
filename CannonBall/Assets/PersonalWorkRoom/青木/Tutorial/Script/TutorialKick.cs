using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialKick : MonoBehaviour
{

    PlayerController[] m_controller;
    [SerializeField]
    bool[] m_isKick;

    bool m_isFinish = false;

    private void Start()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        m_isKick = new bool[player.Length];
        m_controller = new PlayerController[player.Length];
        for (int i = 0; i < m_controller.Length; i++)
        {
            m_controller[i] = player[i].GetComponent<PlayerController>();
        }

    }



    private void Update()
    {
        if (m_isFinish)
        {
            return;
        }


        for (int i = 0; i < m_controller.Length; i++)
        {
            if (m_controller[i].m_IsKicking)
            {
                m_isKick[i] = true;
            }
        }

        if (Check())
        {
            // このチュートリアルが終了したら、次のチュートリアルの準備をする
            TutorialManager.Instance.ChangeNextLevel();


            m_isFinish = true;
        }
    }



    private bool Check()
    {
        for (int i = 0;i < m_controller.Length;i++)
        {
            if (!m_isKick[i])
            {
                return false;
            }
        }
        return true;
    }
}
