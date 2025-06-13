using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RemainUi_Presenter : MonoBehaviour
{
    // View     -------------------------------------
    /// <summary> 値テキスト </summary>
    [SerializeField]
    private RemainUi_View_ValueText m_viewValueText;

    /// <summary> 演出用テキスト </summary>
    //[SerializeField]
   // private RemainUpUi_View_Staging m_viewStaging;


    // Model    -------------------------------------
    /// <summary> プレイヤー </summary>
    [SerializeField]
    private PlayerController m_player;


    /// <summary> 初期化によってオブザーバーが反応し要らない処理をしないためのフラグ </summary>
    private bool m_isOnce = true;


    void Start()
    {
        // 変数を監視して変更時に適宜更新を行わせる

        // Model -> View_Gauge・View_ValueText
        RemainManager.Instance.m_Remain.Subscribe(v =>
            {
                m_viewValueText.SetText(v);

                // 残機増加演出は最初は表示しない
                if (m_isOnce)
                {
                    m_isOnce = false;
                    return;
                }

                //m_viewStaging.CreateUi();
            })
            .AddTo(this);
    }
}
