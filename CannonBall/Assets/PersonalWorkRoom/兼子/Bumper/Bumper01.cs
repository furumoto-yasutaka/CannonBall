using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper01 : MonoBehaviour
{
    private float power = 20.0f;

    // ���W�b�h�{�f�B�ɐG�ꂽ���ɌĂ΂��
    private void OnCollisionEnter(Collision other)
    {
        // ����̓^�O�Ńv���C���[���ǂ������f
        if (other.transform.CompareTag("Player"))
        {
            // �v���C���[�̃��W�b�h�{�f�B���擾
            Rigidbody playerRigid = other.transform.GetComponent<Rigidbody>();

            // �v���C���[�̃��W�b�h�{�f�B�ɁA���݂̐i�s�����̋t�����ɗ͂�������
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
