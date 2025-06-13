using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonGate : MonoBehaviour
{
    [SerializeField]
    CannonCharacter m_cannonCharacter;



    private void Update()
    {
        
    }

    // �A�j���[�V�����Ŏg�p
    private void CannonStart()
    {
        m_cannonCharacter.AnimationStart();
    }

    // �A�j���[�V�����Ŏg�p
    private void CannonFinish()
    {
    }


    // �A�j���[�V�����Ŏg�p
    private void StartSound()
    {
        AudioManager.Instance.PlaySe("�Q�[�g�������u", false);
        AudioManager.Instance.PlaySe("�Q�[�g�J��", false);
    }

    // �A�j���[�V�����Ŏg�p
    private void SmokeSound()
    {
        AudioManager.Instance.PlaySe("�Q�[�g�������u", false);
    }

}
