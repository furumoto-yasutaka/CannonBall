using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginInfo : MonoBehaviour
{
    /// <summary> �v���C���[�R���g���[���[�R���|�[�l���g </summary>
    private PlayerController m_playerController;

    /// <summary> �v���C���[�C���p�N�g�R���|�[�l���g </summary>
    private PlayerImpact m_playerImpact;

    /// <summary> �v���C���[�|�C���g�R���|�[�l���g </summary>
    private PlayerPoint_CannonFight m_playerPoint;


    public PlayerController m_PlayerController { get { return m_playerController; } }

    public PlayerImpact m_PlayerImpact { get { return m_playerImpact; } }

    public PlayerPoint_CannonFight m_PlayerPoint { get { return m_playerPoint; } }


    public void InitParam(
        int playerId,
        PlayerController playerController,
        PlayerImpact playerImpact,
        PlayerPoint_CannonFight playerPoint)
    {
        GetComponent<PlayerId>().InitId(playerId);
        m_playerController = playerController;
        m_playerImpact = playerImpact;
        m_playerPoint = playerPoint;
    }
}
