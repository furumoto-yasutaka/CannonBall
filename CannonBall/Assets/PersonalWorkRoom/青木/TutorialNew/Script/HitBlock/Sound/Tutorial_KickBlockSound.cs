using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_KickBlockSound : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "LegCollision")
        {
            Debug.Log("おとととととｔ");
            AudioManager.Instance.PlaySe("チュートリアル_クッション目標物", false);
        }

    }
}
