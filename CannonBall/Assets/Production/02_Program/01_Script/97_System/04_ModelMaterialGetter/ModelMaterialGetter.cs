using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelMaterialGetter : MonoBehaviour
{
    /// <summary> �C���X�y�N�^�[�ɃA�^�b�`���ꂽ�I�u�W�F�N�g������ׂ邩 </summary>
    [SerializeField, CustomLabel("�C���X�y�N�^�[�ɃA�^�b�`���ꂽ�I�u�W�F�N�g������ׂ邩")]
    protected bool m_isUseInspectorAsset = false;

    /// <summary> �}�e���A�����擾���郂�f���̐e�I�u�W�F�N�g </summary>
    [SerializeField, CustomLabel("�}�e���A�����擾���郂�f���̃I�u�W�F�N�g")]
    protected Transform m_modelObject = null;

    /// <summary> �����𐶐����ꂽ�u�ԍs���� </summary>
    [SerializeField, CustomLabel("�����𐶐����ꂽ�u�ԍs����")]
    private bool m_isQuickExec = true;

    /// <summary> �擾�����}�e���A���̃��X�g </summary>
    public List<Material> m_materials = new List<Material>();


    public List<Material> m_Materials { get { return m_materials; } }


    // �D��I�ɏ������s�����ߗD��x��-190�ʂɂ��Ă���܂�
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

    /// <summary> �}�e���A�����擾 </summary>
    /// <param name="parent"> �T���Ώۂ̐e�I�u�W�F�N�g </param>
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

    /// <summary> �}�e���A�����擾 </summary>
    /// <param name="parent"> �T���Ώۂ̐e�I�u�W�F�N�g </param>
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
