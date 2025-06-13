using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTruck_DangerRun : RespawnTruck
{
    public override void StartJumpOut()
    {
        Transform road = transform.GetChild(1);
        for (int i = 0; i < road.childCount; i++)
        {
            Transform player = road.GetChild(i);

            // プレイヤーをトラックから分離しカメラの子に設定する
            Transform camera = ((RespawnManager_DangerRun)RespawnManager_DangerRun.Instance).m_Camera;
            player.SetParent(camera);

            player.localPosition = Vector3.zero;
            player.localRotation = Quaternion.identity;

            // プレイヤーの飛び出すアニメーションを指定する
            RespawnManager.Instance.SetRespawnAnimation(player);

            // エフェクトを生成
            EffectContainer.Instance.EffectPlay(
                "リスポーン",
                road.position,
                Quaternion.Euler(-90.0f, 0.0f, 0.0f));
        }

        AudioManager.Instance.PlaySe(
            "リスポーン_飛び出す音",
            false);
    }
}
