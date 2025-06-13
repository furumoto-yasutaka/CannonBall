using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLock_Custom : MonoBehaviour
{
    private Vector3 m_dir = Vector3.down;


    private void Update()
    {
        // ‘Ì‚Ì‰ñ“]‚É‚Á‚Ä‚¢‚©‚ê‚È‚¢‚æ‚¤‚É•ûŒü‚ğ•â³‚·‚é
        transform.rotation =
            Quaternion.identity * Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.down, m_dir), Vector3.forward);
    }

    public void SetRotate(Vector3 Dir)
    {
        m_dir = Dir;
    }
}
