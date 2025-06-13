using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandGetPlayerPosition : MonoBehaviour
{
    [SerializeField]
    Transform m_playerPoint;

    public Vector3 GetPoint() { return m_playerPoint.position; }

}
