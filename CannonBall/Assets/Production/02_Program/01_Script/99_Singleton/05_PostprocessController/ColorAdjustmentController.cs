/*******************************************************************************
*
*	�^�C�g���F	�u���[����p�V���O���g���N���X
*	�t�@�C���F	BlurController.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/05/23
*	�X�V�����F�@
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class ColorAdjustmentController : SingletonMonoBehaviour<ColorAdjustmentController>
{
//    /// <summary> �|�X�g�G�t�F�N�g�̐F�����@�\�̃C���X�^���X </summary>
//    private ColorAdjustments m_colorAdjustments;

//    protected override void Awake()
//    {
//        dontDestroyOnLoad = false;

//        base.Awake();

//        if (!GetComponent<Volume>().profile.TryGet(out m_colorAdjustments))
//        {
//#if UNITY_EDITOR
//            Debug.LogError("DepthOfField��Volume�ɒǉ�����Ă��܂���");
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
