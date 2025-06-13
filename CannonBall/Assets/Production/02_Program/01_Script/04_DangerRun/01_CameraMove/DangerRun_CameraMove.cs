/*******************************************************************************
*
*	タイトル：	強制スクロールのカメラ移動スクリプト
*	ファイル：	DangerRun_CameraMove.cs
*	作成者：	古本 泰隆
*	制作日：    2023/10/06
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DangerRun_CameraMove : MonoBehaviour
{
    /// <summary> 区間情報の親オブジェクト </summary>
    [SerializeField, CustomLabel("区間情報の親オブジェクト")]
    private Transform m_sectionParent;

    /// <summary> 侵入不可壁の親オブジェクト </summary>
    [SerializeField, CustomLabel("侵入不可壁の親オブジェクト")]
    private Transform m_wallParent;

    /// <summary> デッドゾーンの親オブジェクト </summary>
    [SerializeField, CustomLabel("デッドゾーンの親オブジェクト")]
    private Transform m_deadzoneParent;

    /// <summary> 現在の区間の移動を開始して経った時間 </summary>
    [SerializeField, CustomLabelReadOnly("時間")]
    private float m_sectionTime = 0.0f;

    /// <summary> 区間ポイント番号(0~) </summary>
    private int m_sectionNumber = 0;

    /// <summary> 区間情報 </summary>
    private DangerRun_CameraMoveSource m_sectionSource;

    private bool m_isMove = false;

    /// <summary> 全ての区間を通り終えたかどうか </summary>
    private bool m_isFinish = false;

    public bool m_IsFinish { get { return m_isFinish; } }


    private void Start()
    {
        // 各区間の初期化
        InitSections();
    }

    private void Update()
    {
        if (m_isFinish || !m_isMove) { return; }

        m_sectionTime += Time.deltaTime;

        float overtime = 0.0f;
        bool isSectionFinish = false;
        // 座標取得
        Vector3 pos = m_sectionSource.GetPosition(m_sectionTime, ref isSectionFinish, ref overtime);

        // 区間を通り終わり、超過があったら超過分の座標移動を行う
        while (isSectionFinish && !m_isFinish)
        {
            isSectionFinish = false;

            // 次の区間に切り替える準備
            m_sectionNumber++;

            if (m_sectionNumber < m_sectionParent.childCount - 1)
            {// 次の区間に切り替え
                m_sectionTime = overtime;
                overtime = 0.0f;
                // 次の区間情報を取得
                ChangeSection();
                // 座標取得
                pos = m_sectionSource.GetPosition(m_sectionTime, ref isSectionFinish, ref overtime);
            }
            else
            {// 全ての区間を通り終えた
                m_isFinish = true;
            }
        }

        // カメラの座標に反映
        transform.position = pos;
    }

    public void StartMove()
    {
        m_isMove = true;
    }

    /// <summary> 各区間の初期化 </summary>
    private void InitSections()
    {
        for (int i = 0; i < m_sectionParent.childCount - 1; i++)
        {
            m_sectionParent.GetChild(i).GetComponent<DangerRun_CameraMoveSource>().InitParam(i);
        }

        m_sectionSource = m_sectionParent.GetChild(m_sectionNumber).GetComponent<DangerRun_CameraMoveSource>();
        // 次の区間の壁のペアレントをカメラに
        Transform temp = m_sectionSource.transform.GetChild(0);
        while (temp.childCount > 0)
        {
            temp.GetChild(0).parent = transform.GetChild(2);
        }
        // 次の区間の死亡域を有効化し、ペアレントをカメラに
        temp = m_sectionSource.transform.GetChild(1);
        while (temp.childCount > 0)
        {
            temp.GetChild(0).gameObject.SetActive(true);
            temp.GetChild(0).parent = transform.GetChild(3);
        }
    }

    /// <summary> 区間情報を取得 </summary>
    private void ChangeSection()
    {
        if (m_sectionSource.m_SectionNumber != m_sectionNumber)
        {
            DangerRun_CameraMoveSource next = m_sectionParent.GetChild(m_sectionNumber).GetComponent<DangerRun_CameraMoveSource>();

            // 前の区間の壁を削除
            for (int i = 0; i < m_wallParent.childCount; i++)
            {
                Destroy(m_wallParent.GetChild(0).gameObject);
            }
            // 前の区間の死亡域のペアレントを元に戻す
            if (next.m_IsReplaceDeadZone)
            {
                for (int i = 0; i < m_deadzoneParent.childCount; i++)
                {
                    m_deadzoneParent.GetChild(0).parent = m_sectionSource.transform.GetChild(1);
                }
            }

            // 区間情報更新
            m_sectionSource = next;
            m_sectionSource.InvokeCallback();

            // 次の区間の壁のペアレントをカメラに
            Transform temp = m_sectionSource.transform.GetChild(0);
            for (int i = 0; i < temp.childCount; i++)
            {
                temp.GetChild(0).gameObject.SetActive(true);
                temp.GetChild(0).parent = m_wallParent;
            }
            // 次の区間の死亡域を有効化し、ペアレントをカメラに
            temp = m_sectionSource.transform.GetChild(1);
            for (int i = 0; i < temp.childCount; i++)
            {
                temp.GetChild(0).gameObject.SetActive(true);
                temp.GetChild(0).parent = m_deadzoneParent;
            }
            if (m_sectionSource.m_IsSpeedEvent)
            {
                SpeedUpController.Instance.Play();
                AudioManager.Instance.PlaySe("デンジャラン_スピードアップ", false);
            }
        }
    }
}
