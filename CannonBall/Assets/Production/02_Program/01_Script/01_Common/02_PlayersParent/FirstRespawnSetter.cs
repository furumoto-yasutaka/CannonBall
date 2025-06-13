using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstRespawnSetter : MonoBehaviour
{
    private void Awake()
    {
        // �v���C���[�̏������X�|�[�����s��
        List<Transform> players = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform p = transform.GetChild(i);
            p.GetComponent<PlayerController_CannonFight>().FirstDeath();
            players.Add(p);
        }
        RespawnManager.Instance.StartRespawnAll(players.ToArray());

        // �s�v�Ȃ̂ł��̃R���|�[�l���g���폜
        Destroy(this);
    }
}
