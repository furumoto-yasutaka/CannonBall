using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmVolumeSliderListener : SliderListener
{
    protected override void Awake()
    {
        base.Awake();
        base.ChangeStep((int)(AudioMasterVolume.Instance.m_BgmVolume * 10));
    }

    /// <summary>
    /// �X���C�_�[�̒i�K�ύX
    /// </summary>
    protected override void ChangeStep(int step)
    {
        base.ChangeStep(step);
        AudioMasterVolume.Instance.m_BgmVolume = (float)m_step.Value / 10;
    }
}
