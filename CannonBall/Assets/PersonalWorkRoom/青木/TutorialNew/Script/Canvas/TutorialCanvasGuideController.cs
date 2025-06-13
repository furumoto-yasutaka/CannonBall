using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class TutorialCanvasGuideController : SingletonMonoBehaviour<TutorialCanvasGuideController>
{
    /// <summary> チュートリアルが成功したか？ </summary>
    [SerializeField, CustomReadOnly]
    ReactiveProperty<bool> m_isTutrialSuccess = new ReactiveProperty<bool>(false);


    [SerializeField]
    UnityEvent m_Action;


    // 成功したときに使う
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
                // 登録されている関数を実行する
                m_Action.Invoke();

                // フラグを落とす
                m_isTutrialSuccess.Value = false;
            }

        }).AddTo(this);
    }
}
