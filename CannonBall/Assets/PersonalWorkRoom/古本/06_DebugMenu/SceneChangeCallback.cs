using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneChangeCallback : MonoBehaviour
{
    private string m_sceneName = "";


    public void Init(string sceneName)
    {
        m_sceneName = sceneName;
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "ChangeScene : " + m_sceneName;
    }

    public void Callback()
    {
        GetComponent<SceneChanger>().StartSceneChange(m_sceneName);
    }
}
