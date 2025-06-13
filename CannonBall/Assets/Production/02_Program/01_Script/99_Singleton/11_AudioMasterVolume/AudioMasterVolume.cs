using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMasterVolume : SingletonMonoBehaviour<AudioMasterVolume>
{
    public class UsingSource
    {
        public AudioSource m_source;

        public float m_originVolume;

        public UsingSource(AudioSource source, float volume)
        {
            m_source = source;
            m_originVolume = volume;
        }
    }


    [SerializeField, CustomLabel("BGM音量"), Range(0, 5)]
    private float m_bgmVolume = 1.0f;

    [SerializeField, CustomLabel("SE音量"), Range(0, 5)]
    private float m_seVolume = 1.0f;

    [SerializeField, CustomLabel("BGMの同時再生可能数"), Range(0, 3)]
    private int m_bgmPlaybackNum = 3;

    [SerializeField, CustomLabel("SEの同時再生可能数"), Range(0, 30)]
    private int m_sePlaybackNum = 20;

    [SerializeField, CustomLabel("ミュート(BGM)")]
    private bool m_isMuteBgm = false;

    [SerializeField, CustomLabel("ミュート(SE)")]
    private bool m_isMuteSe = false;

    // 未使用のAudioSource
    public List<AudioSource> m_bgmSources = new List<AudioSource>();
    public List<AudioSource> m_seSources = new List<AudioSource>();

    // 使用中のAudioSource
    public List<UsingSource> m_bgmUsingSources = new List<UsingSource>();
    public List<UsingSource> m_seUsingSources = new List<UsingSource>();


    public float m_BgmVolume
    {
        get { return m_bgmVolume; }
        set
        {
            if (m_bgmVolume == 0.0f)
            {
                AudioManager.Instance.SetBgmUsingSourceVolume(value);
            }
            else
            {
                float rate = value / m_bgmVolume;
                AudioManager.Instance.SetBgmUsingSourceVolumeRate(rate);
            }
            m_bgmVolume = Mathf.Clamp01(value);
        }
    }

    public float m_SeVolume
    {
        get { return m_seVolume; }
        set
        {
            if (m_seVolume == 0.0f)
            {
                AudioManager.Instance.SetSeUsingSourceVolume(value);
            }
            else
            {
                float rate = value / m_seVolume;
                AudioManager.Instance.SetSeUsingSourceVolumeRate(rate);
            }
            m_seVolume = Mathf.Clamp01(value);
        }
    }

    public float m_BgmPlaybackNum { get { return m_bgmPlaybackNum; } }

    public float m_SePlaybackNum { get { return m_sePlaybackNum; } }

    public bool m_IsMuteBgm
    {
        get { return m_isMuteBgm; }
        set
        {
            m_isMuteBgm = value;

            if (value)
            {
                AudioManager.Instance.MuteBgmAll();
            }
            else
            {
                AudioManager.Instance.UnmuteBgmAll();
            }
        }
    }

    public bool m_IsMuteSe
    {
        get { return m_isMuteSe; }
        set
        {
            m_isMuteSe = value;

            if (value)
            {
                AudioManager.Instance.MuteSeAll();
            }
            else
            {
                AudioManager.Instance.UnmuteSeAll();
            }
        }
    }


    protected override void Awake()
    {
        base.Awake();

        // BGM用にAudioSourceコンポーネントを取得
        for (int i = 0; i < m_BgmPlaybackNum; i++)
        {
            // AudioSourceを追加
            m_bgmSources.Add(gameObject.AddComponent<AudioSource>());

            // 初期ボリュームをセット
            m_bgmSources[i].volume = m_BgmVolume;
        }

        // SE用にAudioSourceコンポーネントを取得
        for (int i = 0; i < m_SePlaybackNum; i++)
        {
            // AudioSourceを追加
            m_seSources.Add(gameObject.AddComponent<AudioSource>());

            // 初期ボリュームをセット
            m_seSources[i].volume = m_SeVolume;
        }
    }
}
