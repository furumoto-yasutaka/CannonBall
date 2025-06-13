using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    /// <summary> �Ǘ��Ώۂ̃R���W���� </summary>
    [SerializeField, CustomLabel("�Ǘ��Ώۂ̃R���W����")]
    private Collider2D[] m_colliders;

    
    public void EnableCollider()
    {
        foreach (Collider2D col in m_colliders)
        {
            col.enabled = true;
        }
    }

    public void DisableCollider()
    {
        foreach (Collider2D col in m_colliders)
        {
            col.enabled = false;
        }
    }
}
