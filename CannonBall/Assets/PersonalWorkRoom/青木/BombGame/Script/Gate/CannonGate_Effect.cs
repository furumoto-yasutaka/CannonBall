using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonGate_Effect : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] m_particleSystems;



    private void Start()
    {
        for (int i = 0; i < m_particleSystems.Length; i++)
        {
            m_particleSystems[i].gameObject.SetActive(false);
        }
    }

    private void StartSmokeParticle()
    {
        for (int i = 0; i < m_particleSystems.Length; i++)
        {
            m_particleSystems[i].gameObject.SetActive(false);
            m_particleSystems[i].gameObject.SetActive(true);
        }
    }


}
