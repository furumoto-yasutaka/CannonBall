using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextBoxBackGroundChange : MonoBehaviour
{
    [SerializeField, CustomLabel("ïÅí ÇÃîwåi(è„)")]
    Sprite m_normalBG_Upper;

    [SerializeField, CustomLabel("ïÅí ÇÃîwåi(è„)")]
    Sprite m_exBG_Upper;

    [SerializeField, CustomLabel("ïÅí ÇÃîwåi(â∫)")]
    Sprite m_normalBG_Bottom;

    [SerializeField, CustomLabel("ïÅí ÇÃîwåi(â∫)")]
    Sprite m_exBG_Bottom;

    Image m_image;

    private void Start()
    {
        m_image = GetComponent<Image>();
    }


    public void ChangeNormalBG_Upper()
    {
        m_image.sprite = m_normalBG_Upper;
    }


    public void ChangeExBG_Upper()
    {
        m_image.sprite = m_exBG_Upper;
    }

    public void ChangeNormalBG_Bottom()
    {
        m_image.sprite = m_normalBG_Bottom;
    }


    public void ChangeExBG_Bottom()
    {
        m_image.sprite = m_exBG_Bottom;
    }
}
