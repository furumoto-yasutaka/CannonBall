using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeVolumeSliderListener : SliderListener
{
    protected override void Awake()
    {
        base.Awake();
        base.ChangeStep((int)(AudioMasterVolume.Instance.m_SeVolume * 10));
    }

    /// <summary>
    /// �X���C�_�[�̒i�K�ύX
    /// </summary>
    protected override void ChangeStep(int step)
    {
        base.ChangeStep(step);
        AudioMasterVolume.Instance.m_SeVolume = (float)m_step.Value / 10;
    }
}
