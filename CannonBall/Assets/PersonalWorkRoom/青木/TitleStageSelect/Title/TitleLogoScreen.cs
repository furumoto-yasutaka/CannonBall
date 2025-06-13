using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogoScreen : MonoBehaviour
{
    [SerializeField]
    Texture2D m_whiteTexture;

    [SerializeField]
    Texture2D m_LogoTexture;

    MeshRenderer m_meshRenderer;

    private void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();

    }
    public void ChangeTexture()
    {
        m_meshRenderer.material.SetTexture("_Test", m_LogoTexture);
    }
}
