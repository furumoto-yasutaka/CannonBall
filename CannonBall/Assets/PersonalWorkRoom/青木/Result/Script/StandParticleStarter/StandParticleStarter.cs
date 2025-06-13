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

                // �T�E���h
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
        // �T�E���h
        AudioManager.Instance.PlaySe("��", false);
        
        await Task.Delay((int)(500));

        // �T�E���h
        AudioManager.Instance.PlaySe("��", false);
    }


}
