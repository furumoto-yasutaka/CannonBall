/*******************************************************************************
*
*	�^�C�g���F	���e�Q�[���̎��Ԃɂ���ĕς��C�x���g�����C����ɂ킩��₷���L�q
*	�t�@�C���F	BombGame_TimeLine.cs
*	�쐬�ҁF	�؁@�喲
*	������F    2023/10/19
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class BombGame_TimeLine : SingletonMonoBehaviour<BombGame_TimeLine>
{
    [SerializeField]
    Timer m_timer;

    //[Space(20)]

    //[Header("30�b�O�̃C�x���g")]
    //[SerializeField]
    //UnityEvent m_30secondAction;

    //[Header("60�b�O�̃C�x���g")]
    //[SerializeField]
    //UnityEvent m_60secondAction;

    [SerializeField, CustomLabel("���o�𐶐�����Canvas")]
    Canvas m_canvas;

    [SerializeField, CustomLabel("���[���h���W�pCanvas")]
    Canvas m_worldCanvas;


    [SerializeField, CustomLabel("60�b�O�̉��o")]
    GameObject m_60secondDecorationPrehab;


    bool m_is60second = false;
    bool m_is30second = false;
    

    public bool GetIs60second { get { return m_is60second; } }

    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();
    }


    private void Start()
    {

    }


    private void Update()
    {
        // �c��30�b�ɂȂ�����
        if (m_timer.m_TimeCounter <= 30 && !m_is30second)
        {
            //m_30secondAction.Invoke();

            BombManager.Instance.SetCurrentSpawMapIndex(2);

            //foreach (GameObject go in m_30secondDecorationPrehab)
            //{
            //    GameObject game = Instantiate(go);
            //    game.transform.parent = m_canvas.transform;
            //}

            m_is30second = true;
        }
        // �c��60�b�ɂȂ�����
        if (m_timer.m_TimeCounter <= 60 && !m_is60second)
        {
            //m_60secondAction.Invoke();

            GameObject game = Instantiate(m_60secondDecorationPrehab);
            game.transform.parent = m_worldCanvas.transform;
            game.transform.localPosition = Vector3.zero;

            game.transform.localScale = Vector3.one;



            m_is60second = true;
        }

    }



    //public void TestEvet()
    //{
    //    Debug.Log("TestEvet");
    //}

    //public void a40Event()
    //{
    //    Debug.Log("a40Event");
    //}



}
