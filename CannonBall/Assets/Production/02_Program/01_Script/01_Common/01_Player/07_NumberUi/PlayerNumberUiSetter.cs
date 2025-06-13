using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNumberUiSetter : MonoBehaviour
{
    [SerializeField, CustomLabel("プレイヤー番号UIデータマップ")]
    private PlayerNumberUiMap m_numberUiMap;


    private void Start()
    {
        // プレイヤー番号に応じたスプライトを設定
        GetComponent<SpriteRenderer>().sprite = 
            m_numberUiMap.m_PlayerNumberSprites[transform.root.GetComponent<PlayerId>().m_Id];

        // 不要なのでこのコンポーネントを削除
        Destroy(this);
    }
}
