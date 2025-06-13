/*******************************************************************************
*
*	タイトル：	オーディオ管理シングルトンスクリプト
*	ファイル：	AudioManager.cs
*	作成者：	古本 泰隆
*	制作日：    2023/05/22
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
        // オーディオファイル
        public AudioClip m_Clip = null;

        // ボリューム
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
        // 音名(ファイル名とは別)
        public string m_Name = "";

        // オーディオファイル
        public AudioClip m_Clip = null;

        // ボリューム
        public float m_Volume = 1.0f;
    }

    #endregion

    #region field

    // オーディオデータ
    // キー   string型     音名(ファイル名とは別)
    // 要素   AudioInfo型  音情報
    private Dictionary<string, AudioInfo> m_bgmInfo = new Dictionary<string, AudioInfo>();
    private Dictionary<string, AudioInfo> m_seInfo = new Dictionary<string, AudioInfo>();

    // インスペクター入力用リスト
    [SerializeField]
    private List<AudioInfoInspector> m_bgmInfoInspector = new List<AudioInfoInspector>();
    [SerializeField]
    private List<AudioInfoInspector> m_seInfoInspector = new List<AudioInfoInspector>();

    #endregion

    #region function

    /// <summary> OnValidateを実行可能かどうか </summary>
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

        // ★セーブデータから取得
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
    //     BGM関連
    //=============================================================================

    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="name"> 音名(Dictionaryのキー情報) </param>
    /// <param name="loop"> ループするか </param>
    /// <param name="volume"> 再生音量倍率 </param>
    public void PlayBgm(string name, bool loop, float volume = 1.0f)
    {
        // 存在しない名前なら終了
        if (!m_bgmInfo.ContainsKey(name)) { return; }

        // 再生に使用するSourcesを取得
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
    /// BGM再生
    /// </summary>
    /// <param name="name"> 音名(Dictionaryのキー情報) </param>
    /// <param name="source"> 再生に使用するコンポーネントを受け取るための参照型 </param>
    /// <param name="loop"> ループするか </param>
    /// <param name="volume"> 再生音量倍率 </param>
    public void PlayBgm(string name, ref AudioSource source, bool loop, float volume = 1.0f)
    {
        // 存在しない名前なら終了
        if (!m_bgmInfo.ContainsKey(name)) { return; }

        // 再生処理
        AudioInfo info = m_bgmInfo[name];
        source.clip = info.m_Clip;
        source.loop = loop;
        source.volume = AudioMasterVolume.Instance.m_BgmVolume * info.m_Volume * volume;
        source.Play();
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    /// <param name="name"> 音名(Dictionaryのキー情報) </param>
    public void StopBgm(string name)
    {
        // 名前から停止するBGMを検索する
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
    /// 全BGM停止
    /// </summary>
    public void StopBgmAll()
    {
        int count = AudioMasterVolume.Instance.m_bgmUsingSources.Count;

        for (int i = 0; i < count; i++)
        {
            // 停止と初期化
            AudioMasterVolume.Instance.m_bgmUsingSources[0].m_source.Stop();
            AudioMasterVolume.Instance.m_bgmUsingSources[0].m_source.clip = null;

            // 未使用リストに移動
            AudioMasterVolume.Instance.m_bgmSources.Add(AudioMasterVolume.Instance.m_bgmUsingSources[0].m_source);
            AudioMasterVolume.Instance.m_bgmUsingSources.RemoveAt(0);
        }
    }

    /// <summary>
    /// BGM一時停止
    /// </summary>
    /// <param name="name"> 音名(Dictionaryのキー情報) </param>
    public void PauseBgm(string name)
    {
        // 名前から一時停止するBGMを検索する
        AudioMasterVolume.UsingSource source = BgmSourceNameToNumber(name);

        if (source != null)
        {
            source.m_source.Pause();
        }
    }

    /// <summary>
    /// 全BGMミュート
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
    /// 全BGMミュート解除
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
    /// BGM再生再開
    /// </summary>
    /// <param name="name"> 音名(Dictionaryのキー情報) </param>
    public void ResumeBgm(string name)
    {
        // 名前から一時停止するBGMを検索する
        AudioMasterVolume.UsingSource source = BgmSourceNameToNumber(name);

        if (source != null)
        {
            source.m_source.Play();
        }
    }

    /// <summary>
    /// 指定した名前のBGMを再生しているAudioSourceを取得
    /// </summary>
    /// <param name="name"> 音名(Dictionaryのキー情報) </param>
    private AudioMasterVolume.UsingSource BgmSourceNameToNumber(string name)
    {
        // 存在しない名前なら終了
        if (!m_bgmInfo.ContainsKey(name)) { return null; }

        AudioClip clip = m_bgmInfo[name].m_Clip;

        // ファイルを比較して探す
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
    /// 再生中の全BGM用Sourceの音量を変更
    /// </summary>
    /// <param name="rate"> 元の音量に対する新しい音量の比率 </param>
    public void SetBgmUsingSourceVolume(float volume)
    {
        foreach (AudioMasterVolume.UsingSource s in AudioMasterVolume.Instance.m_bgmUsingSources)
        {
            s.m_source.volume = volume * s.m_originVolume;
        }
    }

    /// <summary>
    /// 再生中の全BGM用Sourceの音量を変更
    /// </summary>
    /// <param name="rate"> 元の音量に対する新しい音量の比率 </param>
    public void SetBgmUsingSourceVolumeRate(float rate)
    {
        foreach (AudioMasterVolume.UsingSource s in AudioMasterVolume.Instance.m_bgmUsingSources)
        {
            s.m_source.volume *= rate;
        }
    }


    //=============================================================================
    //     SE関連
    //=============================================================================

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="name"> 音名(Dictionaryのキー情報) </param>
    /// <param name="loop"> ループするか </param>
    public void PlaySe(string name, bool loop, float volume = 1.0f)
    {
        // 存在しない名前なら終了
        if (!m_seInfo.ContainsKey(name)) { return; }

        // 再生に使用するSourcesを取得
        AudioSource source;
        AudioInfo info = m_seInfo[name];

        // 未使用のSourceがあるか
        if (AudioMasterVolume.Instance.m_seSources.Count != 0)
        {
            // あればそれを使う
            source = AudioMasterVolume.Instance.m_seSources[0];

            AudioMasterVolume.Instance.m_seSources.RemoveAt(0);
            AudioMasterVolume.Instance.m_seUsingSources.Add(new AudioMasterVolume.UsingSource(source, info.m_Volume));
        }
        else
        {
            // 無ければ現在再生中のSEの中で一番古いSourceから再生
            source = AudioMasterVolume.Instance.m_seUsingSources[0].m_source;
            AudioMasterVolume.Instance.m_seUsingSources[0].m_originVolume = info.m_Volume;
        }

        source.clip = info.m_Clip;
        source.loop = loop;
        source.volume = AudioMasterVolume.Instance.m_SeVolume * info.m_Volume * volume;
        source.Play();
    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="name"> 音名(Dictionaryのキー情報) </param>
    /// <param name="loop"> ループするか </param>
    public void PlaySe(string name, ref AudioSource source, bool loop, float volume = 1.0f)
    {
        // 存在しない名前なら終了
        if (!m_seInfo.ContainsKey(name)) { return; }

        AudioInfo info = m_seInfo[name];
        source.clip = info.m_Clip;
        source.loop = loop;
        source.volume = AudioMasterVolume.Instance.m_SeVolume * info.m_Volume * volume;
        source.Play();
    }

    /// <summary>
    /// SE停止
    /// </summary>
    /// <param name="name"> 音名(Dictionaryのキー情報) </param>
    public void StopSe(string name)
    {
        // 名前から停止するBGMを検索する
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
    /// 全SE停止
    /// </summary>
    public void StopSeAll()
    {
        // 再生中のSourceの数を保存
        // (リストから削除していくため)
        int count = AudioMasterVolume.Instance.m_seUsingSources.Count;

        for (int i = 0; i < count; i++)
        {
            // 停止と初期化
            AudioMasterVolume.Instance.m_seUsingSources[0].m_source.Stop();
            AudioMasterVolume.Instance.m_seUsingSources[0].m_source.clip = null;

            // 未使用リストに移動
            AudioMasterVolume.Instance.m_seSources.Add(AudioMasterVolume.Instance.m_seUsingSources[0].m_source);
            AudioMasterVolume.Instance.m_seUsingSources.RemoveAt(0);
        }
    }

    /// <summary>
    /// 全SEミュート
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
    /// 全SEミュート解除
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
    /// 指定した名前のSEを再生しているAudioSourceを取得
    /// </summary>
    /// <param name="name"> 音名(Dictionaryのキー情報) </param>
    private AudioMasterVolume.UsingSource SeSourceNameToNumber(string name)
    {
        // 存在しない名前なら終了
        if (!m_seInfo.ContainsKey(name)) { return null; }

        AudioClip clip = m_seInfo[name].m_Clip;

        // ファイルを比較して探す
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
    /// 再生中の全SE用Sourceの音量を変更
    /// </summary>
    /// <param name="volume"> 新しい音量 </param>
    public void SetSeUsingSourceVolume(float volume)
    {
        foreach (AudioMasterVolume.UsingSource s in AudioMasterVolume.Instance.m_seUsingSources)
        {
            s.m_source.volume = volume * s.m_originVolume;
        }
    }

    /// <summary>
    /// 再生中の全SE用Sourceの音量を変更
    /// </summary>
    /// <param name="rate"> 元の音量に対する新しい音量の比率 </param>
    public void SetSeUsingSourceVolumeRate(float rate)
    {
        foreach (AudioMasterVolume.UsingSource s in AudioMasterVolume.Instance.m_seUsingSources)
        {
            s.m_source.volume *= rate;
        }
    }

    #endregion
}
