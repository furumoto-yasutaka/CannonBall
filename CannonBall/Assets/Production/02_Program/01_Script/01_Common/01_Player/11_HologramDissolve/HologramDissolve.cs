using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologramDissolve : MonoBehaviour
{
    [SerializeField, CustomLabel("マテリアル取得コンポーネント")]
    private ModelMaterialGetter m_materialGetter;

    [HideInInspector]
    public float m_clipTime = 2.0f;

    private bool m_isUpdateMaterial = false;


    private void Update()
    {
        if (!m_isUpdateMaterial) { return; }

        for (int i = 0; i < m_materialGetter.m_Materials.Count; i++)
        {
            Material mat = m_materialGetter.m_Materials[i];
            mat.SetFloat("_ClipTime", m_clipTime);
        }
    }

    public void StartUpdate()
    {
        m_isUpdateMaterial = true;
    }

    public void EndUpdate()
    {
        m_isUpdateMaterial = false;
        m_clipTime = 2.0f;
        for (int i = 0; i < m_materialGetter.m_Materials.Count; i++)
        {
            Material mat = m_materialGetter.m_Materials[i];
            mat.SetFloat("_ClipTime", m_clipTime);
        }
    }
}
