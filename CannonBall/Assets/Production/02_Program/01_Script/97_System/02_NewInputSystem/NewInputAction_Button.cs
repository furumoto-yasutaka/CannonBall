/*******************************************************************************
*
*	タイトル：	抽象入力情報(ボタン)スクリプト
*	ファイル：	NewInputAction_Button.cs
*	作成者：	古本 泰隆
*	制作日：    2023/09/21
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NewInputAction_Button : NewInputAction
{
    #region enum

    /// <summary> Joyconボタンリスト </summary>
    public enum JoyconButtonId
    {
        Dpad_Up = 0,
        Dpad_Down,
        Dpad_Left,
        Dpad_Right,
        SL,
        SR,
        ZL,
        ZR,
        Minus,
        Plus,
        Stick,
        StickTilt,
        StickTilt_Up,
        StickTilt_Down,
        StickTilt_Left,
        StickTilt_Right,
        Home,
        Capture,
        
        Length,
    }

    /// <summary> Padボタンリスト </summary>
    public enum PadButtonId
    {
        Dpad_Up = 0,
        Dpad_Down,
        Dpad_Left,
        Dpad_Right,
        Y,
        A,
        X,
        B,
        LB,
        RB,
        LT,
        RT,
        Start,
        Select,
        LeftStick,
        RightStick,
        LeftStickTilt,
        LeftStickTilt_Up,
        LeftStickTilt_Down,
        LeftStickTilt_Left,
        LeftStickTilt_Right,
        RightStickTilt,
        RightStickTilt_Up,
        RightStickTilt_Down,
        RightStickTilt_Left,
        RightStickTilt_Right,

        Length,
    }

    #endregion

    /// <summary> 入力情報 </summary>
    public class InputInfo
    {
        public bool m_recvValue = false;

        public bool m_keyDown = false;

        public bool m_keyPress = false;

        public bool m_keyUp = false;
    }


    /// <summary> 関数ポインタの型宣言 </summary>
    public delegate void GetInput_Joycon(int deviceId, int i, int buttonId);

    public delegate void GetInput_Pad(int deviceId, int i);


    /// <summary> スティックを倒したとする閾値 </summary>
    private const float m_stickThreshold = 0.7f;


    /// <summary> Joyconのキー設定 </summary>
    [SerializeField]
    private List<JoyconButtonId> m_joyconActionIdList_Inspector = new List<JoyconButtonId>();

    /// <summary> Padのキー設定 </summary>
    [SerializeField]
    private List<PadButtonId> m_padActionIdList_Inspector = new List<PadButtonId>();

    private NewInputActionMap m_parentMap;

    /// <summary> 各キーの入力状況 </summary>
    private List<InputInfo>[] m_inputInfoList = new List<InputInfo>[DeviceInfo.m_DeviceNum];
    
    /// <summary> Joycon用ボタンの更新メソッド </summary>
    private List<GetInput_Joycon> m_joyconGetInputMethodList = new List<GetInput_Joycon>();

    /// <summary> Pad用ボタンの更新メソッド </summary>
    private List<GetInput_Pad> m_padGetInputMethodList = new List<GetInput_Pad>();

    /// <summary> Joycon用ボタンの更新メソッドの引数 </summary>
    private int[] m_joyconGetInputArgList = new int[(int)JoyconButtonId.Length]
    {
        (int)Joycon.Button.DPAD_UP,             // Dpad_Up
        (int)Joycon.Button.DPAD_DOWN,           // Dpad_Down
        (int)Joycon.Button.DPAD_LEFT,           // Dpad_Left
        (int)Joycon.Button.DPAD_RIGHT,          // Dpad_Right
        (int)Joycon.Button.SL,                  // SL
        (int)Joycon.Button.SR,                  // SR
        (int)Joycon.Button.SHOULDER_1,          // ZL
        (int)Joycon.Button.SHOULDER_2,          // ZR
        (int)Joycon.Button.MINUS,               // Minus
        (int)Joycon.Button.PLUS,                // Plus
        (int)Joycon.Button.STICK,               // Stick
        (int)JoyconButtonId.StickTilt,          // StickTilt
        (int)JoyconButtonId.StickTilt_Up,       // StickTilt_Up
        (int)JoyconButtonId.StickTilt_Down,     // StickTilt_Down
        (int)JoyconButtonId.StickTilt_Left,     // StickTilt_Left
        (int)JoyconButtonId.StickTilt_Right,    // StickTilt_Right
        (int)Joycon.Button.HOME,                // Home
        (int)Joycon.Button.CAPTURE,             // Capture
    };


    public override void Init()
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            m_inputInfoList[i] = new List<InputInfo>();
        }

        foreach (JoyconButtonId id in m_joyconActionIdList_Inspector)
        {
            switch (id)
            {
                case JoyconButtonId.Dpad_Up: m_joyconGetInputMethodList.Add(Update_Joycon_Common); break;
                case JoyconButtonId.Dpad_Down: m_joyconGetInputMethodList.Add(Update_Joycon_Common); break;
                case JoyconButtonId.Dpad_Left: m_joyconGetInputMethodList.Add(Update_Joycon_Common); break;
                case JoyconButtonId.Dpad_Right: m_joyconGetInputMethodList.Add(Update_Joycon_Common); break;
                case JoyconButtonId.SL: m_joyconGetInputMethodList.Add(Update_Joycon_Common); break;
                case JoyconButtonId.SR: m_joyconGetInputMethodList.Add(Update_Joycon_Common); break;
                case JoyconButtonId.ZL: m_joyconGetInputMethodList.Add(Update_Joycon_Common); break;
                case JoyconButtonId.ZR: m_joyconGetInputMethodList.Add(Update_Joycon_Common); break;
                case JoyconButtonId.Minus: m_joyconGetInputMethodList.Add(Update_Joycon_Common); break;
                case JoyconButtonId.Plus: m_joyconGetInputMethodList.Add(Update_Joycon_Common); break;
                case JoyconButtonId.Stick: m_joyconGetInputMethodList.Add(Update_Joycon_Common); break;
                case JoyconButtonId.StickTilt: m_joyconGetInputMethodList.Add(Update_Joycon_StickTilt); break;
                case JoyconButtonId.StickTilt_Up: m_joyconGetInputMethodList.Add(Update_Joycon_StickTilt); break;
                case JoyconButtonId.StickTilt_Down: m_joyconGetInputMethodList.Add(Update_Joycon_StickTilt); break;
                case JoyconButtonId.StickTilt_Left: m_joyconGetInputMethodList.Add(Update_Joycon_StickTilt); break;
                case JoyconButtonId.StickTilt_Right: m_joyconGetInputMethodList.Add(Update_Joycon_StickTilt); break;
                case JoyconButtonId.Home: m_joyconGetInputMethodList.Add(Update_Joycon_Common); break;
                case JoyconButtonId.Capture: m_joyconGetInputMethodList.Add(Update_Joycon_Common); break;
                default: continue;
            }

            for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
            {
                m_inputInfoList[i].Add(new InputInfo());
            }
        }

        foreach (PadButtonId id in m_padActionIdList_Inspector)
        {
            switch (id)
            {
                case PadButtonId.Dpad_Up: m_padGetInputMethodList.Add(Update_Pad_Dpad_Up); break;
                case PadButtonId.Dpad_Down: m_padGetInputMethodList.Add(Update_Pad_Dpad_Down); break;
                case PadButtonId.Dpad_Left: m_padGetInputMethodList.Add(Update_Pad_Dpad_Left); break;
                case PadButtonId.Dpad_Right: m_padGetInputMethodList.Add(Update_Pad_Dpad_Right); break;
                case PadButtonId.Y: m_padGetInputMethodList.Add(Update_Pad_Y); break;
                case PadButtonId.A: m_padGetInputMethodList.Add(Update_Pad_A); break;
                case PadButtonId.X: m_padGetInputMethodList.Add(Update_Pad_X); break;
                case PadButtonId.B: m_padGetInputMethodList.Add(Update_Pad_B); break;
                case PadButtonId.LB: m_padGetInputMethodList.Add(Update_Pad_LB); break;
                case PadButtonId.RB: m_padGetInputMethodList.Add(Update_Pad_RB); break;
                case PadButtonId.LT: m_padGetInputMethodList.Add(Update_Pad_LT); break;
                case PadButtonId.RT: m_padGetInputMethodList.Add(Update_Pad_RT); break;
                case PadButtonId.Start: m_padGetInputMethodList.Add(Update_Pad_Start); break;
                case PadButtonId.Select: m_padGetInputMethodList.Add(Update_Pad_Select); break;
                case PadButtonId.LeftStick: m_padGetInputMethodList.Add(Update_Pad_LeftStick); break;
                case PadButtonId.RightStick: m_padGetInputMethodList.Add(Update_Pad_RightStick); break;
                case PadButtonId.LeftStickTilt: m_padGetInputMethodList.Add(Update_Pad_LeftStick_Tilt); break;
                case PadButtonId.LeftStickTilt_Up: m_padGetInputMethodList.Add(Update_Pad_LeftStick_Tilt_Up); break;
                case PadButtonId.LeftStickTilt_Down: m_padGetInputMethodList.Add(Update_Pad_LeftStick_Tilt_Down); break;
                case PadButtonId.LeftStickTilt_Left: m_padGetInputMethodList.Add(Update_Pad_LeftStick_Tilt_Left); break;
                case PadButtonId.LeftStickTilt_Right: m_padGetInputMethodList.Add(Update_Pad_LeftStick_Tilt_Right); break;
                case PadButtonId.RightStickTilt: m_padGetInputMethodList.Add(Update_Pad_RightStick_Tilt); break;
                case PadButtonId.RightStickTilt_Up: m_padGetInputMethodList.Add(Update_Pad_RightStick_Tilt_Up); break;
                case PadButtonId.RightStickTilt_Down: m_padGetInputMethodList.Add(Update_Pad_RightStick_Tilt_Down); break;
                case PadButtonId.RightStickTilt_Left: m_padGetInputMethodList.Add(Update_Pad_RightStick_Tilt_Left); break;
                case PadButtonId.RightStickTilt_Right: m_padGetInputMethodList.Add(Update_Pad_RightStick_Tilt_Right); break;
                default: return;
            }

            for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
            {
                m_inputInfoList[i].Add(new InputInfo());
            }
        }
    }

    public void InitMap(NewInputActionMap map)
    {
        m_parentMap = map;
    }

    public override void Update()
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            Update_Input(i);
        }
    }

    private void Update_Input(int i)
    {
        if (JoyconManager.Instance.GetIsEntry(i))
        {
            for (int j = 0; j < m_joyconActionIdList_Inspector.Count; j++)
            {
                int buttonId = m_joyconGetInputArgList[(int)m_joyconActionIdList_Inspector[j]];
                m_joyconGetInputMethodList[j](i, j, buttonId);
            }
        }
        else
        {
            Update_InputReset_Joycon(i);
        }

        if (PadManager.Instance.GetIsEntry(i))
        {
            for (int j = 0; j < m_padActionIdList_Inspector.Count; j++)
            {
                m_padGetInputMethodList[j](i, m_joyconActionIdList_Inspector.Count + j);
            }
        }
        else
        {
            Update_InputReset_Pad(i);
        }
    }

    private void Update_InputReset_Joycon(int deviceId)
    {
        for (int j = 0; j < m_joyconActionIdList_Inspector.Count; j++)
        {
            Update_InputReset(deviceId, j);
        }
    }

    private void Update_InputReset_Pad(int deviceId)
    {
        for (int j = 0; j < m_padActionIdList_Inspector.Count; j++)
        {
            Update_InputReset(deviceId, m_joyconActionIdList_Inspector.Count + j);
        }
    }

    public override void Reset()
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            for (int j = 0; j < m_joyconActionIdList_Inspector.Count; j++)
            {
                Update_InputReset(i, j);
            }
            for (int j = 0; j < m_padActionIdList_Inspector.Count; j++)
            {
                Update_InputReset(i, m_joyconActionIdList_Inspector.Count + j);
            }
        }
    }

    protected override void Update_InputReset(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i].m_recvValue = false;
        m_inputInfoList[deviceId][i].m_keyDown = false;
        m_inputInfoList[deviceId][i].m_keyPress = false;
        m_inputInfoList[deviceId][i].m_keyUp = false;
    }

    private void SetInputStickTilt(int deviceId, int i, bool down, bool isPress)
    {
        m_inputInfoList[deviceId][i].m_keyDown = down;
        m_inputInfoList[deviceId][i].m_keyPress = isPress;
        m_inputInfoList[deviceId][i].m_keyUp = m_inputInfoList[deviceId][i].m_recvValue && !isPress;
        m_inputInfoList[deviceId][i].m_recvValue = isPress;
    }


    //--------------------取得処理--------------------

    /// <summary>
    /// 押された瞬間かどうか取得(特定のデバイスから)
    /// </summary>
    /// <param name="deviceId"> ロックするグループの情報 </param>
    public bool GetDown(int deviceId)
    {
        foreach (InputInfo info in m_inputInfoList[deviceId])
        {
            if (info.m_keyDown)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 押された瞬間かどうか取得(全てのデバイスから)
    /// </summary>
    public bool GetDownAll()
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            foreach (InputInfo info in m_inputInfoList[i])
            {
                if (info.m_keyDown)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 押されているかどうか取得(特定のデバイスから)
    /// </summary>
    /// <param name="deviceId"> ロックするグループの情報 </param>
    public bool GetPress(int deviceId)
    {
        foreach (InputInfo info in m_inputInfoList[deviceId])
        {
            if (info.m_keyPress)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 押されているかどうか取得(全てのデバイスから)
    /// </summary>
    public bool GetPressAll()
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            foreach (InputInfo info in m_inputInfoList[i])
            {
                if (info.m_keyPress)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 離した瞬間かどうか取得(特定のデバイスから)
    /// </summary>
    /// <param name="deviceId"> ロックするグループの情報 </param>
    public bool GetUp(int deviceId)
    {
        foreach (InputInfo info in m_inputInfoList[deviceId])
        {
            if (info.m_keyUp)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 離した瞬間かどうか取得(全てのデバイスから)
    /// </summary>
    public bool GetUpAll()
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            foreach (InputInfo info in m_inputInfoList[i])
            {
                if (info.m_keyUp)
                {
                    return true;
                }
            }
        }

        return false;
    }


    //--------------------更新処理(Joycon)--------------------

    private void Update_Joycon_Common(int deviceId, int i, int buttonId)
    {
        m_inputInfoList[deviceId][i].m_keyDown
            = JoyconManager.Instance.GetDevice(deviceId).GetButtonDown((Joycon.Button)buttonId);
        m_inputInfoList[deviceId][i].m_keyPress
            = JoyconManager.Instance.GetDevice(deviceId).GetButton((Joycon.Button)buttonId);
        m_inputInfoList[deviceId][i].m_keyUp
            = JoyconManager.Instance.GetDevice(deviceId).GetButtonUp((Joycon.Button)buttonId);
        m_inputInfoList[deviceId][i].m_recvValue = m_inputInfoList[deviceId][i].m_keyPress;
    }

    private void Update_Joycon_StickTilt(int deviceId, int i, int buttonId)
    {
        float value;

        switch (buttonId)
        {
            case (int)JoyconButtonId.StickTilt:
                value = new Vector2(
                    JoyconManager.Instance.GetDevice(deviceId).GetStick()[0],
                    JoyconManager.Instance.GetDevice(deviceId).GetStick()[1]).sqrMagnitude;
                break;
            case (int)JoyconButtonId.StickTilt_Up:
            case (int)JoyconButtonId.StickTilt_Down:
                value = JoyconManager.Instance.GetDevice(deviceId).GetStick()[1];
                value *= value;
                break;
            case (int)JoyconButtonId.StickTilt_Left:
            case (int)JoyconButtonId.StickTilt_Right:
                value = JoyconManager.Instance.GetDevice(deviceId).GetStick()[0];
                value *= value;
                break;
            default:
                value = 0.0f;
                break;
        }

        bool isPress = Mathf.Abs(value) > m_stickThreshold * m_stickThreshold;
        bool down = !m_inputInfoList[deviceId][i].m_recvValue && isPress;
        SetInputStickTilt(deviceId, i, down, isPress);
    }


    //--------------------更新処理(Pad)--------------------

    private void Update_Pad_Dpad_Up(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i].m_keyDown
            = PadManager.Instance.GetDevice(deviceId).dpad.up.wasPressedThisFrame;
        m_inputInfoList[deviceId][i].m_keyPress
            = PadManager.Instance.GetDevice(deviceId).dpad.up.isPressed;
        m_inputInfoList[deviceId][i].m_keyUp
            = PadManager.Instance.GetDevice(deviceId).dpad.up.wasReleasedThisFrame;
        m_inputInfoList[deviceId][i].m_recvValue = m_inputInfoList[deviceId][i].m_keyPress;
    }

    private void Update_Pad_Dpad_Down(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i].m_keyDown
            = PadManager.Instance.GetDevice(deviceId).dpad.down.wasPressedThisFrame;
        m_inputInfoList[deviceId][i].m_keyPress
            = PadManager.Instance.GetDevice(deviceId).dpad.down.isPressed;
        m_inputInfoList[deviceId][i].m_keyUp
            = PadManager.Instance.GetDevice(deviceId).dpad.down.wasReleasedThisFrame;
        m_inputInfoList[deviceId][i].m_recvValue = m_inputInfoList[deviceId][i].m_keyPress;
    }

    private void Update_Pad_Dpad_Left(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i].m_keyDown
            = PadManager.Instance.GetDevice(deviceId).dpad.left.wasPressedThisFrame;
        m_inputInfoList[deviceId][i].m_keyPress
            = PadManager.Instance.GetDevice(deviceId).dpad.left.isPressed;
        m_inputInfoList[deviceId][i].m_keyUp
            = PadManager.Instance.GetDevice(deviceId).dpad.left.wasReleasedThisFrame;
        m_inputInfoList[deviceId][i].m_recvValue = m_inputInfoList[deviceId][i].m_keyPress;
    }

    private void Update_Pad_Dpad_Right(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i].m_keyDown
            = PadManager.Instance.GetDevice(deviceId).dpad.right.wasPressedThisFrame;
        m_inputInfoList[deviceId][i].m_keyPress
            = PadManager.Instance.GetDevice(deviceId).dpad.right.isPressed;
        m_inputInfoList[deviceId][i].m_keyUp
            = PadManager.Instance.GetDevice(deviceId).dpad.right.wasReleasedThisFrame;
        m_inputInfoList[deviceId][i].m_recvValue = m_inputInfoList[deviceId][i].m_keyPress;
    }

    private void Update_Pad_Y(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i].m_keyDown
            = PadManager.Instance.GetDevice(deviceId).yButton.wasPressedThisFrame;
        m_inputInfoList[deviceId][i].m_keyPress
            = PadManager.Instance.GetDevice(deviceId).yButton.isPressed;
        m_inputInfoList[deviceId][i].m_keyUp
            = PadManager.Instance.GetDevice(deviceId).yButton.wasReleasedThisFrame;
        m_inputInfoList[deviceId][i].m_recvValue = m_inputInfoList[deviceId][i].m_keyPress;
    }

    private void Update_Pad_A(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i].m_keyDown
            = PadManager.Instance.GetDevice(deviceId).aButton.wasPressedThisFrame;
        m_inputInfoList[deviceId][i].m_keyPress
            = PadManager.Instance.GetDevice(deviceId).aButton.isPressed;
        m_inputInfoList[deviceId][i].m_keyUp
            = PadManager.Instance.GetDevice(deviceId).aButton.wasReleasedThisFrame;
        m_inputInfoList[deviceId][i].m_recvValue = m_inputInfoList[deviceId][i].m_keyPress;
    }

    private void Update_Pad_X(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i].m_keyDown
            = PadManager.Instance.GetDevice(deviceId).xButton.wasPressedThisFrame;
        m_inputInfoList[deviceId][i].m_keyPress
            = PadManager.Instance.GetDevice(deviceId).xButton.isPressed;
        m_inputInfoList[deviceId][i].m_keyUp
            = PadManager.Instance.GetDevice(deviceId).xButton.wasReleasedThisFrame;
        m_inputInfoList[deviceId][i].m_recvValue = m_inputInfoList[deviceId][i].m_keyPress;
    }

    private void Update_Pad_B(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i].m_keyDown
            = PadManager.Instance.GetDevice(deviceId).bButton.wasPressedThisFrame;
        m_inputInfoList[deviceId][i].m_keyPress
            = PadManager.Instance.GetDevice(deviceId).bButton.isPressed;
        m_inputInfoList[deviceId][i].m_keyUp
            = PadManager.Instance.GetDevice(deviceId).bButton.wasReleasedThisFrame;
        m_inputInfoList[deviceId][i].m_recvValue = m_inputInfoList[deviceId][i].m_keyPress;
    }

    private void Update_Pad_LB(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i].m_keyDown
            = PadManager.Instance.GetDevice(deviceId).leftShoulder.wasPressedThisFrame;
        m_inputInfoList[deviceId][i].m_keyPress
            = PadManager.Instance.GetDevice(deviceId).leftShoulder.isPressed;
        m_inputInfoList[deviceId][i].m_keyUp
            = PadManager.Instance.GetDevice(deviceId).leftShoulder.wasReleasedThisFrame;
        m_inputInfoList[deviceId][i].m_recvValue = m_inputInfoList[deviceId][i].m_keyPress;
    }

    private void Update_Pad_RB(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i].m_keyDown
            = PadManager.Instance.GetDevice(deviceId).rightShoulder.wasPressedThisFrame;
        m_inputInfoList[deviceId][i].m_keyPress
            = PadManager.Instance.GetDevice(deviceId).rightShoulder.isPressed;
        m_inputInfoList[deviceId][i].m_keyUp
            = PadManager.Instance.GetDevice(deviceId).rightShoulder.wasReleasedThisFrame;
        m_inputInfoList[deviceId][i].m_recvValue = m_inputInfoList[deviceId][i].m_keyPress;
    }

    private void Update_Pad_LT(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i].m_keyDown
            = PadManager.Instance.GetDevice(deviceId).leftTrigger.wasPressedThisFrame;
        m_inputInfoList[deviceId][i].m_keyPress
            = PadManager.Instance.GetDevice(deviceId).leftTrigger.isPressed;
        m_inputInfoList[deviceId][i].m_keyUp
            = PadManager.Instance.GetDevice(deviceId).leftTrigger.wasReleasedThisFrame;
        m_inputInfoList[deviceId][i].m_recvValue = m_inputInfoList[deviceId][i].m_keyPress;
    }

    private void Update_Pad_RT(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i].m_keyDown
            = PadManager.Instance.GetDevice(deviceId).rightTrigger.wasPressedThisFrame;
        m_inputInfoList[deviceId][i].m_keyPress
            = PadManager.Instance.GetDevice(deviceId).rightTrigger.isPressed;
        m_inputInfoList[deviceId][i].m_keyUp
            = PadManager.Instance.GetDevice(deviceId).rightTrigger.wasReleasedThisFrame;
        m_inputInfoList[deviceId][i].m_recvValue = m_inputInfoList[deviceId][i].m_keyPress;
    }

    private void Update_Pad_Start(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i].m_keyDown
            = PadManager.Instance.GetDevice(deviceId).startButton.wasPressedThisFrame;
        m_inputInfoList[deviceId][i].m_keyPress
            = PadManager.Instance.GetDevice(deviceId).startButton.isPressed;
        m_inputInfoList[deviceId][i].m_keyUp
            = PadManager.Instance.GetDevice(deviceId).startButton.wasReleasedThisFrame;
        m_inputInfoList[deviceId][i].m_recvValue = m_inputInfoList[deviceId][i].m_keyPress;
    }

    private void Update_Pad_Select(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i].m_keyDown
            = PadManager.Instance.GetDevice(deviceId).selectButton.wasPressedThisFrame;
        m_inputInfoList[deviceId][i].m_keyPress
            = PadManager.Instance.GetDevice(deviceId).selectButton.isPressed;
        m_inputInfoList[deviceId][i].m_keyUp
            = PadManager.Instance.GetDevice(deviceId).selectButton.wasReleasedThisFrame;
        m_inputInfoList[deviceId][i].m_recvValue = m_inputInfoList[deviceId][i].m_keyPress;
    }

    private void Update_Pad_LeftStick(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i].m_keyDown
            = PadManager.Instance.GetDevice(deviceId).leftStickButton.wasPressedThisFrame;
        m_inputInfoList[deviceId][i].m_keyPress
            = PadManager.Instance.GetDevice(deviceId).leftStickButton.isPressed;
        m_inputInfoList[deviceId][i].m_keyUp
            = PadManager.Instance.GetDevice(deviceId).leftStickButton.wasReleasedThisFrame;
        m_inputInfoList[deviceId][i].m_recvValue = m_inputInfoList[deviceId][i].m_keyPress;
    }

    private void Update_Pad_RightStick(int deviceId, int i)
    {
        m_inputInfoList[deviceId][i].m_keyDown
            = PadManager.Instance.GetDevice(deviceId).rightStickButton.wasPressedThisFrame;
        m_inputInfoList[deviceId][i].m_keyPress
            = PadManager.Instance.GetDevice(deviceId).rightStickButton.isPressed;
        m_inputInfoList[deviceId][i].m_keyUp
            = PadManager.Instance.GetDevice(deviceId).rightStickButton.wasReleasedThisFrame;
        m_inputInfoList[deviceId][i].m_recvValue = m_inputInfoList[deviceId][i].m_keyPress;
    }

    private void Update_Pad_LeftStick_Tilt(int deviceId, int i)
    {
        float value = PadManager.Instance.GetDevice(deviceId).leftStick.value.sqrMagnitude;
        bool isPress = value > m_stickThreshold * m_stickThreshold;
        bool down = !m_inputInfoList[deviceId][i].m_recvValue && isPress;
        SetInputStickTilt(deviceId, i, down, isPress);
    }

    private void Update_Pad_LeftStick_Tilt_Up(int deviceId, int i)
    {
        bool isPress = PadManager.Instance.GetDevice(deviceId).leftStick.up.value > m_stickThreshold * m_stickThreshold;
        bool down = !m_inputInfoList[deviceId][i].m_recvValue && isPress;
        SetInputStickTilt(deviceId, i, down, isPress);
    }

    private void Update_Pad_LeftStick_Tilt_Down(int deviceId, int i)
    {
        bool isPress = PadManager.Instance.GetDevice(deviceId).leftStick.down.value > m_stickThreshold * m_stickThreshold;
        bool down = !m_inputInfoList[deviceId][i].m_recvValue && isPress;
        SetInputStickTilt(deviceId, i, down, isPress);
    }

    private void Update_Pad_LeftStick_Tilt_Left(int deviceId, int i)
    {
        bool isPress = PadManager.Instance.GetDevice(deviceId).leftStick.left.value > m_stickThreshold * m_stickThreshold;
        bool down = !m_inputInfoList[deviceId][i].m_recvValue && isPress;
        SetInputStickTilt(deviceId, i, down, isPress);
    }

    private void Update_Pad_LeftStick_Tilt_Right(int deviceId, int i)
    {
        bool isPress = PadManager.Instance.GetDevice(deviceId).leftStick.right.value > m_stickThreshold * m_stickThreshold;
        bool down = !m_inputInfoList[deviceId][i].m_recvValue && isPress;
        SetInputStickTilt(deviceId, i, down, isPress);
    }

    private void Update_Pad_RightStick_Tilt(int deviceId, int i)
    {
        float value = PadManager.Instance.GetDevice(deviceId).rightStick.value.sqrMagnitude;
        bool isPress = value > m_stickThreshold * m_stickThreshold;
        bool down = !m_inputInfoList[deviceId][i].m_recvValue && isPress;
        SetInputStickTilt(deviceId, i, down, isPress);
    }

    private void Update_Pad_RightStick_Tilt_Up(int deviceId, int i)
    {
        bool isPress = PadManager.Instance.GetDevice(deviceId).rightStick.up.value > m_stickThreshold * m_stickThreshold;
        bool down = !m_inputInfoList[deviceId][i].m_recvValue && isPress;
        SetInputStickTilt(deviceId, i, down, isPress);
    }

    private void Update_Pad_RightStick_Tilt_Down(int deviceId, int i)
    {
        bool isPress = PadManager.Instance.GetDevice(deviceId).rightStick.down.value > m_stickThreshold * m_stickThreshold;
        bool down = !m_inputInfoList[deviceId][i].m_recvValue && isPress;
        SetInputStickTilt(deviceId, i, down, isPress);
    }

    private void Update_Pad_RightStick_Tilt_Left(int deviceId, int i)
    {
        bool isPress = PadManager.Instance.GetDevice(deviceId).rightStick.left.value > m_stickThreshold * m_stickThreshold;
        bool down = !m_inputInfoList[deviceId][i].m_recvValue && isPress;
        SetInputStickTilt(deviceId, i, down, isPress);
    }

    private void Update_Pad_RightStick_Tilt_Right(int deviceId, int i)
    {
        bool isPress = PadManager.Instance.GetDevice(deviceId).rightStick.right.value > m_stickThreshold * m_stickThreshold;
        bool down = !m_inputInfoList[deviceId][i].m_recvValue && isPress;
        SetInputStickTilt(deviceId, i, down, isPress);
    }
}
