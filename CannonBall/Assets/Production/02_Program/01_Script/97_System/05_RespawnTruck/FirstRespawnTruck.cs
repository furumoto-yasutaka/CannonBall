using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstRespawnTruck : RespawnTruck
{
    public void SetPlayers(Transform[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetParent(transform.GetChild(1).GetChild(i));
            players[i].localPosition = Vector3.zero;
            players[i].localRotation = Quaternion.identity;
        }
    }

    public override void StartJumpOut()
    {
        Transform road = transform.GetChild(1);
        for (int i = 0; i < road.childCount; i++)
        {
            Transform player = road.GetChild(i).GetChild(0);

            // �v���C���[���g���b�N���番��
            player.SetParent(null);

            // �v���C���[�̔�яo���A�j���[�V�������w�肷��
            RespawnManager.Instance.SetFirstRespawnAnimation(player);

            // �G�t�F�N�g�𐶐�
            EffectContainer.Instance.EffectPlay(
                "�ŏ��̃X�|�[��",
                road.position,
                Quaternion.Euler(-90.0f, 0.0f, 0.0f));
        }
    }
}
