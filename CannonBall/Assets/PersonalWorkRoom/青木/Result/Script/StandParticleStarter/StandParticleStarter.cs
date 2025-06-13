using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Threading.Tasks;

public class StandParticleStarter : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_particleSystems;


    private void Start()
    {
        ResultSceneController.Instance.m_State.Subscribe(_ =>
        {
            if (_ == ResultSceneController.STATE.SHOW_WINNER)
            {
                ParticleStart();

                // サウンド
                Sound();
            }
        }).AddTo(this);


    }

    private void ParticleStart()
    {
        foreach (var particle in m_particleSystems)
        {
            particle.gameObject.SetActive(true);
        }
    }

    async void Sound()
    {
        // サウンド
        AudioManager.Instance.PlaySe("煙", false);
        
        await Task.Delay((int)(500));

        // サウンド
        AudioManager.Instance.PlaySe("煙", false);
    }


}
