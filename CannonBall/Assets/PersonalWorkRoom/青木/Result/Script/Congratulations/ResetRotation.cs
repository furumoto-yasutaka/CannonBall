using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRotation : MonoBehaviour
{
    private void ResetFunction()
    {
        Debug.Log("�O���Z�b" + transform.localRotation);
        transform.localRotation = Quaternion.identity;
        transform.localEulerAngles = Vector3.zero;
        Debug.Log("���Z�b�g" + transform.localRotation);
    }
}
