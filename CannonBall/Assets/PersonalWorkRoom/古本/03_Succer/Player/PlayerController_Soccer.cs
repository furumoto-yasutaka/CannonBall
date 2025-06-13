using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Soccer : PlayerController
{
    protected override void MoveUpdate()
    {
        if (SuccerTeamPointManager.Instance.m_IsFinish) { return; }

        base.MoveUpdate();
    }

    protected override void KickUpdate()
    {
        if (SuccerTeamPointManager.Instance.m_IsFinish) { return; }

        base.KickUpdate();
    }
}
