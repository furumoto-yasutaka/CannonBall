using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TitleTutorialView : SingletonMonoBehaviour<TitleTutorialView>
{
    public enum STATE
    {
        NONE,
        VIEW,
        HIDE,
    }


    [SerializeField, CustomLabel("アルファ速度")]
    float m_speed = 2.0f;

    CanvasGroup m_canvasGroup;
    float m_alpha = 0.0f;

    public STATE m_State;

    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();

        m_State = STATE.NONE;
    }

    private void Start()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
    }
    public async void View()
    {
        await Task.Delay((int)(1000 * 1));

        m_State = STATE.VIEW;
    }




    private void Update()
    {
        switch (m_State)
        {
            case STATE.NONE:
                break;
            case STATE.VIEW:
                m_alpha += m_speed * Time.deltaTime;
                if (m_alpha >= 1.0f)
                {
                    m_State = STATE.NONE;
                }
                break;
            case STATE.HIDE:
                m_alpha -= m_speed * Time.deltaTime;
                if (m_alpha <= 0.0f)
                {
                    m_State = STATE.NONE;

                    // チュートリアルシーンの最後の最後に行う処理
                    TitleStageSelectManager.Instance.ExchangeStageSelect(TitleStageSelectManager.Scene.StageSelect);
                }
                break;
            default:
                break;
        }

        if (m_State != STATE.NONE)
        {
            m_canvasGroup.alpha = m_alpha;
        }
    }
}
