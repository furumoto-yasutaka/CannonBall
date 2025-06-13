/*******************************************************************************
*
*	タイトル：	セーブデータ管理シングルトンスクリプト
*	ファイル：	SaveDataManager.cs
*	作成者：	古本 泰隆
*	制作日：    2023/05/05
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using UniRx;

public class SaveDataManager : SingletonMonoBehaviour<SaveDataManager>
{
    #region struct

    /// <summary> セーブデータ構造体 </summary>
    public struct SaveData
    {
        /// <summary> テスト </summary>
        public int Test;
    }

    #endregion

    #region field

    /// <summary> セーブデータのファイル名 </summary>
    private string m_FileName = "SaveData";

    /// <summary> セーブデータを保存する際のパス </summary>
    private string m_FullPath;

    /// <summary> セーブデータ </summary>
    private ReactiveProperty<SaveData> m_saveData = new ReactiveProperty<SaveData>();

    /// <summary> セーブデータ(Presenter参照用) </summary>
    public IReadOnlyReactiveProperty<SaveData> m_SaveData => m_saveData;

    #endregion

    #region function

    /// <summary>
    /// 他のスクリプトの初期化で参照されるため、
    /// 最速で実行できるように優先度が-105に上がっています
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = 60;

        m_FullPath = Application.persistentDataPath + "/" + m_FileName + ".json";

        // セーブデータの有無
        if (File.Exists(m_FullPath))
        {
            // 存在したらロード
            Load();
        }
        else
        {
            // 存在していなかったら
            // デフォルト値で初期化してディレクトリを作成
            FormatSaveData();
            CreateDirectory();
            Save();
        }
    }

    /// <summary>
    /// ロード処理
    /// </summary>
    public void Load()
    {
        // ファイル読み込み
        StreamReader reader = new StreamReader(m_FullPath);
        string fileData = reader.ReadToEnd();
        reader.Close();

        // 文字列をセーブデータ構造体配列に変換して格納
        m_saveData.Value = JsonConvert.DeserializeObject<SaveData>(fileData);
    }

    /// <summary>
    /// セーブ処理
    /// </summary>
    public void Save()
    {
        // jsonファイルの設定
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;  // 循環参照するオブジェクトの警告を無視する
        settings.Formatting = Formatting.Indented;                      // 改行あり
        settings.Converters.Add(new StringEnumConverter(new DefaultNamingStrategy()));  // enumで文字列を記載

        // データをjson形式に変換
        string data = JsonConvert.SerializeObject(m_saveData.Value, settings);

        // ファイル書き込み
        StreamWriter writer = new StreamWriter(m_FullPath, false);
        writer.WriteLine(data);
        writer.Flush();
        writer.Close();
    }

    /// <summary>
    /// セーブデータ情報を初期化
    /// </summary>
    private void FormatSaveData()
    {

    }

    /// <summary>
    /// セーブデータ削除
    /// </summary>
    public void DeleteSaveData()
    {
        // セーブデータ初期化
        FormatSaveData();

        //// ★ゲーム側も初期化する
        //AudioManager.Instance.m_BgmVolume = m_SaveData.Value.Option.BgmVolume * 0.1f;
        //AudioManager.Instance.m_SeVolume = m_SaveData.Value.Option.SeVolume * 0.1f;

        // 初期化したデータでセーブする
        Save();
    }

    /// <summary>
    /// セーブデータ生成予定パスまでのフォルダを生成
    /// </summary>
    private void CreateDirectory()
    {
        // ディレクトリ名から最上層のディレクトリ名を抜いたパスを取得
        string remainPath = Path.GetDirectoryName(m_FullPath) + "/";
        // 確認対象のフォルダ
        string searchDirectoryPath = "";
        // 生成に使用するディレクトリ名
        string directoryName;

        // 全てのパスを確認するまで
        while (remainPath != "")
        {
            // 次のフォルダのパスを設定
            int index = GetIndexOfSlash(remainPath);
            directoryName = remainPath.Remove(index);
            searchDirectoryPath += "/" + directoryName;
            remainPath = remainPath.Remove(0, index + 1);

            // フォルダが存在しなかったら新しく生成する
            if (!Directory.Exists(searchDirectoryPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
                directoryInfo.Create();
            }
        }
    }

    /// <summary>
    /// スラッシュが文字列の何文字目にあるか取得する
    /// </summary>
    /// <param name="path"> 探索対象のパス </param>
    /// <returns> 見つかった場合は何文字目かを返し、見つからなかったら文字列の文字数を返す </returns>
    private int GetIndexOfSlash(string path)
    {
        int index = path.IndexOf("/");

        if (index != -1)
        {
            return index;
        }
        else
        {
            return path.Length;
        }
    }

    #endregion
}
