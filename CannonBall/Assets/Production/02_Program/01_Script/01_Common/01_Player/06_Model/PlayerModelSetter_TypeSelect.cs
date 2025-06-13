using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelSetter_TypeSelect : MonoBehaviour
{
    [SerializeField, CustomLabel("プレイヤーモデルデータマップ")]
    private PlayerModelMap_CannonFight m_playerModelMap;

    [SerializeField, CustomLabel("置き換えたモデルの名前を変更する")]
    private bool m_isChangeModelName = false;

    [SerializeField, CustomLabel("置き換えたモデルに付ける名前")]
    private string m_changeModelName = "";


    public void ChangeModel(int cursorNum)
    {
        // プレビュー用モデルを削除する
        DestroyImmediate(transform.GetChild(1).gameObject);

        // モデルオブジェクトを生成
        PlayerModels m = m_playerModelMap.m_PlayerModelsByType[cursorNum];
        GameObject obj = Instantiate(m.m_Models[transform.parent.parent.GetComponent<PlayerId>().m_Id], transform);
        if (m_isChangeModelName)
        {
            obj.name = m_changeModelName;
        }
    }
}
