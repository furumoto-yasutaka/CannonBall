using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ResultBGMStarter : MonoBehaviour
{
    [SerializeField]
    float m_time = 0f;


    private void Start()
    {
        Sound();
    }

    private async void Sound()
    {
        AudioManager.Instance.PlayBgm("���U���g", false);

        await Task.Delay((int)(1000 * m_time));

        AudioManager.Instance.PlayBgm("���U���g�p��", true);
    }


}
