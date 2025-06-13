/*******************************************************************************
*
*	タイトル：	ブラー制御用シングルトンクラス
*	ファイル：	BlurController.cs
*	作成者：	古本 泰隆
*	制作日：    2023/05/23
*	更新履歴：　
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class ColorAdjustmentController : SingletonMonoBehaviour<ColorAdjustmentController>
{
//    /// <summary> ポストエフェクトの色調整機能のインスタンス </summary>
//    private ColorAdjustments m_colorAdjustments;

//    protected override void Awake()
//    {
//        dontDestroyOnLoad = false;

//        base.Awake();

//        if (!GetComponent<Volume>().profile.TryGet(out m_colorAdjustments))
//        {
//#if UNITY_EDITOR
//            Debug.LogError("DepthOfFieldがVolumeに追加されていません");
//#endif
//        }
//        else
//        {
//            SetBrightness(SaveDataManager.Instance.m_SaveData.Value.Option.Brightness);
//        }
//    }

//    private void Start()
//    {
//        SetBrightness(SaveDataManager.Instance.m_SaveData.Value.Option.Brightness);
//    }

//    public void SetBrightness(float value)
//    {
//        m_colorAdjustments.postExposure.value = value;
//    }

//    public void SetBrightness(int step)
//    {
//        m_colorAdjustments.postExposure.value = (step - 5) * 0.2f + 0.1f;
//    }
}
