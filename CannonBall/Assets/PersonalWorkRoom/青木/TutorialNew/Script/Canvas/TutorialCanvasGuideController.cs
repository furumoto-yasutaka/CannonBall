using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class TutorialCanvasGuideController : SingletonMonoBehaviour<TutorialCanvasGuideController>
{
    /// <summary> �`���[�g���A���������������H </summary>
    [SerializeField, CustomReadOnly]
    ReactiveProperty<bool> m_isTutrialSuccess = new ReactiveProperty<bool>(false);


    [SerializeField]
    UnityEvent m_Action;


    // ���������Ƃ��Ɏg��
    public void IsSuccess() { m_isTutrialSuccess.Value = true; }


    protected override void Awake()
    {
        dontDestroyOnLoad = false;
        base.Awake();
    }

    private void Start()
    {
        m_isTutrialSuccess.Subscribe(_ =>
        {
            if (m_isTutrialSuccess.Value)
            {
                // �o�^����Ă���֐������s����
                m_Action.Invoke();

                // �t���O�𗎂Ƃ�
                m_isTutrialSuccess.Value = false;
            }

        }).AddTo(this);
    }
}
