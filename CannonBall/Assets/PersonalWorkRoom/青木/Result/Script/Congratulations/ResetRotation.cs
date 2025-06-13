using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRotation : MonoBehaviour
{
    private void ResetFunction()
    {
        Debug.Log("前リセッ" + transform.localRotation);
        transform.localRotation = Quaternion.identity;
        transform.localEulerAngles = Vector3.zero;
        Debug.Log("リセット" + transform.localRotation);
    }
}
