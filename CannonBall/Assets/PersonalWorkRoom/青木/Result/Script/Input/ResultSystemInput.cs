using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSystemInput : InputElement
{
    [SerializeField]
    private ResultNextSceneCursor m_sceneCursor;

    private SceneChanger m_sceneChanger;

    private NewInputAction_Button m_inputAct;



    private void Start()
    {
        m_sceneChanger = GetComponent<SceneChanger>();

        m_inputAct = GetActionMap().GetAction_Button(NewInputActionName_ResultPlayer.Button.Submit.ToString());
    }



    private void Update()
    {
        if (m_inputAct.GetDownAll())
        {
            ResultSceneController resultScene = ResultSceneController.Instance;

            // 勝者発表状態→詳細ウィンドウ表示に変更
            if (resultScene.m_State.Value == ResultSceneController.STATE.SHOW_WINNER &&
                ResultSceneController.Instance.m_waittimeCount <= 0.0f)
            {
                resultScene.SetState(ResultSceneController.STATE.DETAIL_WINDOW);
            }
            // 詳細ウィンドウ表示→次のステージを変更する画面に変更
            else if (resultScene.m_State.Value == ResultSceneController.STATE.DETAIL_WINDOW &&
                ResultSceneController.Instance.m_waittimeCount <= 0.0f)
            {
                resultScene.SetState(ResultSceneController.STATE.NEXT_SCENE_SELECT);
            }
            else if (resultScene.m_State.Value == ResultSceneController.STATE.NEXT_SCENE_SELECT &&
                ResultSceneController.Instance.m_waittimeCount <= 0.0f)
            {
                switch (m_sceneCursor.GetCursorType())
                {
                    case ResultNextSceneCursor.CURSOR_TYPE.TO_STAGESELECT:
                        m_sceneChanger.StartSceneChange(SceneNameEnum.StageSelect);
                        break;
                    case ResultNextSceneCursor.CURSOR_TYPE.TO_TITLE:
                        m_sceneChanger.StartSceneChange(SceneNameEnum.EndScene);
                        break;
                }
            }
        }
    }

}
