using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TutorialPlayersController : SingletonMonoBehaviour<TutorialPlayersController>
{
    PlayerController[] m_playerControllers;

    override protected void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();
    }

    private void Start()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
        m_playerControllers = new PlayerController[obj.Length];
        for(int i = 0; i < m_playerControllers.Length; i++)
        {
            m_playerControllers[i] = obj[i].GetComponent<PlayerController>();
        }
        // Ç∑Ç◊ÇƒÇÃÉRÉìÉgÉçÅ[ÉãÇOFF
        SetPlayerControllerSleep();
    }

    public void SetPlayerControllerSleep()
    {
        foreach (PlayerController playerController in m_playerControllers)
        {
            playerController.enabled = false;
        }
    }


    public void SetPlayerControllerAwake()
    {
        foreach (PlayerController playerController in m_playerControllers)
        {
            playerController.enabled = true;
        }
    }
}
