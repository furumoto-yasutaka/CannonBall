/*******************************************************************************
*
*	�^�C�g���F	�Z�[�u�f�[�^�Ǘ��V���O���g���X�N���v�g
*	�t�@�C���F	SaveDataManager.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/05/05
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

    /// <summary> �Z�[�u�f�[�^�\���� </summary>
    public struct SaveData
    {
        /// <summary> �e�X�g </summary>
        public int Test;
    }

    #endregion

    #region field

    /// <summary> �Z�[�u�f�[�^�̃t�@�C���� </summary>
    private string m_FileName = "SaveData";

    /// <summary> �Z�[�u�f�[�^��ۑ�����ۂ̃p�X </summary>
    private string m_FullPath;

    /// <summary> �Z�[�u�f�[�^ </summary>
    private ReactiveProperty<SaveData> m_saveData = new ReactiveProperty<SaveData>();

    /// <summary> �Z�[�u�f�[�^(Presenter�Q�Ɨp) </summary>
    public IReadOnlyReactiveProperty<SaveData> m_SaveData => m_saveData;

    #endregion

    #region function

    /// <summary>
    /// ���̃X�N���v�g�̏������ŎQ�Ƃ���邽�߁A
    /// �ő��Ŏ��s�ł���悤�ɗD��x��-105�ɏオ���Ă��܂�
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = 60;

        m_FullPath = Application.persistentDataPath + "/" + m_FileName + ".json";

        // �Z�[�u�f�[�^�̗L��
        if (File.Exists(m_FullPath))
        {
            // ���݂����烍�[�h
            Load();
        }
        else
        {
            // ���݂��Ă��Ȃ�������
            // �f�t�H���g�l�ŏ��������ăf�B���N�g�����쐬
            FormatSaveData();
            CreateDirectory();
            Save();
        }
    }

    /// <summary>
    /// ���[�h����
    /// </summary>
    public void Load()
    {
        // �t�@�C���ǂݍ���
        StreamReader reader = new StreamReader(m_FullPath);
        string fileData = reader.ReadToEnd();
        reader.Close();

        // ��������Z�[�u�f�[�^�\���̔z��ɕϊ����Ċi�[
        m_saveData.Value = JsonConvert.DeserializeObject<SaveData>(fileData);
    }

    /// <summary>
    /// �Z�[�u����
    /// </summary>
    public void Save()
    {
        // json�t�@�C���̐ݒ�
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;  // �z�Q�Ƃ���I�u�W�F�N�g�̌x���𖳎�����
        settings.Formatting = Formatting.Indented;                      // ���s����
        settings.Converters.Add(new StringEnumConverter(new DefaultNamingStrategy()));  // enum�ŕ�������L��

        // �f�[�^��json�`���ɕϊ�
        string data = JsonConvert.SerializeObject(m_saveData.Value, settings);

        // �t�@�C����������
        StreamWriter writer = new StreamWriter(m_FullPath, false);
        writer.WriteLine(data);
        writer.Flush();
        writer.Close();
    }

    /// <summary>
    /// �Z�[�u�f�[�^����������
    /// </summary>
    private void FormatSaveData()
    {

    }

    /// <summary>
    /// �Z�[�u�f�[�^�폜
    /// </summary>
    public void DeleteSaveData()
    {
        // �Z�[�u�f�[�^������
        FormatSaveData();

        //// ���Q�[����������������
        //AudioManager.Instance.m_BgmVolume = m_SaveData.Value.Option.BgmVolume * 0.1f;
        //AudioManager.Instance.m_SeVolume = m_SaveData.Value.Option.SeVolume * 0.1f;

        // �����������f�[�^�ŃZ�[�u����
        Save();
    }

    /// <summary>
    /// �Z�[�u�f�[�^�����\��p�X�܂ł̃t�H���_�𐶐�
    /// </summary>
    private void CreateDirectory()
    {
        // �f�B���N�g��������ŏ�w�̃f�B���N�g�����𔲂����p�X���擾
        string remainPath = Path.GetDirectoryName(m_FullPath) + "/";
        // �m�F�Ώۂ̃t�H���_
        string searchDirectoryPath = "";
        // �����Ɏg�p����f�B���N�g����
        string directoryName;

        // �S�Ẵp�X���m�F����܂�
        while (remainPath != "")
        {
            // ���̃t�H���_�̃p�X��ݒ�
            int index = GetIndexOfSlash(remainPath);
            directoryName = remainPath.Remove(index);
            searchDirectoryPath += "/" + directoryName;
            remainPath = remainPath.Remove(0, index + 1);

            // �t�H���_�����݂��Ȃ�������V������������
            if (!Directory.Exists(searchDirectoryPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
                directoryInfo.Create();
            }
        }
    }

    /// <summary>
    /// �X���b�V����������̉������ڂɂ��邩�擾����
    /// </summary>
    /// <param name="path"> �T���Ώۂ̃p�X </param>
    /// <returns> ���������ꍇ�͉������ڂ���Ԃ��A������Ȃ������當����̕�������Ԃ� </returns>
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
