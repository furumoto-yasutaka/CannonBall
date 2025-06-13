using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSuccessMonitor : MonoBehaviour
{
    [SerializeField, CustomLabel("OK‰æ‘œ")]
    Texture2D m_okTexture;

    float m_baseColor = 6000f;

    MeshRenderer m_meshRenderer;

    private void Start()
    {
        m_meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    public void SetOKSprite() { m_meshRenderer.material.SetTexture("_MainTex", m_okTexture); }

    public void SetColor() { m_meshRenderer.material.SetFloat("_BaseEmissionPower", m_baseColor); }

}
