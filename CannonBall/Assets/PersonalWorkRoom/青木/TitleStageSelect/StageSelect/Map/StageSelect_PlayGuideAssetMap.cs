using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayGuideAsset
{
    [CustomLabel("説明画テクスチャ")]
    public Sprite m_GuideSprite;

    [CustomLabel("説明文章テキスト")]
    public TextAsset m_GuideText;
}


[CreateAssetMenu(fileName = "PlayGuideAssetMap", menuName = "CreatePlayGuideAssetMap")]


public class StageSelect_PlayGuideAssetMap : ScriptableObject
{
    public GameObject m_GameLogo;
    public GameObject m_GuideObject;
}
