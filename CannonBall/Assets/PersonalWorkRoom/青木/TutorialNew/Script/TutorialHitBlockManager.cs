using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TutorialHitBlockManager : MonoBehaviour
{

    //TutorialHitBlock[] m_tutorialHitBlocks;

    /// <summary> HitBlockが当たった個数 </summary>
    [SerializeField, CustomReadOnly]
    private ReactiveProperty<int> m_hitCount = new ReactiveProperty<int>();


    public void AddHitCount() { m_hitCount.Value++; }

    private void Awake()
    {
        m_hitCount.Value = 0;
    }


    private void Start()
    {
        //m_tutorialHitBlocks = new TutorialHitBlock[transform.childCount];
        //for (int i = 0; i  < m_tutorialHitBlocks.Length; i++) 
        //{
        //    m_tutorialHitBlocks[i] = transform.GetChild(i).GetComponent<TutorialHitBlock>();
        //}

        //m_HitCount = m_hitCount;

        m_hitCount.Subscribe(_ =>
        {
            // 個数のカウントであって、配列ではないのでLengthに-1をしない
            if (m_hitCount.Value >= (transform.childCount))
            {
                // 成功フラグを立てる
                TutorialCanvasGuideController.Instance.IsSuccess();
            }
        }).AddTo(this);
    }


}
