using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpMoveChargeAdditionChecker : MonoBehaviour
{
    private void Update()
    {
        if (Timer_Screen.Instance.m_TimeCounter <= 60.0f)
        {
            PlayerSpMove.m_IsChargeAddition = true;
        }
    }
}
