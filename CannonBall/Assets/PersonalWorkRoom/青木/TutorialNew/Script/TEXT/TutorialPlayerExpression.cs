using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayerExpression : MonoBehaviour
{
    //[SerializeField, CustomLabel("変更するメッシュ")]
    //MeshRenderer m_meshRenderer;
    //private const string m_matReferenceName = "_EmissiveColorMap";

    //private void Start()
    //{


    //}



    public void NormalFace()
    {
        Debug.Log("通常顔状態に移動!!");
    }

    // やる気状態の顔つき
    public void MotivatedFace()
    {
        Debug.Log("やる気顔状態に移動!!");
    }

    public void PienFace()
    {
        Debug.Log("ぴえん顔状態に移動!!");
    }



}

