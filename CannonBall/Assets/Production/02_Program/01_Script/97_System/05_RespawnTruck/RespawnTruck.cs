using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTruck : MonoBehaviour
{
    /// <summary> �A�j���[�^�[�R���|�[�l���g </summary>
    protected Animator m_animator;


    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void SetPlayer(Transform player)
    {
        player.SetParent(transform.GetChild(1));
        player.localPosition = Vector3.zero;
        player.localRotation = Quaternion.identity;
    }

    public virtual void StartJumpOut()
    {
        Transform road = transform.GetChild(1);
        while (road.childCount > 0)
        {
            Transform player = road.GetChild(0);

            // �v���C���[���g���b�N���番��
            player.SetParent(null);

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

    public void DestroyTruck()
    {
        Destroy(gameObject);
    }
}
