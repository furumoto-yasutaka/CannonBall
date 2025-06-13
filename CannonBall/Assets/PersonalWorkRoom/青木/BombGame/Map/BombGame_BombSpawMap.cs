using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BombInfo
{
    [CustomLabel("    ���e�̃v���n�u")]
    public GameObject m_BombPrefab;

    [CustomLabel("    �X�|�[�������鐔")]
    public int m_Volume = 1;

    [CustomLabel("    �����½�݂߰������ꍇ�̽�߰݊Ԋu(�b)")]
    public float m_SpawDistance;
}

[System.Serializable]
public class BombSpaw
{
    [CustomLabel("�����e�̏o���p�x(0.0�`1.0)")]
    public float m_BombFrequency;

    // �Ⴄ�^�C�v�̔��e���o�ꂷ��ꍇ�A�z����g��
    [CustomLabel("���e�̏��")]
    public BombInfo[] m_BombType;

}

[CreateAssetMenu(fileName = "BombSpawMap", menuName = "CreateBombSpawMap")]


public class BombGame_BombSpawMap : ScriptableObject
{
    /// <summary> �}�b�v�̖��O </summary>
    public List<BombSpaw> m_SpawBombMap;

}
