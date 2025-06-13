using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstRespawnSetter : MonoBehaviour
{
    private void Awake()
    {
        // プレイヤーの初期リスポーンを行う
        List<Transform> players = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform p = transform.GetChild(i);
            p.GetComponent<PlayerController_CannonFight>().FirstDeath();
            players.Add(p);
        }
        RespawnManager.Instance.StartRespawnAll(players.ToArray());

        // 不要なのでこのコンポーネントを削除
        Destroy(this);
    }
}
