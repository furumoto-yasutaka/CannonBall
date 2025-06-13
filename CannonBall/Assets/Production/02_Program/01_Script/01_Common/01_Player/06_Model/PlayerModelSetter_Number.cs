using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelSetter_Number : MonoBehaviour
{
    [SerializeField, CustomLabel("プレイヤーモデルデータマップ")]
    private PlayerModelMap m_playerModelMap;

    [SerializeField, CustomLabel("処理を生成された瞬間行うか")]
    private bool m_isQuickExec = true;

    [SerializeField, CustomLabel("置き換えたモデルの名前を変更する")]
    private bool m_isChangeModelName = false;

    [SerializeField, CustomLabel("置き換えたモデルに付ける名前")]
    private string m_changeModelName = "";


    // 優先的に処理を行うため優先度を-190位にしてあります
    private void Awake()
    {
        if (!m_isQuickExec) { return; }

        ChangeModel();
    }

    private void Start()
    {
        if (m_isQuickExec) { return; }

        ChangeModel();
    }

    private void ChangeModel()
    {
        // プレビュー用モデルを削除する
        if (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        // モデルオブジェクトを生成
        GameObject obj = Instantiate(m_playerModelMap.m_PlayerModels.m_Models[transform.root.GetComponent<PlayerId>().m_Id], transform);
        if (m_isChangeModelName)
        {
            obj.name = m_changeModelName;
        }

        // 不要なのでこのコンポーネントを削除
        Destroy(this);
    }
}
