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

        AudioManager.Instance.PlaySe("スタート入力時の弾ける音", false);
    }

    // アニメーション側で使用
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
