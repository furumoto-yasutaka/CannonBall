using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TutorialHitBlockManager : MonoBehaviour
{

    //TutorialHitBlock[] m_tutorialHitBlocks;

    /// <summary> HitBlock������������ </summary>
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
            // ���̃J�E���g�ł����āA�z��ł͂Ȃ��̂�Length��-1�����Ȃ�
            if (m_hitCount.Value >= (transform.childCount))
            {
                // �����t���O�𗧂Ă�
                TutorialCanvasGuideController.Instance.IsSuccess();
            }
        }).AddTo(this);
    }


}
