using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ResultPlayerRePosition : MonoBehaviour
{
    [SerializeField, CustomLabel("プレイヤーの親オブジェクト")]
    private Transform m_playerParent;

    List<GameObject> m_playerObjects = new List<GameObject>();

    List<StandGetPlayerPosition> m_standObject;

    private void Start()
    {
        TemporaryData.Rank[] ranks = TemporaryData.GetRank(1);

        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < playerObjects.Length; i++)
        {
            int id = playerObjects[i].GetComponent<PlayerId>().m_Id;

            for (int j = 0; j < ranks.Length; j++)
            {
                // 全てのプレイヤーのIDから、欲しいIDと合致した場合
                if (id == ranks[j].m_PlayerId)
                {
                    m_playerObjects.Add(playerObjects[i]);
                }
            }
        }

        // 2位以下を非表示にする
        for (int r = 2; r <= 4; r++)
        {
            ranks = TemporaryData.GetRank(r);

            for (int i = 0; i < playerObjects.Length; i++)
            {
                int id = playerObjects[i].GetComponent<PlayerId>().m_Id;

                for (int j = 0; j < ranks.Length; j++)
                {
                    // 全てのプレイヤーのIDから、欲しいIDと合致した場合
                    if (id == ranks[j].m_PlayerId)
                    {
                        playerObjects[i].SetActive(false);
                    }
                }
            }
        }

        ResultSceneController.Instance.m_State.Subscribe(_ =>
        {
            if (_ == ResultSceneController.STATE.SHOW_WINNER)
            {
                m_standObject = GetComponent<SpawPodium>().GetSpawPodiumList();

                for (int i = 0; i < m_playerObjects.Count; i++)
                {
                    m_playerObjects[i].transform.position = m_standObject[i].GetPoint();
                }
            }

        }).AddTo(this);

    }

    

}
