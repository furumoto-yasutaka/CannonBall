using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirImpact : MonoBehaviour
{
    [SerializeField]
    float m_airKickPower;

    PlayerController m_playerController;
    Rigidbody2D m_rb;

    private void Start()
    {
        m_playerController = GetComponent<PlayerController>();
        m_rb = GetComponent<Rigidbody2D>();
    }


    public void AirKick()
    {
        Debug.Log("AirKick");

        Vector2 baseVel = -m_playerController.m_KickDir * m_airKickPower;
        Vector2 resultVel = baseVel;

        resultVel += -m_playerController.m_KickDir * m_rb.velocity.magnitude;

        m_rb.velocity = resultVel;

        // ë¨ìxÇå≥Ç…âÒì]â¡ë¨ìxÇåvéZÅEîΩâf
        //m_rb.angularVelocity = m_rb.velocity.x * -m_airKickPower;

    }

}
