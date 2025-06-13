using UnityEngine;

public class HitBar : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        //�@
        if (collider2D.gameObject.CompareTag("Player"))
        {
            Destroy(collider2D.gameObject);
        }
    }
}
