using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerImpact_CannonFight_WallShockWave : PlayerImpact_CannonFight
{
    protected override void Start()
    {
        base.Start();

        m_playerSpMove = GetComponent<PlayerSpMove_WallShockWave>();
    }

    /// <summary>
    /// ÉvÉåÉCÉÑÅ[Ç…èRÇÁÇÍÇΩèàóù
    /// </summary>
    public override void Kicked(Vector2 dir, float power, Vector2 vel)
    {
        base.Kicked(dir, power, vel);

        ((PlayerSpMove_WallShockWave)m_playerSpMove).DisableIsSetSpMoveVelocity();
    }
}
