using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialProgress : SingletonMonoBehaviour<TutorialProgress>
{
	enum STATE
	{
        RIGHT_MOVE,
        LEFT_MOVE,
        JUMP,
        JUMP_KICK,
        STICK_MOVE,
        FIGHT
    }


    static public int m_TutorialProgressCount = 0;
}
