using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTruck_DangerRun : RespawnTruck
{
    public override void StartJumpOut()
    {
        Transform road = transform.GetChild(1);
        for (int i = 0; i < road.childCount; i++)
        {
            Transform player = road.GetChild(i);

            // �v���C���[���g���b�N���番�����J�����̎q�ɐݒ肷��
            Transform camera = ((RespawnManager_DangerRun)RespawnManager_DangerRun.Instance).m_Camera;
            player.SetParent(camera);

            player.localPosition = Vector3.zero;
            player.localRotation = Quaternion.identity;

            // �v���C���[�̔�яo���A�j���[�V�������w�肷��
            RespawnManager.Instance.SetRespawnAnimation(player);

            // �G�t�F�N�g�𐶐�
            EffectContainer.Instance.EffectPlay(
                "���X�|�[��",
                road.position,
                Quaternion.Euler(-90.0f, 0.0f, 0.0f));
        }

        AudioManager.Instance.PlaySe(
            "���X�|�[��_��яo����",
            false);
    }
}
