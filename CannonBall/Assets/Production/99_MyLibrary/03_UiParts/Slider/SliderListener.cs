/*******************************************************************************
*
*	タイトル：	スライダーリスナースクリプト
*	ファイル：	SliderListener.cs
*	作成者：	古本 泰隆
*	制作日：    2023/05/07
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class SliderListener : UiListenerBase
{
    public enum ValueTextPattern
    {
        Persent = 0,
        Relative,
        Persent_Select,
        Length,
    }

    #region field

    /// <summary> 値の表記方法 </summary>
    [SerializeField, CustomLabel("値の表記方法")]
    protected ValueTextPattern m_valueTextPattern;

    /// <summary> スライダー </summary>
    [SerializeField, CustomLabel("スライダー")]
    protected Slider m_slider;

    /// <summary> 値を示すテキストにTMPを使うか </summary>
    [SerializeField, CustomLabel("値を示すテキストにTMPを使うか")]
    protected bool m_isTextUseTMP = true;

    /// <summary> 値を示すテキストの親オブジェクト </summary>
    [SerializeField, CustomLabel("値を示すテキストの親オブジェクト")]
    protected Transform m_valueTextParent;

    /// <summary> 段階の最大数 </summary>
    [SerializeField, CustomLabel("段階の最大数")]
    protected int m_stepMax = 10;

    /// <summary> パーセントの上限値(Persent_Select用) </summary>
    [SerializeField, CustomLabel("パーセントの上限値(Persent_Select用)")]
    protected int m_persentMax = 500;

    /// <summary> 現在の段階数 </summary>
    protected ReactiveProperty<int> m_step = new ReactiveProperty<int>();

    #endregion

    #region property

    public ValueTextPattern m_ValueTextPattern { get { return m_valueTextPattern; } }

    public int m_Step
    {
        get { return m_step.Value; }
        set { m_step.Value = value; }
    }

    public bool m_IsTextUseTMP { get { return m_isTextUseTMP; } }

    public ReactiveProperty<int> m_StepReactiveProperty { get { return m_step; } }

    #endregion

    #region function

    protected virtual void Awake()
    {
        InitUiPattern(UiPattern.Slider);
    }

    /// <summary>
    /// 右にハンドルの段階を変更
    /// </summary>
    public override void LeftSlide()
    {
        if (m_step.Value > 0)
        {
            ChangeStep((m_step.Value - 1) % (m_stepMax + 1));
        }
    }

    /// <summary>
    /// 右にハンドルの段階を変更
    /// </summary>
    public override void RightSlide()
    {
        if (m_step.Value < m_stepMax)
        {
            ChangeStep((m_step.Value + 1) % (m_stepMax + 1));
        }
    }

    /// <summary>
    /// スライダーの段階変更
    /// </summary>
    protected virtual void ChangeStep(int step)
    {
        m_step.Value = step;
        SetSliderValue();
    }

    /// <summary>
    /// スライダーコンポーネントの値を更新
    /// </summary>
    protected void SetSliderValue()
    {
        m_slider.value = (float)m_step.Value / m_stepMax;
    }

    /// <summary>
    /// テキスト用画像設定(パーセント表示)
    /// </summary>
    /// <param name="numberSprite"> 数字画像 </param>
    /// <param name="percentSprite"> パーセント記号画像 </param>
    public void SetText_Persent(Sprite[] numberSprite, Sprite percentSprite)
    {
        int i = 0;

        // 記号
        SetSprite(i, percentSprite);
        i++;

        // 数値
        int value = m_step.Value * m_stepMax;
        do
        {
            int d = value % 10;
            SetSprite(i, numberSprite[d]);
            value /= 10;
            i++;
        }
        while (i < 4 && value != 0);

        // 残りの桁はすべてnullで設定
        SetNullSprite(i);
    }

    /// <summary>
    /// テキスト用画像設定(パーセント表示)
    /// </summary>
    public void SetTMPText_Persent()
    {
        // 数値
        int value = m_step.Value * m_stepMax;
        m_valueTextParent.GetComponent<TextMeshProUGUI>().text = value.ToString() + "%";
    }

    /// <summary>
    /// テキスト用画像設定(相対値表示)
    /// </summary>
    /// <param name="numberSprite"> 数字画像 </param>
    /// <param name="plusMinusSprite"> プラマイ記号画像 </param>
    /// <param name="plusSprite"> プラス記号画像 </param>
    /// <param name="minusSprite"> マイナス記号画像 </param>
    public void SetText_Relative(Sprite[] numberSprite,
        Sprite plusMinusSprite, Sprite plusSprite, Sprite minusSprite)
    {
        int i = 0;
        int temp = m_stepMax / 2;
        int relative = m_step.Value - temp;

        // 数値
        SetSprite(i, numberSprite[Mathf.Abs(relative)]);
        i++;

        // 記号
        if (relative < 0)
        {
            SetSprite(i, minusSprite);
        }
        else if (relative > 0)
        {
            SetSprite(i, plusSprite);
        }
        else
        {
            SetSprite(i, plusMinusSprite);
        }
        i++;

        // 残りの桁はすべてnullで設定
        SetNullSprite(i);
    }

    /// <summary>
    /// テキスト用画像設定(パーセント表示)
    /// </summary>
    public void SetTMPText_Relative()
    {
        // 数値
        int temp = m_stepMax / 2;
        int relative = m_step.Value - temp;
        string symbol;
        if (relative < 0)
        {
            symbol = "-";
        }
        else if (relative > 0)
        {
            symbol = "+";
        }
        else
        {
            symbol = "±";
        }
        m_valueTextParent.GetComponent<TextMeshProUGUI>().text = relative.ToString() + symbol;
    }

    /// <summary>
    /// テキスト用画像設定(パーセント表示)
    /// </summary>
    /// <param name="numberSprite"> 数字画像 </param>
    /// <param name="percentSprite"> パーセント記号画像 </param>
    public void SetText_Persent_Select(Sprite[] numberSprite, Sprite percentSprite)
    {
        int i = 0;

        // 記号
        SetSprite(i, percentSprite);
        i++;

        // 数値
        int value = (int)((float)m_step.Value / m_stepMax * m_persentMax);
        do
        {
            int d = value % 10;
            SetSprite(i, numberSprite[d]);
            value /= 10;
            i++;
        }
        while (i < 4 && value != 0);

        // 残りの桁はすべてnullで設定
        SetNullSprite(i);
    }

    /// <summary>
    /// テキスト用画像設定(パーセント表示)
    /// </summary>
    public void SetTMPText_Persent_Select()
    {
        // 数値
        int value = (int)((float)m_step.Value / m_stepMax * m_persentMax);
        m_valueTextParent.GetComponent<TextMeshProUGUI>().text = value.ToString() + "%";
    }

    /// <summary>
    /// 画像1枚を設定
    /// </summary>
    /// <param name="childIndex"> 何桁目か(何番目の子オブジェクトか) </param>
    /// <param name="sprite"> 設定したいスプライト </param>
    private void SetSprite(int childIndex, Sprite sprite)
    {
        Image image = m_valueTextParent.GetChild(childIndex).GetComponent<Image>();
        image.sprite = sprite;
        
        // スプライトがnullだったら表示しないようにする
        if (sprite == null)
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
        }
    }

    /// <summary>
    /// 指定したインデックス以降のスプライトをnullで設定する
    /// </summary>
    /// /// <param name="i"> 何桁目か(この桁以降をすべて非表示にする) </param>
    private void SetNullSprite(int i)
    {
        for (; i < 4; i++)
        {
            SetSprite(i, null);
        }
    }

    #endregion
}
