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

    // アニメーションで使用
    private void CannonStart()
    {
        m_cannonCharacter.AnimationStart();
    }

    // アニメーションで使用
    private void CannonFinish()
    {
    }


    // アニメーションで使用
    private void StartSound()
    {
        AudioManager.Instance.PlaySe("ゲート減圧装置", false);
        AudioManager.Instance.PlaySe("ゲート開閉音", false);
    }

    // アニメーションで使用
    private void SmokeSound()
    {
        AudioManager.Instance.PlaySe("ゲート減圧装置", false);
    }

}
