using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ResultSceneController : SingletonMonoBehaviour<ResultSceneController>
{
	public enum STATE
	{
		//PRE_START,	    // �O�i�K�̏������
        RESULT_TITLE,       // ���ʔ��\�\�����
		//SHOW_RANK,	    // ��l�ЂƂ�̏��ʂ�������
        //SHOW_WHOLE,       // �S�̂̏��ʂ�������
        SHOW_WINNER,        // ���҂�\��
		DETAIL_WINDOW,	    // �ڍ׃E�B���h�E�\��
        NEXT_SCENE_SELECT,  // �ð�޾ڸ�or���ق��I�ԏ��
    }

    /// <summary> �c�@�� </summary>
    [SerializeField, CustomReadOnly]
    private ReactiveProperty<STATE> m_state = new ReactiveProperty<STATE>();

    /// <summary> �c�@��(Presenter�Q�Ɨp) </summary>
    public IReadOnlyReactiveProperty<STATE> m_State => m_state;

    private float m_waittime = 2.0f;

    public float m_waittimeCount = 0.0f;

    public void SetState(STATE _state) { m_state.Value = _state; }

    public void SetWaitTime()
    {
        m_waittimeCount = m_waittime;
    }

    protected override void Awake()
    {
        dontDestroyOnLoad = false;
        base.Awake();


        m_state.Value = STATE.RESULT_TITLE;
    }

    private void Update()
    {
        if (m_waittimeCount > 0.0f)
        {
            m_waittimeCount -= Time.deltaTime;
            if (m_waittimeCount < 0.0f)
            {
                m_waittimeCount = 0.0f;
            }
        }
    }
}
