using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTruck : MonoBehaviour
{
    /// <summary> アニメーターコンポーネント </summary>
    protected Animator m_animator;


    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void SetPlayer(Transform player)
    {
        player.SetParent(transform.GetChild(1));
        player.localPosition = Vector3.zero;
        player.localRotation = Quaternion.identity;
    }

    public virtual void StartJumpOut()
    {
        Transform road = transform.GetChild(1);
        while (road.childCount > 0)
        {
            Transform player = road.GetChild(0);

            // プレイヤーをトラックから分離
            player.SetParent(null);

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

    public void DestroyTruck()
    {
        Destroy(gameObject);
    }
}
