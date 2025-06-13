using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelMaterialGetter : MonoBehaviour
{
    /// <summary> インスペクターにアタッチされたオブジェクトをしらべるか </summary>
    [SerializeField, CustomLabel("インスペクターにアタッチされたオブジェクトをしらべるか")]
    protected bool m_isUseInspectorAsset = false;

    /// <summary> マテリアルを取得するモデルの親オブジェクト </summary>
    [SerializeField, CustomLabel("マテリアルを取得するモデルのオブジェクト")]
    protected Transform m_modelObject = null;

    /// <summary> 処理を生成された瞬間行うか </summary>
    [SerializeField, CustomLabel("処理を生成された瞬間行うか")]
    private bool m_isQuickExec = true;

    /// <summary> 取得したマテリアルのリスト </summary>
    public List<Material> m_materials = new List<Material>();


    public List<Material> m_Materials { get { return m_materials; } }


    // 優先的に処理を行うため優先度を-190位にしてあります
    protected virtual void Awake()
    {
        if (!m_isQuickExec) { return; }

        if (m_isUseInspectorAsset)
        {
            GetMaterialThis(m_modelObject);
        }
        else
        {
            GetMaterialChild(transform);
        }
    }

    protected virtual void Start()
    {
        if (m_isQuickExec) { return; }

        if (m_isUseInspectorAsset)
        {
            GetMaterialThis(m_modelObject);
        }
        else
        {
            GetMaterialChild(transform);
        }
    }

    /// <summary> マテリアルを取得 </summary>
    /// <param name="parent"> 探索対象の親オブジェクト </param>
    public void GetMaterialChild(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            MeshRenderer renderer;
            if (parent.GetChild(i).TryGetComponent(out renderer))
            {
                if (!m_materials.Contains(renderer.material) && renderer.material != null)
                {
                    m_materials.Add(renderer.material);
                }
            }
            GetMaterialChild(parent.GetChild(i));
        }
    }

    /// <summary> マテリアルを取得 </summary>
    /// <param name="parent"> 探索対象の親オブジェクト </param>
    public void GetMaterialThis(Transform trans)
    {
        MeshRenderer renderer;
        if (trans.TryGetComponent(out renderer))
        {
            if (!m_materials.Contains(renderer.material) && renderer.material != null)
            {
                m_materials.Add(renderer.material);
            }
        }
        GetMaterialChild(trans);
    }
}
