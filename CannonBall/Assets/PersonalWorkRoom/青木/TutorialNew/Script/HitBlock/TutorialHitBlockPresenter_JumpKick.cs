using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TutorialHitBlockPresenter_JumpKick : MonoBehaviour
{
    [Header("他オブジェクトを参照")]
    [SerializeField, CustomLabel("モニター(無くてもエラー出ない)")]
    TutorialSuccessMonitor m_successMonitor;

    TutorialHitBlockManager tutorialHitBlockManager;

    [SerializeField, CustomLabel("ヒットオブジェクト")]
    TutorialKickBlock[] m_hitBlocks;

    ReactiveProperty<int> m_success = new ReactiveProperty<int>(0);

    void Start()
    {
        // 取得
        //m_hitBlocks = new ITutorialHitBlock[transform.childCount];
        //for (int i = 0; i < m_hitBlocks.Length; i++)
        //{
        //    m_hitBlocks[i] = transform.GetChild(i).GetComponent<ITutorialHitBlock>();
        //}
        // 取得
        tutorialHitBlockManager = transform.parent.GetComponent<TutorialHitBlockManager>();

        // 壁に当たったら
        foreach (var item in m_hitBlocks)
        {
            item.GetIsSuccess().Subscribe(_ =>
            {
                if (item.GetIsSuccess().Value == true)
                {
                    // 成功カウント
                    m_success.Value++;
                }
            }).AddTo(this);
        }

        // 全ての壁に当たったら
        m_success.Subscribe(_ =>
        {
            if (m_success.Value >= m_hitBlocks.Length)
            {
                // 管理クラスにカウント
                tutorialHitBlockManager.AddHitCount();

                // モニターを設定
                if (m_successMonitor != null)
                {
                    m_successMonitor.SetOKSprite();
                    m_successMonitor.SetColor();
                }

                // SE
                AudioManager.Instance.PlaySe("チュートリアル_矢印目標物", false);
            }
        }).AddTo(this);
        
    }

}
