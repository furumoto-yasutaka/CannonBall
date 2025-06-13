using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TutorialTextRead : MonoBehaviour
{

    /// <summary> HitBlockÇ™ìñÇΩÇ¡ÇΩå¬êî </summary>
    [SerializeField, CustomReadOnly]
    private ReactiveProperty<bool> m_isReading = new ReactiveProperty<bool>();


    private TextFeed m_textFeed;


    public void SetIsReading() { m_isReading.Value = true; }

    private void Start()
    {
        m_textFeed = GetComponent<TextFeed>();


        m_isReading.Subscribe(_=>
        {

            if (m_isReading.Value == true)
            {
                m_textFeed.NextTextLine();

                m_isReading.Value = false;
            }

        }).AddTo(this);
    }
}
