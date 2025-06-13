using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CommonSlideArrowView : MonoBehaviour
{
    enum ARROW_TYPE
    {
        RIGHT,
        LEFT,
    }


    public enum STATE
    {
        NONE,
        VIEW,
        HIDE,
    }



    [SerializeField]
    ARROW_TYPE m_type;

    [SerializeField]
    PlayGuideCursorInput m_input;

    STATE m_state = STATE.NONE;

    Image m_image;

    float m_speed = 6.0f;
    float m_alpha = 0.0f;
    float m_delayTime = 0.2f;


    public STATE GetState() {  return m_state; }
    public void SetState(STATE _state) { m_state = _state; }


    private void Start()
    {
        m_image = GetComponent<Image>();
    }




    private void Update()
    {
        switch (m_state)
        {
            case STATE.VIEW:
                ImageAlphaAdd();
                break;
            case STATE.HIDE:
                ImageAlphaSub();
                break;
            default:
                break;
        }

    }

    private void ChangeAlpha()
    {
        Color color = Color.white - (Color.black - Color.black * m_alpha);

        if (m_image != null)
        {
            m_image.color = color;
        }
    }




    async void ImageAlphaAdd()
    {
        await Task.Delay((int)(m_delayTime * 1000));

        m_alpha += m_speed * Time.deltaTime;

        ChangeAlpha();

        if (m_alpha >= 1.0f)
        {
            m_alpha = 1.0f;
        }
    }

    private void ImageAlphaSub()
    {
        m_alpha -= m_speed * Time.deltaTime;

        ChangeAlpha();

        if (m_alpha <= 0.0f)
        {
            m_alpha = 0.0f;
        }
    }


}
