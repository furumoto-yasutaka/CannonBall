using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWall : MonoBehaviour
{
    MeshRenderer m_meshRenderer;

    bool m_hit = false;

    private void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.CompareTag("Player") && !m_hit)
        {
            // プレイヤーが衝突してきたことをTutorialWallsに通知する
            transform.parent.GetComponent<TutorialWalls>().WallHit(this);

            m_hit = true;
        }
    }


    public void ChangeHitMaterial(Material _material)
    {
        m_meshRenderer.material = _material;
    }
}
