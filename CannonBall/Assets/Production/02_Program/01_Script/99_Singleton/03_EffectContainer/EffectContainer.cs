/*******************************************************************************
*
*	タイトル：	エフェクト管理シングルトンスクリプト
*	ファイル：	EffectContainer.cs
*	作成者：	古本 泰隆
*	制作日：    2023/09/17
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectContainer : SingletonMonoBehaviour<EffectContainer>
{
    [System.Serializable]
    public class EffectData
    {
        // 名前(インスペクターからの判別用)
        public string name = "";

        // エフェクト本体
        public GameObject Effect = null;
    }

    //=============================================================================
    //     変数
    //=============================================================================
    [SerializeField]
    private List<EffectData> EffectPrefabs = null;

    //=============================================================================
    //     スタート
    //=============================================================================
    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        // 継承元の呼び出し
        base.Awake();

        // エフェクトが登録されているかの判別
        bool registered = false;

        // エフェクトが登録されているかチェック
        for (int i = 0; i < EffectPrefabs.Count; i++)
        {
            if(!EffectPrefabs[i].Effect)
            {
                // エフェクトが登録されていない場合は削除して間を詰める
                EffectPrefabs.RemoveAt(i);
            }
            else
            {
                // 一つでも登録されている場合は登録を記録
                registered = true;
            }
        }

        // エフェクトが一つも登録されていない場合
        if (!registered)
        {
            Debug.Log(this.gameObject.name + " はエフェクトの登録が無いため削除されました");

            // オブジェクトを削除
            Destroy(this.gameObject);
        }
    }

    //=============================================================================
    //     エフェクトを再生
    //=============================================================================
    /// <summary> エフェクトを再生 </summary>
    /// <param name="name"> 再生するエフェクトの名前 </param>
    /// <param name="pos"> 再生するエフェクトの初期ポジション </param>
    /// <param name="rotation"> 再生するエフェクトの初期回転量 </param>
    /// <param name="parent"> ペアレント設定するオブジェクト </param>
    /// <param name="scaleRate"> 生成したオブジェクトのスケールに掛ける倍率 </param>
    public void EffectPlay(string name, Vector3 pos,
        Quaternion rotation = default, Transform parent = null, Vector3 scaleRate = default)
    {
        if (rotation == default)
        {
            rotation = Quaternion.identity;
        }

        GameObject obj = Instantiate(GetEffect(name), pos, rotation);
        if (scaleRate != default)
        {
            obj.transform.localScale = new Vector3(
                obj.transform.localScale.x * scaleRate.x,
                obj.transform.localScale.y * scaleRate.y,
                obj.transform.localScale.z * scaleRate.z);
        }
        ParticleSystem particle = obj.GetComponent<ParticleSystem>();
        particle.Play(true);
        particle.transform.SetParent(parent);
    }

    /// <summary> エフェクトを再生 </summary>
    /// <param name="refObj"> エフェクトを格納するオブジェクト </param>
    /// <param name="name"> 再生するエフェクトの名前 </param>
    /// <param name="pos"> 再生するエフェクトの初期ポジション </param>
    /// <param name="rotation"> 再生するエフェクトの初期回転量 </param>
    /// <param name="parent"> ペアレント設定するオブジェクト </param>
    /// <param name="scaleRate"> 生成したオブジェクトのスケールに掛ける倍率 </param>
    public void EffectPlay(ref GameObject refObj, string name, Vector3 pos,
        Quaternion rotation = default, Transform parent = null, Vector3 scaleRate = default)
    {
        if (rotation == default)
        {
            rotation = Quaternion.identity;
        }

        refObj = Instantiate(GetEffect(name), pos, rotation);
        if (scaleRate != default)
        {
            refObj.transform.localScale = new Vector3(
                refObj.transform.localScale.x * scaleRate.x,
                refObj.transform.localScale.y * scaleRate.y,
                refObj.transform.localScale.z * scaleRate.z);
        }
        ParticleSystem particle = refObj.GetComponent<ParticleSystem>();
        particle.Play(true);
        particle.transform.SetParent(parent);
    }

    /// <summary> エフェクトを再生 </summary>
    /// <param name="particle"> パーティクル設定を格納するオブジェクト </param>
    /// <param name="name"> 再生するエフェクトの名前 </param>
    /// <param name="pos"> 再生するエフェクトの初期ポジション </param>
    /// <param name="rotation"> 再生するエフェクトの初期回転量 </param>
    /// <param name="parent"> ペアレント設定するオブジェクト </param>
    /// <param name="scaleRate"> 生成したオブジェクトのスケールに掛ける倍率 </param>
    public void EffectPlay(ref ParticleSystem particle, string name, Vector3 pos,
        Quaternion rotation = default, Transform parent = null, Vector3 scaleRate = default)
    {
        if (rotation == default)
        {
            rotation = Quaternion.identity;
        }

        GameObject obj = Instantiate(GetEffect(name), pos, rotation);
        if (scaleRate != default)
        {
            obj.transform.localScale = new Vector3(
                obj.transform.localScale.x * scaleRate.x,
                obj.transform.localScale.y * scaleRate.y,
                obj.transform.localScale.z * scaleRate.z);
        }
        particle = obj.GetComponent<ParticleSystem>();
        particle.Play(true);
        particle.transform.SetParent(parent);
    }

    //=============================================================================
    //     名前からエフェクトを検索
    //=============================================================================
    /// <summary>
    /// 名前からエフェクトを検索
    /// </summary>
    /// <param name="name">エフェクトの名前</param>
    private int GetIndex(string name)
    {
        // ループ回数
        int loopIndex = 0;

        // エフェクト一覧の中から該当する名前のエフェクトを検索
        foreach(EffectData effect in EffectPrefabs)
        {
            // 検索関数を用いて合致した場合
            if(FindTitleMatch(effect, name))
            {
                // ループ回数を戻り値で返す
                return loopIndex;
            }

            // ループ回数加算処理
            loopIndex++;
        }

        // 検索で該当のファイルが見つからなかった場合
        Debug.LogError("指定された名前のファイルは登録されていません");

        return -1;
    }

    //=============================================================================
    //     名前を入れてエフェクトを取得
    //=============================================================================
    /// <summary>
    /// 名前を入れてエフェクトを取得
    /// </summary>
    /// <param name="name">エフェクトの名前</param>
    public GameObject GetEffect(string name)
    {
        // 名前が無効の場合は終了
        if (name == null)
        {
            return null;
        }

        // 名前からインデックスを検索
        int index = GetIndex(name);

        // BGMの指定にエラーが起こった場合は終了
        if (index < 0 || EffectPrefabs.Count <= index)
        {
            return null;
        }

        // エフェクトのプレハブを返す
        return EffectPrefabs[index].Effect;
    }

    //=============================================================================
    //     名前の一致を検索
    //=============================================================================
    /// <summary> エフェクト名もしくはファイル名と一致するか判定</summary>
    /// <param name="Data"> エフェクトデータ </param>
    /// <param name="name"> 判定する名前 </param>
    /// <returns> エフェクト名もしくはタイトル名と一致したらtrueを返す </returns>
    private bool FindTitleMatch(EffectData Data, string name)
    {
        if (Data.name.Equals(name) || Data.Effect.name.Equals(name)) { return true; }

        return false;
    }
}
