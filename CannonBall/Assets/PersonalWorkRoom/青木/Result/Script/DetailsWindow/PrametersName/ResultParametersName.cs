using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultParametersName : MonoBehaviour
{
    [SerializeField]
    Sprite[] m_sprite;

    Image[] m_tmps;

    private void OnEnable()
    {
        m_tmps = new Image[transform.childCount];

        for (int i = 0; i < m_tmps.Length; i++)
        {
            m_tmps[i] = transform.GetChild(i).GetComponent<Image>();
        }

        for (int i = 0;i < m_tmps.Length;i++)
        {
            switch (TemporaryData.m_PreGameMode)
            {
                case SceneNameEnum.CannonFight:
                    m_tmps[i].sprite = m_sprite[0];
                    break;
                case SceneNameEnum.BombRush:
                    m_tmps[i].sprite = m_sprite[1];
                    break;
                case SceneNameEnum.DangerRun:
                    m_tmps[i].sprite = m_sprite[2];
                    break;

                default:
                    m_tmps[i].sprite = null;
                    break;
            }

        }
    }


}
