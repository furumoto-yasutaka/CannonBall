/*******************************************************************************
*
*	タイトル：	ラスト60秒前の処理
*	ファイル：	BombGame_Last60second.cs
*	作成者：	青木　大夢
*	制作日：    2023/10/20
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BombGame_Last60second : MonoBehaviour
{

    [SerializeField]
    GameObject m_60second;

    [SerializeField]
    GameObject m_60secondRedScreen;

    [SerializeField]
    GameObject m_Damage2;


    public Canvas m_Canvas;

    private void Start()
    {
        DelaySpaw();
    }


    private void Update()
    {
        
    }

    private async void DelaySpaw()
    {
        GameObject obj = Instantiate(m_60second);
        obj.transform.SetParent(gameObject.transform);
        obj.transform.localPosition = Vector3.zero;

        obj = Instantiate(m_60secondRedScreen);
        obj.transform.SetParent(gameObject.transform);
        obj.transform.localPosition = Vector3.zero;


        BombManager.Instance.SetCurrentSpawMapIndex(1);

        BombManager.Instance.BombMultSpeed = 2;

        await Task.Delay(3 * 1000);

        obj = Instantiate(m_Damage2);
        obj.transform.SetParent(gameObject.transform);
        obj.transform.localPosition = Vector3.zero;
    }



}
