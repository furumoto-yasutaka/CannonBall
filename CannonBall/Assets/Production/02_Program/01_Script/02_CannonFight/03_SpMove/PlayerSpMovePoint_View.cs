using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpMovePoint_View : MonoBehaviour
{
    /// <summary> ポイント制御コンポーネント </summary>
    [SerializeField, CustomLabel("スライダー")]
    private Slider m_slider;

    /// <summary> 必殺技催促アイコンのアニメーター </summary>
    [SerializeField, CustomLabel("必殺技催促アイコンのアニメーター")]
    private Animator m_pushiconAnimator;

    /// <summary> 溜まったときの必殺技ゲージ </summary>
    [SerializeField, CustomLabel("溜まったときの必殺技ゲージ")]
    private GameObject m_chargedGauge;


    /// <summary> スライダーの描画割合更新 </summary>
    public void SetSliderValue(float value)
    {
        m_slider.value = value;
        m_chargedGauge.SetActive(value == 1.0f);
        m_pushiconAnimator.SetBool("IsShow", value == 1.0f);
    }
}
