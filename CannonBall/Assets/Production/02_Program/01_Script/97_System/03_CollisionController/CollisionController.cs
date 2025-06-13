using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    /// <summary> 管理対象のコリジョン </summary>
    [SerializeField, CustomLabel("管理対象のコリジョン")]
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
