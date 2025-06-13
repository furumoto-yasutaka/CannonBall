using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_Tutorial : PlayerController
{
    [Header("チュートリアル用入力制限")]
    [SerializeField, CustomLabel("スティック移動を許可")]
    private bool m_isMove = true;
    [SerializeField, CustomLabel("キック移動を許可")]
    private bool m_isKick = true;


    public bool m_IsMove { get { return m_isMove; } set { m_isMove = value; } }

    public bool m_IsKick { get { return m_isKick; } set { m_isKick = value; } }

    protected override void PlayAction()
    {
        // 移動についての更新
        if (m_isMove)
        {
            MoveUpdate();
        }
        // 蹴りについての更新
        if (m_isKick)
        {
            KickUpdate();
        }
        // 頭突きについての更新
        HeadbuttUpdate();
        // 体の回転に持っていかれないように足の方向を補正する
        m_legParent.transform.rotation =
            Quaternion.identity * Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.down, m_kickDir), Vector3.forward);

        if (m_revivalNextFrame)
        {
            // 蹴って出てくる動き
            m_playerImpact.KickPlatform();
            m_revivalNextFrame = false;
        }
    }
}
