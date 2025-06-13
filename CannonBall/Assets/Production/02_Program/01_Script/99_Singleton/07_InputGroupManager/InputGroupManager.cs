/*******************************************************************************
*
*	�^�C�g���F	���͎�t����V���O���g���X�N���v�g
*	�t�@�C���F	InputGroupManager.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/04/16
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;

#region support class

/// <summary> ���͎�t�O���[�v </summary>
[System.Serializable]
public class InputGroup
{
    /// <summary> �O���[�v�� </summary>
    [CustomLabel("�O���[�v��")]
    public string m_Name;
    /// <summary> ���͎�t���Ă��邩 </summary>
    [HideInInspector]
    public bool m_IsCanInput;
    /// <summary> �O���[�v�ɕR�Â��E�B���h�E </summary>
    [CustomLabel("�Ή�����E�B���h�E�X�N���v�g")]
    public Window m_Window;
    /// <summary> �E�B���h�E�Ŏg�p����C���v�b�g�A�N�V�����}�b�v�� </summary>
    [CustomLabel("�C���v�b�g�A�N�V������")]
    public NewInputActionMapName m_inputActionMapName;
    /// <summary> ����Ώۂ̃N���X </summary>
    [Header("���E�B���h�E�͂��̒��Ɋ܂߂Ȃ��Ă�OK\n" +
        "(�܂߂Ă��܂����ꍇ�ł��G���[�ɂ͂Ȃ�܂���)")]
    public InputElement[] m_Elemants;
}

/// <summary> ���b�N�����\��O���[�v��� </summary>
public class OrderInfo
{
    /// <summary> �Ώۂ̃O���[�v </summary>
    public InputGroup m_Group;
    /// <summary> �v�f�P�ʂŔ��f���邩 </summary>
    public bool m_IsRefChild;

    public OrderInfo(InputGroup group, bool isRefChild)
    {
        m_Group = group;
        m_IsRefChild = isRefChild;
    }
}

#endregion


public class InputGroupManager : SingletonMonoBehaviour<InputGroupManager>
{
    #region field

    [Header("��ԏ�̗v�f���ŏ��Ƀ��b�N������ԂɂȂ�܂�")]

    /// <summary> ���b�N����������Ă���O���[�v�� </summary>
    [SerializeField, CustomLabel("���b�N����������Ă���O���[�v��")]
    private string m_unlockGroupName = "";

    /// <summary> �C���v�b�g�A�N�V�����}�b�v </summary>
    [SerializeField]
    private List<NewInputActionMap> m_actionMapList = new List<NewInputActionMap>();

    /// <summary> �C���X�y�N�^�[������͉\�ɂ��邽�߂̈ꎞ�ϐ� </summary>
    [SerializeField]
    private InputGroup[] m_inputGroupListInspector;

    /// <summary> ���b�N��ԕύX�Ɏg����� </summary>
    private OrderInfo m_lockOrderInfo = null;

    /// <summary> ���b�N��ԕύX�Ɏg����� </summary>
    private OrderInfo m_unlockOrderInfo = null;

    /// <summary> �e�O���[�v�̏�� </summary>
    private Dictionary<string, InputGroup> m_inputGroupList = new Dictionary<string, InputGroup>();

    /// <summary> ���͂̃��b�N������Ԃ̃O���[�v�� </summary>
    private InputGroup m_UnlockGroup;

    #endregion

    #region function

    /// <summary>
    /// ���̃X�N���v�g�̏��������s�����߁A
    /// �ő��Ŏ��s�ł���悤�ɗD��x��-100�ɏオ���Ă��܂�
    /// </summary>
    protected override void Awake()
    {
        // �V�[���J�ڂō폜�����悤�ɂ���
        dontDestroyOnLoad = false;

        base.Awake();

        // InputActionMap������������
        foreach (NewInputActionMap map in m_actionMapList)
        {
            map.Init();
        }

        // ���X�g�̏�����
        foreach (InputGroup info in m_inputGroupListInspector)
        {
            m_inputGroupList.Add(info.m_Name, info);

            // �E�B���h�E�̏�����
            info.m_Window.InitializeParam(this, info);

            // ���͂����v�f�̏�����
            foreach (InputElement elem in info.m_Elemants)
            {
                elem.InitializeParam(this, info);
            }

            // �A�N�V�����}�b�v�̏�����
            info.m_Window.InitInputActionMap(m_actionMapList.FirstOrDefault(map => map.m_mapName == info.m_inputActionMapName.ToString()));
        }

        // �ŏ��̃O���[�v�̃��b�N����������
        if (m_inputGroupListInspector.Length > 0)
        {
            UnlockInputGroup(new OrderInfo(m_inputGroupListInspector[0], false));
        }
    }

    /// <summary>
    /// �ő��Ŏ��s�ł���悤�ɗD��x��-100�ɏオ���Ă��܂�
    /// </summary>
    private void Update()
    {
        // ���b�N�󋵂̕ω��𔽉f
        if (m_lockOrderInfo != null)
        {
            LockInputGroup(m_lockOrderInfo);
            m_lockOrderInfo = null;
        }
        if (m_unlockOrderInfo != null)
        {
            UnlockInputGroup(m_unlockOrderInfo);
            m_unlockOrderInfo = null;
        }

        // InputActionMap�ɓo�^����Ă���S�Ă�InputAction���X�V����
        foreach (NewInputActionMap map in m_actionMapList)
        {
            if (map.m_IsEnable)
            {
                map.Update();
            }
            else
            {
                map.Reset();
            }
        }
    }


    /// <summary>
    /// �O���[�v�P�ʂœ��͂����b�N
    /// </summary>
    /// <param name="info"> ���b�N����O���[�v�̏�� </param>
    private void LockInputGroup(OrderInfo info)
    {
        InputGroup group = info.m_Group;

        group.m_IsCanInput = false;
        group.m_Window.DisableInputActionMap();

        m_unlockGroupName = "";

        if (info.m_IsRefChild)
        {
            group.m_Window.LockInput();
            foreach (InputElement elem in group.m_Elemants)
            {
                elem.LockInput();
            }
        }
    }

    /// <summary>
    /// �O���[�v�P�ʂœ��͂̃��b�N����
    /// </summary>
    /// <param name="info"> ���b�N����O���[�v�̏�� </param>
    private void UnlockInputGroup(OrderInfo info)
    {
        InputGroup group = info.m_Group;

        group.m_IsCanInput = true;
        group.m_Window.EnableInputActionMap();

        m_UnlockGroup = info.m_Group;
        m_unlockGroupName = info.m_Group.m_Name;

        if (info.m_IsRefChild)
        {
            group.m_Window.UnlockInput();
            foreach (InputElement elem in group.m_Elemants)
            {
                elem.UnlockInput();
            }
        }
    }

    /// <summary>
    /// ���b�N��v������
    /// </summary>
    /// <param name="isRefChild"> �v�f�P�ʂŔ��f���邩 </param>
    public void LockInputGroupOrder(bool isRefChild)
    {
        if (m_UnlockGroup == null) { return; }

        // ��ɗv����������Γo�^����
        if (m_lockOrderInfo == null)
        {
            m_lockOrderInfo = new OrderInfo(m_UnlockGroup, isRefChild);
        }
    }

    /// <summary>
    /// ���b�N������v������
    /// </summary>
    /// <param name="group"> �ΏۃO���[�v </param>
    /// <param name="isRefChild"> �v�f�P�ʂŔ��f���邩 </param>
    public void UnlockInputGroupOrder(InputGroup group, bool isRefChild)
    {
        // ��ɗv����������Γo�^����
        if (m_unlockOrderInfo == null)
        {
            m_unlockOrderInfo = new OrderInfo(group, isRefChild);
        }
    }

    #endregion
}
