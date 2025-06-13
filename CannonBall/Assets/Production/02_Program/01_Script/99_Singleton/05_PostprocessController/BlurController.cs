/*******************************************************************************
*
*	タイトル：	ブラー制御用シングルトンクラス
*	ファイル：	BlurController.cs
*	作成者：	古本 泰隆
*	制作日：    2023/05/09
*	更新履歴：　
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class BlurController : SingletonMonoBehaviour<BlurController>
{
//    /// <summary> ブラーの状態 </summary>
//    public enum State
//    {
//        BlurOn = 0,
//        ChangeBlurOn,
//        BlurOff,
//        ChangeBlurOff,
//        Length,
//    }

//    #region field

//    /// <summary> 被写界深度のインスタンス </summary>
//    private DepthOfField m_depthOfField;

//    /// <summary> ピント距離 </summary>
//    private float m_toFoculLength = 0.0f;
//    /// <summary> ピント距離 </summary>
//    private float m_focalLengthPerFrame = 0.0f;
//    /// <summary> インスペクターから入力可能にするための一時変数 </summary>
//    private int m_toFrame = 0;
//    /// <summary> インスペクターから入力可能にするための一時変数 </summary>
//    private int m_frameCount = 0;

//    /// <summary> インスペクターから入力可能にするための一時変数 </summary>
//    private State m_state = State.BlurOff;

//    #endregion

//    #region function

//    protected override void Awake()
//    {
//        dontDestroyOnLoad = false;

//        base.Awake();

//        if (!GetComponent<Volume>().profile.TryGet(out m_depthOfField))
//        {
//#if UNITY_EDITOR
//            Debug.LogError("DepthOfFieldがVolumeに追加されていません");
//#endif
//        }
//    }

//    private void Update()
//    {
//        if (m_state == State.ChangeBlurOn)
//        {
//            UpdateBlurOn();
//        }
//        else if (m_state == State.ChangeBlurOff)
//        {
//            UpdateBlurOff();
//        }
//    }

//    /// <summary>
//    /// ブラー有効化処理
//    /// </summary>
//    /// <param name="focesDistance"> 焦点距離 </param>
//    /// <param name="focalLength"> 焦点距離 </param>
//    /// <param name="frame"> フレーム数 </param>
//    public void BlurOn(float focesDistance, float focalLength, int frame)
//    {
//        // すでに実行中または遷移済みの場合無視する
//        if (m_state == State.BlurOn)
//        {
//            return;
//        }

//        // パラメータ設定
//        m_state = State.ChangeBlurOn;
//        m_depthOfField.focusDistance.value = focesDistance;
//        m_toFoculLength = focalLength;
//        m_toFrame = frame;
//        m_focalLengthPerFrame = (m_toFoculLength - m_depthOfField.focusDistance.value) / m_toFrame;
//    }

//    /// <summary>
//    /// ブラー有効化処理(即時変更)
//    /// </summary>
//    /// <param name="focesDistance"> 焦点距離 </param>
//    /// <param name="focalLength"> 焦点距離 </param>
//    public void BlurOnFast(float focesDistance, float focalLength)
//    {
//        // パラメータ設定
//        m_state = State.BlurOn;
//        m_depthOfField.focusDistance.value = focesDistance;
//        m_depthOfField.focalLength.value = focalLength;
//    }

//    /// <summary>
//    /// ブラー無効化処理
//    /// </summary>
//    /// <param name="frame"> フレーム数 </param>
//    public void BlurOff(int frame)
//    {
//        // すでに実行中または遷移済みの場合無視する
//        if (m_state == State.BlurOff)
//        {
//            return;
//        }

//        // パラメータ設定
//        m_state = State.ChangeBlurOff;
//        m_toFoculLength = 0;
//        m_toFrame = frame;
//        m_focalLengthPerFrame = m_depthOfField.focalLength.value / m_toFrame;
//    }

//    /// <summary>
//    /// ブラー無効化処理(即時変更)
//    /// </summary>
//    public void BlurOffFast()
//    {
//        // パラメータ設定
//        m_state = State.BlurOff;
//        m_depthOfField.focusDistance.value = 0.0f;
//        m_depthOfField.focalLength.value = 0.0f;
//    }

//    /// <summary>
//    /// ブラー更新処理(有効化)
//    /// </summary>
//    private void UpdateBlurOn()
//    {
//        m_frameCount++;
//        m_depthOfField.focalLength.value += m_focalLengthPerFrame;

//        if (m_frameCount == m_toFrame)
//        {
//            m_state = State.BlurOn;
//            m_frameCount = 0;
//        }
//    }

//    /// <summary>
//    /// ブラー更新処理(無効化)
//    /// </summary>
//    private void UpdateBlurOff()
//    {
//        m_frameCount++;
//        m_depthOfField.focalLength.value -= m_focalLengthPerFrame;

//        if (m_frameCount == m_toFrame)
//        {
//            m_state = State.BlurOff;
//            m_frameCount = 0;
//        }
//    }

//    #endregion
}
