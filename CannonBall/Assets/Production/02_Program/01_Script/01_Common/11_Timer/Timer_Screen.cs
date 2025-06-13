using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer_Screen : Timer
{
    /// <summary> タイマーの色を変える閾値 </summary>
    [SerializeField, CustomLabel("タイマーの色を変える閾値")]
    private int m_timerColorChangeThreshold = 60;

    /// <summary> 背景のマテリアル1 </summary>
    [SerializeField, CustomLabel("背景のマテリアル1")]
    private Material m_backgroundMaterial_1;

    /// <summary> 背景のマテリアル2 </summary>
    [SerializeField, CustomLabel("背景のマテリアル2")]
    private Material m_backgroundMaterial_2;

    /// <summary> 背景のマテリアル3 </summary>
    [SerializeField, CustomLabel("背景のマテリアル3")]
    private Material m_backgroundMaterial_3;

    /// <summary> 背景の色を連動させるか </summary>
    [SerializeField, CustomLabel("背景の色を連動させるか")]
    public bool m_isBackgroundEmissionColorControl = true;

    /// <summary> 背景のエミッションカラー </summary>
    [SerializeField, CustomLabel("背景のエミッションカラー")]
    public Vector3 m_backgroundEmissionColor;

    /// <summary> ゲームセット画像 </summary>
    [SerializeField, CustomLabel("ゲームセット画像")]
    public GameObject m_gamesetImage;

    /// <summary> 生存可能域 </summary>
    [SerializeField, CustomLabel("生存可能域")]
    public GameObject m_aliveZone;

    private Animator m_animator;


    protected override void Awake()
    {
        base.Awake();

        m_animator = GetComponent<Animator>();

        SetMaterialColor();
    }

    protected override void Update()
    {
        base.Update();

        if (!m_isStopTimer)
        {
            m_animator.SetBool("IsNoTime", m_timeCounter <= m_timerColorChangeThreshold);

            SetMaterialColor();
        }
    }

    private void SetMaterialColor()
    {
        if (!m_isBackgroundEmissionColorControl) { return; }

        Color c;
        c.r = m_backgroundEmissionColor.x;
        c.g = m_backgroundEmissionColor.y;
        c.b = m_backgroundEmissionColor.z;
        c.a = 0.0f;
        m_backgroundMaterial_1.SetColor("_EmissiveColor", c);
        m_backgroundMaterial_2.SetColor("_EmissiveColor", c);
        m_backgroundMaterial_3.SetColor("_EmissiveColor", c);
    }

    /// <summary> 得点スプライト設定 </summary>
    /// <param name="value"> 設定する値 </param>
    /// <param name="parent"> 数値表示の親オブジェクト </param>
    protected override void SetNumberSprite(int value, Transform[] parent)
    {
        for (int i = 0; i < parent.Length; i++)
        {
            int temp = value;
            int j = 0;
            do
            {
                int d = temp % 10;
                parent[i].GetChild(j).GetComponent<MeshRenderer>().material.SetTexture("_MainTex", m_numberSprites[d].texture);
                temp /= 10;
                j++;
            }
            while (j < parent[i].childCount && temp != 0);

            for (; j < parent[i].childCount; j++)
            {
                parent[i].GetChild(j).GetComponent<MeshRenderer>().material.SetTexture("_MainTex", m_numberSprites[0].texture);
            }
        }
    }

    protected override void TimerEndCallback()
    {
        Time.timeScale = 0.1f;
        m_gamesetImage.SetActive(true);
        m_aliveZone.GetComponent<AliveZone_CannonFight>().Stop();
    }
}
