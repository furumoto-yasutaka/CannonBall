using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGuideId : MonoBehaviour
{

    [SerializeField]
    StageChangeManager.StageType m_stageType;

    public StageChangeManager.StageType GetStageType() { return m_stageType; }
}
