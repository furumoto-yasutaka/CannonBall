using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper01 : MonoBehaviour
{
    private float power = 20.0f;

    // リジッドボディに触れた時に呼ばれる
    private void OnCollisionEnter(Collision other)
    {
        // 今回はタグでプレイヤーかどうか判断
        if (other.transform.CompareTag("Player"))
        {
            // プレイヤーのリジッドボディを取得
            Rigidbody playerRigid = other.transform.GetComponent<Rigidbody>();

            // プレイヤーのリジッドボディに、現在の進行方向の逆向きに力を加える
            playerRigid.velocity = (other.transform.position - transform.position).normalized * power;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
