using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateDebugMenu : MonoBehaviour
{
    private void Awake()
    {
#if RELEASE
#else
        SceneManager.LoadScene("DebugMenu", LoadSceneMode.Additive);
        Destroy(gameObject);
#endif
    }
}
