using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombMultSpeed : MonoBehaviour
{
    [SerializeField]
    AnimationCurve m_animCurve;

    Image m_image;
    Material m_material;

    float m_timeCount;
    float m_clipTime;


    private void Start()
    {
        m_image = GetComponent<Image>();
        m_material = GetComponent<Material>();
    }

    private void Update()
    {
        m_timeCount += Time.deltaTime;

        m_image.material.SetFloat("_ClipTime", m_animCurve.Evaluate(m_timeCount / 1.5f));
    }


}
