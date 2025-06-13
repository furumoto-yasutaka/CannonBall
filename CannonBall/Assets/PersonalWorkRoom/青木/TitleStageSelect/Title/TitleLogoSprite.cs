using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TitleLogoSprite : MonoBehaviour
{
    [SerializeField]
    TitleLogoScreen m_titleLogoScreen;

    Animator m_animator;


    private void Start()
    {
        m_animator = GetComponent<Animator>();

    }

    public void StartAnimation()
    {
        m_animator.SetTrigger("Start");

        AudioManager.Instance.PlaySe("�X�^�[�g���͎��̒e���鉹", false);
    }

    // �A�j���[�V�������Ŏg�p
    private void TitleFinish()
    {
        //m_titleLogoScreen.ChangeTexture();

        Wait();
    }


    private /*async*/ void Wait()
    {
        //await Task.Delay((int)(1000 * 1.5));

        TitleManager.Instance.SetTitleFinish(true);
    }


}
