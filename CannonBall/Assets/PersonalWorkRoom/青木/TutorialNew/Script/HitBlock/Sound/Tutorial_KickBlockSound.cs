using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_KickBlockSound : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "LegCollision")
        {
            Debug.Log("���ƂƂƂƂƂ�");
            AudioManager.Instance.PlaySe("�`���[�g���A��_�N�b�V�����ڕW��", false);
        }

    }
}
