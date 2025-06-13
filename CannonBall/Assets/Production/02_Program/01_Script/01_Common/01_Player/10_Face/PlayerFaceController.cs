using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerFaceController : MonoBehaviour
{
    [SerializeField, CustomLabel("顔テクスチャのマップ")]
    private PlayerFaceMap m_faceMap;

    [SerializeField, CustomLabel("体モデルの親オブジェクト")]
    private Transform m_bodyModelParent;

    private Material m_bodyWindowMaterial;

    private Action[] m_setFaceAction;

    private const string m_matReferenceName = "_EmissiveColorMap";

    private bool[] m_faceFlag = new bool[(int)PlayerFaceMap.Face.Length]
    {
        true,
        false,
        false,
        false,
        false,
    };


    private void Start()
    {
        m_bodyWindowMaterial = m_bodyModelParent.GetChild(0).GetComponent<ModelMaterialGetter>().m_Materials[0];
        m_setFaceAction = new Action[(int)PlayerFaceMap.Face.Length]
            {
                ChangeNormalFace,
                ChangeHitFace,
                ChangeAngryFace,
                ChangePienFace,
                ChangeSadFace,
            };
    }

    private void CheckChangeFace()
    {
        for (int i = m_faceFlag.Length - 1; i >= 0; i--)
        {
            if (m_faceFlag[i])
            {
                m_setFaceAction[i]();
                break;
            }
        }
    }

    public void SetHitFace(bool flag)
    {
        m_faceFlag[(int)PlayerFaceMap.Face.Hit] = flag;
        CheckChangeFace();
    }

    public void SetAngryFace(bool flag)
    {
        m_faceFlag[(int)PlayerFaceMap.Face.Angry] = flag;
        CheckChangeFace();
    }

    public void SetPienFace(bool flag)
    {
        m_faceFlag[(int)PlayerFaceMap.Face.Pien] = flag;
        CheckChangeFace();
    }

    public void SetSadFace(bool flag)
    {
        m_faceFlag[(int)PlayerFaceMap.Face.Sad] = flag;
        CheckChangeFace();
    }

    private void ChangeNormalFace()
    {
        m_bodyWindowMaterial.SetTexture(m_matReferenceName, m_faceMap.m_Faces[(int)PlayerFaceMap.Face.Normal]);
    }

    private void ChangeHitFace()
    {
        m_bodyWindowMaterial.SetTexture(m_matReferenceName, m_faceMap.m_Faces[(int)PlayerFaceMap.Face.Hit]);
    }

    private void ChangeAngryFace()
    {
        m_bodyWindowMaterial.SetTexture(m_matReferenceName, m_faceMap.m_Faces[(int)PlayerFaceMap.Face.Angry]);
    }

    private void ChangePienFace()
    {
        m_bodyWindowMaterial.SetTexture(m_matReferenceName, m_faceMap.m_Faces[(int)PlayerFaceMap.Face.Pien]);
    }

    private void ChangeSadFace()
    {
        m_bodyWindowMaterial.SetTexture(m_matReferenceName, m_faceMap.m_Faces[(int)PlayerFaceMap.Face.Sad]);
    }
}
