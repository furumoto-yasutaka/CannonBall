using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTargetSelector : MonoBehaviour
{
    // バーチャルカメラ
    [SerializeField] private CinemachineVirtualCamera m_virtualCamera;

    // 追従対象リスト
    [SerializeField] private Transform[] m_targetTransform;


    [SerializeField] private bool m_isChange = false;


    // 選択中のターゲットのインデックス
    private int m_currentTarget = 0;

    // フレーム更新
    private void Update()
    {
        // 追従対象情報が設定されていなければ、何もしない
        if (m_targetTransform == null || m_targetTransform.Length <= 0)
            return;

        // マウスクリックされたら
        if (m_isChange)
        {
            // 追従対象を順番に切り替え
            if (++m_currentTarget >= m_targetTransform.Length)
                m_currentTarget = 0;

            // 追従対象を更新
            var info = m_targetTransform[m_currentTarget];
            m_virtualCamera.LookAt = info;


            m_isChange = false;
        }
    }

    public void ChangeViewTarget(int _targetNum)
    {
        // 追従対象を順番に切り替え
        m_currentTarget = _targetNum;
        m_currentTarget %= m_targetTransform.Length;

        // 追従対象を更新
        var info = m_targetTransform[m_currentTarget];
        m_virtualCamera.LookAt = info;

    }

}
