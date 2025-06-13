/*******************************************************************************
*
*	タイトル：	抽象入力名のEnum生成スクリプト
*	ファイル：	NewInputActionNameCreator.cs
*	作成者：	古本 泰隆
*	制作日：    2023/09/24
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NewInputActionNameCreator : MonoBehaviour
{
    // 無効な文字を管理する配列
    private static readonly string[] m_InvaludChars =
    {
        " ", "!", "\"", "#", "$",
        "%", "&", "\'", "(", ")",
        "-", "=", "^",  "~", "\\",
        "|", "[", "{",  "@", "`",
        "]", "}", ":",  "*", ";",
        "+", "/", "?",  ".", ">",
        "<", ",",
    };

    /// <summary> Unityのどこで操作できるようにするかのパス </summary>
    private const string m_ItemName = "Tools/Create/NewInputAction Name Enum";

    /// <summary> ファイルパス </summary>
    private const string m_FilePath
        = "Assets/Production/02_Program/01_Script/97_System/01_NewInputActionName/NewInputActionNameEnum.cs";

    /// <summary> ファイル名(拡張子込み) </summary>
    private static readonly string m_FileName = Path.GetFileName(m_FilePath);

    /// <summary> ファイル名(拡張子抜き) </summary>
    private static readonly string m_FileNameWithoutExtension = Path.GetFileNameWithoutExtension(m_FilePath);


#if UNITY_EDITOR

    /// <summary>
    /// 生成処理
    /// </summary>
    [MenuItem(m_ItemName)]
    public static void Create()
    {
        if (!CanCreate()) { return; }

        List<NewInputActionMap> mapList = new List<NewInputActionMap>();
        List<string> mapNameList = new List<string>();

        SearchActionMap(ref mapNameList, ref mapList);
        CreateScript(mapNameList, mapList);

        EditorUtility.DisplayDialog(m_FileName, "作成が完了しました", "OK");
    }

    /// <summary>
    /// このプロジェクトに存在する全てのアクションマップを検索
    /// </summary>
    public static void SearchActionMap(ref List<string> mapNameList, ref List<NewInputActionMap> mapList)
    {
        string[] guids = AssetDatabase.FindAssets("t:" + nameof(NewInputActionMap));
        foreach (string guid in guids)
        {
            string filePath = AssetDatabase.GUIDToAssetPath(guid);
            
            if (string.IsNullOrEmpty(filePath))
            {
                // 無い場合はログを出す
                throw new FileNotFoundException("NewInputActionMapはありませんでした");
            }

            mapNameList.Add(Path.GetFileNameWithoutExtension(Path.GetFileName(filePath)));
            mapList.Add(AssetDatabase.LoadAssetAtPath<NewInputActionMap>(filePath));
        }
    }

    /// <summary>
    /// 生成してもいい状態か確認
    /// </summary>
    [MenuItem(m_ItemName, true)]
    public static bool CanCreate()
    {
        if (!EditorApplication.isPlaying
            && !Application.isPlaying
            && !EditorApplication.isCompiling)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// スクリプトを作成
    /// </summary>
    public static void CreateScript(List<string> mapNameList, List<NewInputActionMap> mapList)
    {
        StringBuilder builder = new StringBuilder();
        List<string> actionNameList = new List<string>();

        CreateEnum(builder, "NewInputActionMapName", mapNameList);

        foreach (NewInputActionMap map in mapList)
        {
            builder
                .AppendLine("/// <summary>")
                .AppendLine("/// InputAction名を管理するEnum")
                .AppendLine("/// </summary>")
                .AppendLine("namespace NewInputActionName_" + map.m_mapName)
                .AppendLine("{");

            actionNameList.Clear();
            foreach (NewInputAction_Button act_button in map.m_inputActionList_Button)
            {
                actionNameList.Add(act_button.m_ActionName);
            }
            CreateEnum(builder, "Button", actionNameList);

            actionNameList.Clear();
            foreach (NewInputAction_Trigger act_trigger in map.m_inputActionList_Trigger)
            {
                actionNameList.Add(act_trigger.m_ActionName);
            }
            CreateEnum(builder, "Trigger", actionNameList);

            actionNameList.Clear();
            foreach (NewInputAction_Vector2 act_vector2 in map.m_inputActionList_Vector2)
            {
                actionNameList.Add(act_vector2.m_ActionName);
            }
            CreateEnum(builder, "Vector2", actionNameList);

            actionNameList.Clear();
            foreach (NewInputAction_Vector3 act_vector3 in map.m_inputActionList_Vector3)
            {
                actionNameList.Add(act_vector3.m_ActionName);
            }
            CreateEnum(builder, "Vector3", actionNameList);

            builder
                .AppendLine("}")
                .AppendLine("");
        }

        // フォルダを作成
        string directoryName = Path.GetDirectoryName(m_FilePath);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        // ファイルを生成
        File.WriteAllText(m_FilePath, builder.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
    }

    public static void CreateEnum(StringBuilder builder, string enumName, List<string> nameList)
    {
        // enumの構文を書き込む
        builder
            .AppendLine("\t/// <summary>")
            .AppendLine("\t/// InputAction名を管理するEnum")
            .AppendLine("\t/// </summary>")
            .AppendLine("\tpublic enum " + enumName)
            .AppendLine("\t{");

        foreach (var n in nameList.Distinct().Select(
            c => new { var = RemoveInvalidChars(c), val = c }))
        {
            builder.AppendLine($"\t\t{n.var},");
        }

        builder
            .AppendLine("\t}")
            .AppendLine("");

        // 重複している要素をログで伝える
        List<string> duplication = nameList
            .GroupBy(name => name)
            .Where(name => name.Count() > 1)
            .Select(group => group.Key).ToList();
        foreach (string name in duplication)
        {
            Debug.LogError("InputActionMapの" + enumName + "で、" + name + "という名前のInputActionが複数存在しています。重複しないようにしてください");
        }
    }

    /// <summary>
    /// 無効な文字を削除
    /// </summary>
    public static string RemoveInvalidChars(string str)
    {
        Array.ForEach(m_InvaludChars, c => str = str.Replace(c, string.Empty));
        return str;
    }
#endif
}
