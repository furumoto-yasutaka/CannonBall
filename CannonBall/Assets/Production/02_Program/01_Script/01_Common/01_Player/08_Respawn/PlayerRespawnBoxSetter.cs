using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnBoxSetter : MonoBehaviour
{
    [SerializeField, CustomLabel("リスポーンボックスモデルデータマップ")]
    private PlayerRespawnBoxModelMap m_playerRespawnBoxModelMap;


    private void Start()
    {
        // プレビュー用モデルを削除する
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        // モデルオブジェクトを生成
        Instantiate(m_playerRespawnBoxModelMap.m_PlayerRespawnBoxModels.m_Models[transform.root.GetComponent<PlayerId>().m_Id], transform);

        // 不要なのでこのコンポーネントを削除
        Destroy(this);
    }
}
