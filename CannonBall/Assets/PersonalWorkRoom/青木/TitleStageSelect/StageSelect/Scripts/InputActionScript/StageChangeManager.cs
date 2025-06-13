using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StageChangeManager;

public class StageChangeManager : SingletonMonoBehaviour<StageChangeManager>
{
    public enum StageType
    {
        FightGame = 1,
        BombGame,
        MagumaGame,

        Length
    }


    StageType m_stageType = StageType.FightGame;

    [SerializeField]
    Animator m_CameraAnimator;

    [SerializeField]
    private PlayGuideCursorInput m_guideCursorInput;

    public StageType GetCurrentStage() { return m_stageType; }



    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();
    }


    private void Start()
    {
        //m_CameraAnimator = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<Animator>();
    }


    private void Update()
    {
        StageType stageType = m_stageType;

        // 変えていたら変更
        if (stageType != m_stageType)
        {
            if (stageType > m_stageType)
            {
                m_CameraAnimator.SetInteger("State", (int)m_stageType + (int)StageType.Length);
            }
            else if (stageType < m_stageType)
            {
                m_CameraAnimator.SetInteger("State", (int)m_stageType);
            }
        }
    }




    public void InputUp()
    {
        StageType stageType = m_stageType;

        m_stageType--;
        m_stageType = (StageType)Mathf.Clamp((int)m_stageType, (int)StageType.FightGame, (int)StageType.Length - 1);

        // SE
        SelectSE(stageType);

        CameraAnimation(stageType);
    }

    public void InputDown()
    {
        StageType stageType = m_stageType;

        m_stageType++;
        m_stageType = (StageType)Mathf.Clamp((int)m_stageType, (int)StageType.FightGame, (int)StageType.Length - 1);

        // SE
        SelectSE(stageType);

        CameraAnimation(stageType);
    }


    private void CameraAnimation(StageType _stageType)
    {
        // 変えていたら変更
        if (_stageType != m_stageType)
        {

            m_guideCursorInput.SetState(PlayGuideCursorInput.STATE.LOGO);


            if (_stageType > m_stageType)
            {
                m_CameraAnimator.SetInteger("State", (int)m_stageType + (int)StageType.Length);
            }
            else if (_stageType < m_stageType)
            {
                m_CameraAnimator.SetInteger("State", (int)m_stageType);
            }
        }
    }

    private void SelectSE(StageType _stageType)
    {
        if (m_stageType != _stageType)
        {
            AudioManager.Instance.PlaySe("ステージセレクト画面縦移動", false);
        }
    }
}
