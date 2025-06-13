using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstRespawnTruck : RespawnTruck
{
    public void SetPlayers(Transform[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetParent(transform.GetChild(1).GetChild(i));
            players[i].localPosition = Vector3.zero;
            players[i].localRotation = Quaternion.identity;
        }
    }

    public override void StartJumpOut()
    {
        Transform road = transform.GetChild(1);
        for (int i = 0; i < road.childCount; i++)
        {
            Transform player = road.GetChild(i).GetChild(0);

            // プレイヤーをトラックから分離
            player.SetParent(null);

            // プレイヤーの飛び出すアニメーションを指定する
            RespawnManager.Instance.SetFirstRespawnAnimation(player);

            // エフェクトを生成
            EffectContainer.Instance.EffectPlay(
                "最初のスポーン",
                road.position,
                Quaternion.Euler(-90.0f, 0.0f, 0.0f));
        }
    }
}
