/*******************************************************************************
*
*	�^�C�g���F	���ۓ��͖���Enum�����X�N���v�g
*	�t�@�C���F	NewInputActionNameCreator.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/09/24
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
    // �����ȕ������Ǘ�����z��
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

    /// <summary> Unity�̂ǂ��ő���ł���悤�ɂ��邩�̃p�X </summary>
    private const string m_ItemName = "Tools/Create/NewInputAction Name Enum";

    /// <summary> �t�@�C���p�X </summary>
    private const string m_FilePath
        = "Assets/Production/02_Program/01_Script/97_System/01_NewInputActionName/NewInputActionNameEnum.cs";

    /// <summary> �t�@�C����(�g���q����) </summary>
    private static readonly string m_FileName = Path.GetFileName(m_FilePath);

    /// <summary> �t�@�C����(�g���q����) </summary>
    private static readonly string m_FileNameWithoutExtension = Path.GetFileNameWithoutExtension(m_FilePath);


#if UNITY_EDITOR

    /// <summary>
    /// ��������
    /// </summary>
    [MenuItem(m_ItemName)]
    public static void Create()
    {
        if (!CanCreate()) { return; }

        List<NewInputActionMap> mapList = new List<NewInputActionMap>();
        List<string> mapNameList = new List<string>();

        SearchActionMap(ref mapNameList, ref mapList);
        CreateScript(mapNameList, mapList);

        EditorUtility.DisplayDialog(m_FileName, "�쐬���������܂���", "OK");
    }

    /// <summary>
    /// ���̃v���W�F�N�g�ɑ��݂���S�ẴA�N�V�����}�b�v������
    /// </summary>
    public static void SearchActionMap(ref List<string> mapNameList, ref List<NewInputActionMap> mapList)
    {
        string[] guids = AssetDatabase.FindAssets("t:" + nameof(NewInputActionMap));
        foreach (string guid in guids)
        {
            string filePath = AssetDatabase.GUIDToAssetPath(guid);
            
            if (string.IsNullOrEmpty(filePath))
            {
                // �����ꍇ�̓��O���o��
                throw new FileNotFoundException("NewInputActionMap�͂���܂���ł���");
            }

            mapNameList.Add(Path.GetFileNameWithoutExtension(Path.GetFileName(filePath)));
            mapList.Add(AssetDatabase.LoadAssetAtPath<NewInputActionMap>(filePath));
        }
    }

    /// <summary>
    /// �������Ă�������Ԃ��m�F
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
    /// �X�N���v�g���쐬
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
                .AppendLine("/// InputAction�����Ǘ�����Enum")
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

        // �t�H���_���쐬
        string directoryName = Path.GetDirectoryName(m_FilePath);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        // �t�@�C���𐶐�
        File.WriteAllText(m_FilePath, builder.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
    }

    public static void CreateEnum(StringBuilder builder, string enumName, List<string> nameList)
    {
        // enum�̍\������������
        builder
            .AppendLine("\t/// <summary>")
            .AppendLine("\t/// InputAction�����Ǘ�����Enum")
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

        // �d�����Ă���v�f�����O�œ`����
        List<string> duplication = nameList
            .GroupBy(name => name)
            .Where(name => name.Count() > 1)
            .Select(group => group.Key).ToList();
        foreach (string name in duplication)
        {
            Debug.LogError("InputActionMap��" + enumName + "�ŁA" + name + "�Ƃ������O��InputAction���������݂��Ă��܂��B�d�����Ȃ��悤�ɂ��Ă�������");
        }
    }

    /// <summary>
    /// �����ȕ������폜
    /// </summary>
    public static string RemoveInvalidChars(string str)
    {
        Array.ForEach(m_InvaludChars, c => str = str.Replace(c, string.Empty));
        return str;
    }
#endif
}
