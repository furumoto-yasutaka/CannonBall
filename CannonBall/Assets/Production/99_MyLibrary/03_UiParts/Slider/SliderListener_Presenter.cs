/*******************************************************************************
*
*	タイトル：	スライダーリスナー情報の橋渡し
*	ファイル：	SliderListener_Presenter.cs
*	作成者：	古本 泰隆
*	制作日：    2023/05/11
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SliderListener_Presenter : MonoBehaviour
{
    // View & Model -------------------------------------
    /// <summary> スライダー親オブジェクト </summary>
    [SerializeField]
    protected Transform m_sliderParent;


    // Resource    -------------------------------------
    /// <summary> 収集アイテムアイコン(未取得) </summary>
    [SerializeField, CustomLabel("パーセント画像")]
    protected Sprite m_percentSprite;

    [SerializeField, CustomLabel("数値画像")]
    protected Sprite[] m_numberSprite;

    [SerializeField, CustomLabel("プラマイ画像")]
    protected Sprite m_plusMinusSprite;

    [SerializeField, CustomLabel("プラス画像")]
    protected Sprite m_plusSprite;

    [SerializeField, CustomLabel("マイナス画像")]
    protected Sprite m_minusSprite;


    protected virtual void Start()
    {
        // 各スライダーの段階変化を監視・初期化
        for (int i = 0; i < m_sliderParent.childCount; i++)
        {
            SliderListener listener;
            if (!m_sliderParent.GetChild(i).TryGetComponent(out listener))
            {
                continue;
            }

            listener.m_StepReactiveProperty.Subscribe(v =>
                {
                    SetText(listener);
                })
                .AddTo(this);

            SetText(listener);
        }
    }

    /// <summary>
    /// スライダーの値テキスト設定
    /// </summary>
    /// <param name="listener"> 対象のスライダーのリスナー </param>
    private void SetText(SliderListener listener)
    {
        if (listener.m_ValueTextPattern == SliderListener.ValueTextPattern.Persent)
        {
            if (listener.m_IsTextUseTMP)
            {
                listener.SetTMPText_Persent();
            }
            else
            {
                listener.SetText_Persent(m_numberSprite, m_percentSprite);
            }
        }
        else if (listener.m_ValueTextPattern == SliderListener.ValueTextPattern.Relative)
        {
            if (listener.m_IsTextUseTMP)
            {
                listener.SetTMPText_Relative();
            }
            else
            {
                listener.SetText_Relative(m_numberSprite, m_plusMinusSprite, m_plusSprite, m_minusSprite);
            }
        }
        else if (listener.m_ValueTextPattern == SliderListener.ValueTextPattern.Persent_Select)
        {
            if (listener.m_IsTextUseTMP)
            {
                listener.SetTMPText_Persent_Select();
            }
            else
            {
                listener.SetText_Persent_Select(m_numberSprite, m_percentSprite);
            }
        }
    }
}
