using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayGuideAsset
{
    [CustomLabel("������e�N�X�`��")]
    public Sprite m_GuideSprite;

    [CustomLabel("�������̓e�L�X�g")]
    public TextAsset m_GuideText;
}


[CreateAssetMenu(fileName = "PlayGuideAssetMap", menuName = "CreatePlayGuideAssetMap")]


public class StageSelect_PlayGuideAssetMap : ScriptableObject
{
    public GameObject m_GameLogo;
    public GameObject m_GuideObject;
}
