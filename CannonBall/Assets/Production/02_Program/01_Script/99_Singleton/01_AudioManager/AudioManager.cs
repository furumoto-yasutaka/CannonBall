/*******************************************************************************
*
*	�^�C�g���F	�I�[�f�B�I�Ǘ��V���O���g���X�N���v�g
*	�t�@�C���F	AudioManager.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/05/22
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    #region support class

    public class AudioInfo
    {
        // �I�[�f�B�I�t�@�C��
        public AudioClip m_Clip = null;

        // �{�����[��
        public float m_Volume = 1.0f;

        public AudioInfo(AudioClip clip, float volume)
        {
            m_Clip = clip;
            m_Volume = volume;
        }
    }

    [System.Serializable]
    public class AudioInfoInspector
    {
        // ����(�t�@�C�����Ƃ͕�)
        public string m_Name = "";

        // �I�[�f�B�I�t�@�C��
        public AudioClip m_Clip = null;

        // �{�����[��
        public float m_Volume = 1.0f;
    }

    #endregion

    #region field

    // �I�[�f�B�I�f�[�^
    // �L�[   string�^     ����(�t�@�C�����Ƃ͕�)
    // �v�f   AudioInfo�^  �����
    private Dictionary<string, AudioInfo> m_bgmInfo = new Dictionary<string, AudioInfo>();
    private Dictionary<string, AudioInfo> m_seInfo = new Dictionary<string, AudioInfo>();

    // �C���X�y�N�^�[���͗p���X�g
    [SerializeField]
    private List<AudioInfoInspector> m_bgmInfoInspector = new List<AudioInfoInspector>();
    [SerializeField]
    private List<AudioInfoInspector> m_seInfoInspector = new List<AudioInfoInspector>();

    #endregion

    #region function

    /// <summary> OnValidate�����s�\���ǂ��� </summary>
    private bool m_isCanOnValidate = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!EditorApplication.isPlaying || !m_isCanOnValidate)
        {
            return;
        }

        foreach (AudioInfoInspector info in m_bgmInfoInspector)
        {
            m_bgmInfo[info.m_Name].m_Volume = info.m_Volume;
        }
        foreach (AudioInfoInspector info in m_seInfoInspector)
        {
            m_seInfo[info.m_Name].m_Volume = info.m_Volume;
        }
    }
#endif

    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();

        // ���Z�[�u�f�[�^����擾
        //m_bgmVolume = SaveDataManager.Instance.m_SaveData.Value.Option.BgmVolume * 0.1f;
        //m_seVolume = SaveDataManager.Instance.m_SaveData.Value.Option.SeVolume * 0.1f;

        foreach (AudioInfoInspector info in m_bgmInfoInspector)
        {
            m_bgmInfo.Add(info.m_Name, new AudioInfo(info.m_Clip, info.m_Volume));
        }
        foreach (AudioInfoInspector info in m_seInfoInspector)
        {
            m_seInfo.Add(info.m_Name, new AudioInfo(info.m_Clip, info.m_Volume));
        }

        m_isCanOnValidate = true;
    }

    private void Update()
    {
        int i = 0;
        while (i < AudioMasterVolume.Instance.m_bgmUsingSources.Count)
        {
            if (!AudioMasterVolume.Instance.m_bgmUsingSources[i].m_source.isPlaying)
            {
                AudioMasterVolume.Instance.m_bgmSources.Add(AudioMasterVolume.Instance.m_bgmUsingSources[i].m_source);
                AudioMasterVolume.Instance.m_bgmUsingSources.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }

        i = 0;
        while (i < AudioMasterVolume.Instance.m_seUsingSources.Count)
        {
            if (!AudioMasterVolume.Instance.m_seUsingSources[i].m_source.isPlaying)
            {
                AudioMasterVolume.Instance.m_seSources.Add(AudioMasterVolume.Instance.m_seUsingSources[i].m_source);
                AudioMasterVolume.Instance.m_seUsingSources.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
    }

    //=============================================================================
    //     BGM�֘A
    //=============================================================================

    /// <summary>
    /// BGM�Đ�
    /// </summary>
    /// <param name="name"> ����(Dictionary�̃L�[���) </param>
    /// <param name="loop"> ���[�v���邩 </param>
    /// <param name="volume"> �Đ����ʔ{�� </param>
    public void PlayBgm(string name, bool loop, float volume = 1.0f)
    {
        // ���݂��Ȃ����O�Ȃ�I��
        if (!m_bgmInfo.ContainsKey(name)) { return; }

        // �Đ��Ɏg�p����Sources���擾
        AudioSource source;
        AudioInfo info = m_bgmInfo[name];

        if (AudioMasterVolume.Instance.m_bgmSources.Count != 0)
        {
            source = AudioMasterVolume.Instance.m_bgmSources[0];

            AudioMasterVolume.Instance.m_bgmSources.RemoveAt(0);
            AudioMasterVolume.Instance.m_bgmUsingSources.Add(new AudioMasterVolume.UsingSource(source, info.m_Volume));
        }
        else
        {
            source = AudioMasterVolume.Instance.m_bgmUsingSources[0].m_source;
            AudioMasterVolume.Instance.m_bgmUsingSources[0].m_originVolume = info.m_Volume;
        }

        source.clip = info.m_Clip;
        source.loop = loop;
        source.volume = AudioMasterVolume.Instance.m_BgmVolume * info.m_Volume * volume;
        source.Play();
    }

    /// <summary>
    /// BGM�Đ�
    /// </summary>
    /// <param name="name"> ����(Dictionary�̃L�[���) </param>
    /// <param name="source"> �Đ��Ɏg�p����R���|�[�l���g���󂯎�邽�߂̎Q�ƌ^ </param>
    /// <param name="loop"> ���[�v���邩 </param>
    /// <param name="volume"> �Đ����ʔ{�� </param>
    public void PlayBgm(string name, ref AudioSource source, bool loop, float volume = 1.0f)
    {
        // ���݂��Ȃ����O�Ȃ�I��
        if (!m_bgmInfo.ContainsKey(name)) { return; }

        // �Đ�����
        AudioInfo info = m_bgmInfo[name];
        source.clip = info.m_Clip;
        source.loop = loop;
        source.volume = AudioMasterVolume.Instance.m_BgmVolume * info.m_Volume * volume;
        source.Play();
    }

    /// <summary>
    /// BGM��~
    /// </summary>
    /// <param name="name"> ����(Dictionary�̃L�[���) </param>
    public void StopBgm(string name)
    {
        // ���O�����~����BGM����������
        AudioMasterVolume.UsingSource source = BgmSourceNameToNumber(name);

        if (source != null)
        {
            source.m_source.Stop();
            source.m_source.clip = null;

            AudioMasterVolume.Instance.m_bgmSources.Add(source.m_source);
            AudioMasterVolume.Instance.m_bgmUsingSources.Remove(source);
        }
    }

    /// <summary>
    /// �SBGM��~
    /// </summary>
    public void StopBgmAll()
    {
        int count = AudioMasterVolume.Instance.m_bgmUsingSources.Count;

        for (int i = 0; i < count; i++)
        {
            // ��~�Ə�����
            AudioMasterVolume.Instance.m_bgmUsingSources[0].m_source.Stop();
            AudioMasterVolume.Instance.m_bgmUsingSources[0].m_source.clip = null;

            // ���g�p���X�g�Ɉړ�
            AudioMasterVolume.Instance.m_bgmSources.Add(AudioMasterVolume.Instance.m_bgmUsingSources[0].m_source);
            AudioMasterVolume.Instance.m_bgmUsingSources.RemoveAt(0);
        }
    }

    /// <summary>
    /// BGM�ꎞ��~
    /// </summary>
    /// <param name="name"> ����(Dictionary�̃L�[���) </param>
    public void PauseBgm(string name)
    {
        // ���O����ꎞ��~����BGM����������
        AudioMasterVolume.UsingSource source = BgmSourceNameToNumber(name);

        if (source != null)
        {
            source.m_source.Pause();
        }
    }

    /// <summary>
    /// �SBGM�~���[�g
    /// </summary>
    public void MuteBgmAll()
    {
        foreach (AudioSource s in AudioMasterVolume.Instance.m_bgmSources)
        {
            s.mute = true;
        }
        foreach (AudioMasterVolume.UsingSource s in AudioMasterVolume.Instance.m_bgmUsingSources)
        {
            s.m_source.mute = true;
        }
    }

    /// <summary>
    /// �SBGM�~���[�g����
    /// </summary>
    public void UnmuteBgmAll()
    {
        foreach (AudioSource s in AudioMasterVolume.Instance.m_bgmSources)
        {
            s.mute = false;
        }
        foreach (AudioMasterVolume.UsingSource s in AudioMasterVolume.Instance.m_bgmUsingSources)
        {
            s.m_source.mute = false;
        }
    }

    /// <summary>
    /// BGM�Đ��ĊJ
    /// </summary>
    /// <param name="name"> ����(Dictionary�̃L�[���) </param>
    public void ResumeBgm(string name)
    {
        // ���O����ꎞ��~����BGM����������
        AudioMasterVolume.UsingSource source = BgmSourceNameToNumber(name);

        if (source != null)
        {
            source.m_source.Play();
        }
    }

    /// <summary>
    /// �w�肵�����O��BGM���Đ����Ă���AudioSource���擾
    /// </summary>
    /// <param name="name"> ����(Dictionary�̃L�[���) </param>
    private AudioMasterVolume.UsingSource BgmSourceNameToNumber(string name)
    {
        // ���݂��Ȃ����O�Ȃ�I��
        if (!m_bgmInfo.ContainsKey(name)) { return null; }

        AudioClip clip = m_bgmInfo[name].m_Clip;

        // �t�@�C�����r���ĒT��
        foreach (AudioMasterVolume.UsingSource s in AudioMasterVolume.Instance.m_bgmUsingSources)
        {
            if (s.m_source.clip == clip)
            {
                return s;
            }
        }

        return null;
    }

    /// <summary>
    /// �Đ����̑SBGM�pSource�̉��ʂ�ύX
    /// </summary>
    /// <param name="rate"> ���̉��ʂɑ΂���V�������ʂ̔䗦 </param>
    public void SetBgmUsingSourceVolume(float volume)
    {
        foreach (AudioMasterVolume.UsingSource s in AudioMasterVolume.Instance.m_bgmUsingSources)
        {
            s.m_source.volume = volume * s.m_originVolume;
        }
    }

    /// <summary>
    /// �Đ����̑SBGM�pSource�̉��ʂ�ύX
    /// </summary>
    /// <param name="rate"> ���̉��ʂɑ΂���V�������ʂ̔䗦 </param>
    public void SetBgmUsingSourceVolumeRate(float rate)
    {
        foreach (AudioMasterVolume.UsingSource s in AudioMasterVolume.Instance.m_bgmUsingSources)
        {
            s.m_source.volume *= rate;
        }
    }


    //=============================================================================
    //     SE�֘A
    //=============================================================================

    /// <summary>
    /// SE�Đ�
    /// </summary>
    /// <param name="name"> ����(Dictionary�̃L�[���) </param>
    /// <param name="loop"> ���[�v���邩 </param>
    public void PlaySe(string name, bool loop, float volume = 1.0f)
    {
        // ���݂��Ȃ����O�Ȃ�I��
        if (!m_seInfo.ContainsKey(name)) { return; }

        // �Đ��Ɏg�p����Sources���擾
        AudioSource source;
        AudioInfo info = m_seInfo[name];

        // ���g�p��Source�����邩
        if (AudioMasterVolume.Instance.m_seSources.Count != 0)
        {
            // ����΂�����g��
            source = AudioMasterVolume.Instance.m_seSources[0];

            AudioMasterVolume.Instance.m_seSources.RemoveAt(0);
            AudioMasterVolume.Instance.m_seUsingSources.Add(new AudioMasterVolume.UsingSource(source, info.m_Volume));
        }
        else
        {
            // ������Ό��ݍĐ�����SE�̒��ň�ԌÂ�Source����Đ�
            source = AudioMasterVolume.Instance.m_seUsingSources[0].m_source;
            AudioMasterVolume.Instance.m_seUsingSources[0].m_originVolume = info.m_Volume;
        }

        source.clip = info.m_Clip;
        source.loop = loop;
        source.volume = AudioMasterVolume.Instance.m_SeVolume * info.m_Volume * volume;
        source.Play();
    }

    /// <summary>
    /// SE�Đ�
    /// </summary>
    /// <param name="name"> ����(Dictionary�̃L�[���) </param>
    /// <param name="loop"> ���[�v���邩 </param>
    public void PlaySe(string name, ref AudioSource source, bool loop, float volume = 1.0f)
    {
        // ���݂��Ȃ����O�Ȃ�I��
        if (!m_seInfo.ContainsKey(name)) { return; }

        AudioInfo info = m_seInfo[name];
        source.clip = info.m_Clip;
        source.loop = loop;
        source.volume = AudioMasterVolume.Instance.m_SeVolume * info.m_Volume * volume;
        source.Play();
    }

    /// <summary>
    /// SE��~
    /// </summary>
    /// <param name="name"> ����(Dictionary�̃L�[���) </param>
    public void StopSe(string name)
    {
        // ���O�����~����BGM����������
        AudioMasterVolume.UsingSource source = SeSourceNameToNumber(name);

        if (source != null)
        {
            source.m_source.Stop();
            source.m_source.clip = null;

            AudioMasterVolume.Instance.m_seSources.Add(source.m_source);
            AudioMasterVolume.Instance.m_seUsingSources.Remove(source);
        }
    }

    /// <summary>
    /// �SSE��~
    /// </summary>
    public void StopSeAll()
    {
        // �Đ�����Source�̐���ۑ�
        // (���X�g����폜���Ă�������)
        int count = AudioMasterVolume.Instance.m_seUsingSources.Count;

        for (int i = 0; i < count; i++)
        {
            // ��~�Ə�����
            AudioMasterVolume.Instance.m_seUsingSources[0].m_source.Stop();
            AudioMasterVolume.Instance.m_seUsingSources[0].m_source.clip = null;

            // ���g�p���X�g�Ɉړ�
            AudioMasterVolume.Instance.m_seSources.Add(AudioMasterVolume.Instance.m_seUsingSources[0].m_source);
            AudioMasterVolume.Instance.m_seUsingSources.RemoveAt(0);
        }
    }

    /// <summary>
    /// �SSE�~���[�g
    /// </summary>
    public void MuteSeAll()
    {
        foreach (AudioSource s in AudioMasterVolume.Instance.m_seSources)
        {
            s.mute = true;
        }
        foreach (AudioMasterVolume.UsingSource s in AudioMasterVolume.Instance.m_seUsingSources)
        {
            s.m_source.mute = true;
        }
    }

    /// <summary>
    /// �SSE�~���[�g����
    /// </summary>
    public void UnmuteSeAll()
    {
        foreach (AudioSource s in AudioMasterVolume.Instance.m_seSources)
        {
            s.mute = false;
        }
        foreach (AudioMasterVolume.UsingSource s in AudioMasterVolume.Instance.m_seUsingSources)
        {
            s.m_source.mute = false;
        }
    }

    /// <summary>
    /// �w�肵�����O��SE���Đ����Ă���AudioSource���擾
    /// </summary>
    /// <param name="name"> ����(Dictionary�̃L�[���) </param>
    private AudioMasterVolume.UsingSource SeSourceNameToNumber(string name)
    {
        // ���݂��Ȃ����O�Ȃ�I��
        if (!m_seInfo.ContainsKey(name)) { return null; }

        AudioClip clip = m_seInfo[name].m_Clip;

        // �t�@�C�����r���ĒT��
        foreach (AudioMasterVolume.UsingSource s in AudioMasterVolume.Instance.m_seUsingSources)
        {
            if (s.m_source.clip == clip)
            {
                return s;
            }
        }

        return null;
    }

    /// <summary>
    /// �Đ����̑SSE�pSource�̉��ʂ�ύX
    /// </summary>
    /// <param name="volume"> �V�������� </param>
    public void SetSeUsingSourceVolume(float volume)
    {
        foreach (AudioMasterVolume.UsingSource s in AudioMasterVolume.Instance.m_seUsingSources)
        {
            s.m_source.volume = volume * s.m_originVolume;
        }
    }

    /// <summary>
    /// �Đ����̑SSE�pSource�̉��ʂ�ύX
    /// </summary>
    /// <param name="rate"> ���̉��ʂɑ΂���V�������ʂ̔䗦 </param>
    public void SetSeUsingSourceVolumeRate(float rate)
    {
        foreach (AudioMasterVolume.UsingSource s in AudioMasterVolume.Instance.m_seUsingSources)
        {
            s.m_source.volume *= rate;
        }
    }

    #endregion
}
