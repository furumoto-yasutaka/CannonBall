/*******************************************************************************
*
*	�^�C�g���F	�R���g���[���[�U���Ǘ��V���O���g���X�N���v�g
*	�t�@�C���F	VibrationManager.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/07
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VibrationManager : SingletonMonoBehaviour<VibrationManager>
{
    [System.Serializable]
    public class VibrationInfo
    {
        public int m_time = 0;
        public float m_power = 0.0f;
    }

    [System.Serializable]
    public class VibrationInfoArray
    {
        public List<VibrationInfo> m_info = new List<VibrationInfo>();
    }


    /// <summary> Joycon�U���̒���g�� </summary>
    [SerializeField, CustomLabel("Joycon�U���̒���g��")]
    private float m_freqLow = 600.0f;

    /// <summary> Joycon�U���̍����g�� </summary>
    [SerializeField, CustomLabel("Joycon�U���̍����g��")]
    private float m_freqHigh = 600.0f;

    /// <summary> ���ݍ쓮���̃o�C�u���[�V������� </summary>
    [SerializeField, CustomLabel("���ݍ쓮���̃o�C�u���[�V�������")]
    private VibrationInfo[] m_currentInfo = new VibrationInfo[DeviceInfo.m_DeviceNum];

    /// <summary> �쓮�҂��̃o�C�u���[�V������� </summary>
    [SerializeField, CustomLabel("�쓮�҂��̃o�C�u���[�V�������")]
    private VibrationInfoArray[] m_waitInfoList = new VibrationInfoArray[DeviceInfo.m_DeviceNum];


    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < m_waitInfoList.Length; i++)
        {
            m_waitInfoList[i].m_info = new List<VibrationInfo>();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            CurrentInfoUpdate(i);
            WaitInfoUpdate(i);
        }
    }

    /// <summary>
    /// ���ݍ쓮���̐U�����̍X�V
    /// </summary>
    /// <param name="playerId"> �v���C���[�ԍ� </param>
    private void CurrentInfoUpdate(int playerId)
    {
        if (m_currentInfo[playerId].m_time == 0.0f) { return; }

        m_currentInfo[playerId].m_time--;
        if (m_currentInfo[playerId].m_time <= 0)
        {
            if (m_waitInfoList[playerId].m_info.Count > 0)
            {
                m_currentInfo[playerId] = m_waitInfoList[playerId].m_info[0];
                m_waitInfoList[playerId].m_info.RemoveAt(0);

                // �U��������
                SetVibration(playerId);
            }
            else
            {
                m_currentInfo[playerId] = null;
                StopVibration(playerId);
            }
        }
    }

    /// <summary>
    /// ���ݑҋ@���̐U�����̍X�V
    /// </summary>
    /// <param name="playerId"> �v���C���[�ԍ� </param>
    private void WaitInfoUpdate(int playerId)
    {
        int j = 0;
        while (j < m_waitInfoList[playerId].m_info.Count)
        {
            m_waitInfoList[playerId].m_info[j].m_time--;
            if (m_waitInfoList[playerId].m_info[j].m_time <= 0)
            {
                m_waitInfoList[playerId].m_info.RemoveAt(j);
            }
            else
            {
                j++;
            }
        }
    }

    /// <summary>
    /// �o�C�u���[�V�����쓮
    /// </summary>
    /// <param name="playerId"> �ǂ̃v���C���[�ɑΉ������f�o�C�X��U�������邩 </param>
    /// <param name="time"> �U������(�t���[��) </param>
    /// <param name="power"> �U���̋���(0.0~1.0) </param>
    public void SetVibration(int playerId, int time, float power)
    {
        VibrationInfo info = new VibrationInfo();
        info.m_power = power;
        info.m_time = time;

        RegistInfo(info, playerId);

        SetVibration(playerId);
    }

    /// <summary>
    /// �o�C�u���[�V�����쓮
    /// </summary>
    /// <param name="playerId"> �ǂ̃v���C���[�ɑΉ������f�o�C�X��U�������邩 </param>
    private void SetVibration(int playerId)
    {
        // Joycon
        if (JoyconConnector.Instance.m_IsRun)
        {
            if (JoyconManager.Instance.GetIsEntry(playerId))
            {
                Joycon jc = JoyconManager.Instance.GetDevice(playerId);
                jc.SetRumble(m_freqLow, m_freqHigh, m_currentInfo[playerId].m_power, 0);
            }
        }

        // Pad
        if (PadManager.Instance.GetIsEntry(playerId))
        {
            Gamepad pad = PadManager.Instance.GetDevice(playerId);
            pad.SetMotorSpeeds(m_currentInfo[playerId].m_power, m_currentInfo[playerId].m_power);
        }
    }

    /// <summary>
    /// �o�C�u���[�V�����쓮(�S�v���C���[)
    /// </summary>
    /// <param name="time"> �U������(�t���[��) </param>
    /// <param name="power"> �U���̋��� </param>
    public void SetVibrationAll(int time, float power)
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            VibrationInfo info = new VibrationInfo();
            info.m_power = power;
            info.m_time = time;

            RegistInfo(info, i);

            SetVibration(i);
        }
    }

    /// <summary>
    /// �o�C�u���[�V������~
    /// </summary>
    /// <param name="playerId"> �ǂ̃v���C���[�ɑΉ������f�o�C�X��U�����~���邩 </param>
    public void StopVibration(int playerId)
    {
        ResetInfo(playerId);

        // Joycon
        if (JoyconConnector.Instance.m_IsRun)
        {
            if (JoyconManager.Instance.GetIsEntry(playerId))
            {
                Joycon jc = JoyconManager.Instance.GetDevice(playerId);
                jc.SetRumble(m_freqLow, m_freqHigh, 0.0f, 0);
            }
        }

        // Pad
        if (PadManager.Instance.GetIsEntry(playerId))
        {
            Gamepad pad = PadManager.Instance.GetDevice(playerId);
            pad.SetMotorSpeeds(0.0f, 0.0f);
        }
    }

    /// <summary>
    /// �o�C�u���[�V������~(�S�v���C���[)
    /// </summary>
    public void StopVibrationAll()
    {
        for (int i = 0; i < DeviceInfo.m_DeviceNum; i++)
        {
            StopVibration(i);
        }
    }

    /// <summary>
    /// �U�������L�^
    /// </summary>
    /// <param name="info"> �L�^����U����� </param>
    /// <param name="playerId"> �Ή�����v���C���[�ԍ� </param>
    private void RegistInfo(VibrationInfo info, int playerId)
    {
        int i = 0;

        if (m_currentInfo[playerId].m_power == 0.0f)
        {
            m_currentInfo[playerId] = info;
        }
        else if (info.m_power > m_currentInfo[playerId].m_power)
        {// �S�Ă̐U�����̒��ň�ԋ����ꍇ�����ɍ쓮������
            m_waitInfoList[playerId].m_info.Insert(0, m_currentInfo[playerId]);
            m_currentInfo[playerId] = info;
        }
        else if (info.m_time > m_currentInfo[playerId].m_time)
        {// ���݂̐U�����U�����Ԃ������ꍇ
            // ���̐U��������U�ۑ����邩���ׂ�
            for (; i < m_waitInfoList[playerId].m_info.Count; i++)
            {
                if (info.m_power > m_waitInfoList[playerId].m_info[i].m_power)
                {// ���傫�������̐U���̏ꍇ
                    m_waitInfoList[playerId].m_info.Insert(i, info);
                    i++;
                    break;
                }
                else if (info.m_time <= m_waitInfoList[playerId].m_info[i].m_time)
                {// ��蒷���U�����Ԃł͂Ȃ��ꍇ�͋L�^�̕K�v�Ȃ�
                    return;
                }
            }

            if (i == m_waitInfoList[playerId].m_info.Count)
            {
                // ��ԐU�����ア���U�����Ԃ��Œ��̏ꍇ�Ȃ̂ŋL�^����
                m_waitInfoList[playerId].m_info.Add(info);
                i++;
            }
        }

        // �ۑ��������ɂ���ĕK�v�Ȃ��Ȃ�U�������폜����
        for (; i < m_waitInfoList[playerId].m_info.Count; i++)
        {
            // �L�^���ꂽ�U�����Ԃ��Z���ꍇ�͐U�����邱�Ƃ͖����̂ō폜����
            if (info.m_time >= m_waitInfoList[playerId].m_info[i].m_time)
            {
                m_waitInfoList[playerId].m_info.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// �L�^���폜
    /// </summary>
    /// <param name="playerId"> �Ή�����v���C���[�ԍ� </param>
    private void ResetInfo(int playerId)
    {
        m_currentInfo[playerId] = new VibrationInfo();
        m_waitInfoList[playerId].m_info.Clear();
    }

    private void OnApplicationQuit()
    {
        StopVibrationAll();
    }
}
