/*******************************************************************************
*
*	タイトル：	ビネット制御用シングルトンクラス
*	ファイル：	VignetteController.cs
*	作成者：	古本 泰隆
*	制作日：    2023/05/31
*	更新履歴：　
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class VignetteController : SingletonMonoBehaviour<VignetteController>
{
    /// <summary> ビネットの状態 </summary>
    public enum State
    {
        VignetteOn = 0,
        ChangeVignetteOn,
        VignetteOff,
        ChangeVignetteOff,
        Length,
    }

    #region field

    /// <summary> ビネットのインスタンス </summary>
    private Vignette m_vignette;

    /// <summary> ビネットの強さ </summary>
    private float m_toIntensity = 0.0f;
    /// <summary> ピント距離 </summary>
    private float m_intensityPerFrame = 0.0f;
    /// <summary> インスペクターから入力可能にするための一時変数 </summary>
    private int m_toFrame = 0;
    /// <summary> インスペクターから入力可能にするための一時変数 </summary>
    private int m_frameCount = 0;

    /// <summary> インスペクターから入力可能にするための一時変数 </summary>
    private State m_state = State.VignetteOff;

    #endregion

    #region function

    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();

        if (!GetComponent<Volume>().profile.TryGet(out m_vignette))
        {
#if UNITY_EDITOR
            Debug.LogError("VignetteがVolumeに追加されていません");
#endif
        }
    }

    private void Update()
    {
        if (m_state == State.ChangeVignetteOn)
        {
            UpdateVignetteOn();
        }
        else if (m_state == State.ChangeVignetteOff)
        {
            UpdateVignetteOff();
        }
    }

    /// <summary>
    /// ビネット有効化処理
    /// </summary>
    /// <param name="intensity"> 強さ </param>
    /// <param name="frame"> フレーム数 </param>
    public void VignetteOn(float intensity, int frame)
    {
        // すでに実行中または遷移済みの場合無視する
        if (m_state == State.VignetteOn)
        {
            return;
        }

        // パラメータ設定
        m_state = State.ChangeVignetteOn;
        m_vignette.intensity.value = 0.0f;
        m_toIntensity = intensity;
        m_toFrame = frame;
        m_intensityPerFrame = (m_toIntensity - m_vignette.intensity.value) / m_toFrame;
    }

    /// <summary>
    /// ビネット有効化処理(即時変更)
    /// </summary>
    /// <param name="intensity"> 強さ </param>
    public void VignetteOnFast(float intensity)
    {
        // パラメータ設定
        m_state = State.VignetteOn;
        m_vignette.intensity.value = intensity;
    }

    /// <summary>
    /// ビネット無効化処理
    /// </summary>
    /// <param name="frame"> フレーム数 </param>
    public void VignetteOff(int frame)
    {
        // すでに実行中または遷移済みの場合無視する
        if (m_state == State.VignetteOff)
        {
            return;
        }

        // パラメータ設定
        m_state = State.ChangeVignetteOff;
        m_toFrame = frame;
        m_intensityPerFrame = m_vignette.intensity.value / m_toFrame;
    }

    /// <summary>
    /// ビネット無効化処理(即時変更)
    /// </summary>
    public void VignetteOffFast()
    {
        // パラメータ設定
        m_state = State.VignetteOff;
        m_vignette.intensity.value = 0.0f;
    }

    /// <summary>
    /// ビネット更新処理(有効化)
    /// </summary>
    private void UpdateVignetteOn()
    {
        m_frameCount++;
        m_vignette.intensity.value += m_intensityPerFrame;

        if (m_frameCount == m_toFrame)
        {
            m_state = State.VignetteOn;
            m_frameCount = 0;
        }
    }

    /// <summary>
    /// ビネット更新処理(無効化)
    /// </summary>
    private void UpdateVignetteOff()
    {
        m_frameCount++;
        m_vignette.intensity.value -= m_intensityPerFrame;

        if (m_frameCount == m_toFrame)
        {
            m_state = State.VignetteOff;
            m_frameCount = 0;
        }
    }

    #endregion
}
