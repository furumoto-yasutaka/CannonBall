using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InitVersionText : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = "ver " + Application.version;
    }
}
