
// temporary�f�[�^�Ƀf�[�^������ׂ̒��Ԓn�_
// �����ʂ��āA�f�[�^�����Ă���
// �X�N���v�g�ł����G�邱�Ƃ͂Ȃ�

using UnityEngine;
using UnityEngine.SceneManagement;


public class TemporaryDataLine : SingletonMonoBehaviour<TemporaryDataLine>
{
    PlayerId[] m_playerIds;
    ITemporaryDataGetter[] m_tempGetters;

    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();
    }
    private void Start()
    {
        // �f�[�^�̎擾
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        m_playerIds = new PlayerId[playerObjects.Length];
        m_tempGetters = new ITemporaryDataGetter[playerObjects.Length];

        for (int i = 0; i < playerObjects.Length; i++)
        {
            m_tempGetters[i] = playerObjects[i].GetComponent<ITemporaryDataGetter>();
            m_playerIds[i] = playerObjects[i].GetComponent<PlayerId>();
        }


        //SceneManager.sceneUnloaded += OnSceneLoaded;
    }

    //private void OnSceneLoaded(Scene _scene)
    //{
    //    Debug.Log("�V�[���I��" + _scene.name);

    //    // �f�[�^�̎擾
    //    GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
    //    PlayerId[] playerIds = new PlayerId[playerObjects.Length];
    //    ITemporaryDataGetter[] tempGetters = new ITemporaryDataGetter[playerObjects.Length];

    //    for (int i = 0; i < playerObjects.Length; i++)
    //    {
    //        tempGetters[i] = playerObjects[i].GetComponent<ITemporaryDataGetter>();
    //        playerIds[i] = playerObjects[i].GetComponent<PlayerId>();
    //    }

    //    Debug.Log("�f�[�^�擾�܂Ł@�v���C���[�̐��@�F" + playerObjects.Length);


    //    // �V�[�����ƂɃf�[�^�𑗂�
    //    string name = _scene.name;
    //    if (name == SceneNameEnum.CannonFight.ToString())
    //    {
    //        TemporaryData.m_PreGameMode = SceneNameEnum.CannonFight;

    //        Common(playerIds, tempGetters);
    //    }
    //    else if (name == SceneNameEnum.BombRush.ToString())
    //    {
    //        TemporaryData.m_PreGameMode = SceneNameEnum.BombRush;

    //        Common(playerIds, tempGetters);
    //    }
    //    else if (name == SceneNameEnum.DangeRun.ToString())
    //    {
    //        TemporaryData.m_PreGameMode = SceneNameEnum.DangeRun;

    //        Common(playerIds, tempGetters);
    //    }
    //}


    public void SetResultData()
    {

        // �V�[�����ƂɃf�[�^�𑗂�
        string name = SceneManager.GetActiveScene().name;
        if (name == SceneNameEnum.CannonFight.ToString())
        {
            TemporaryData.m_PreGameMode = SceneNameEnum.CannonFight;

            Common(m_playerIds, m_tempGetters);
        }
        else if (name == SceneNameEnum.BombRush.ToString())
        {
            TemporaryData.m_PreGameMode = SceneNameEnum.BombRush;

            Common(m_playerIds, m_tempGetters);
        }
        else if (name == SceneNameEnum.DangerRun.ToString())
        {
            TemporaryData.m_PreGameMode = SceneNameEnum.DangerRun;

            Common(m_playerIds, m_tempGetters);
        }
    }

    private void Common(PlayerId[] _playerIds, ITemporaryDataGetter[] _tempGetters)
    {
        for (int i = 0; i < TemporaryData.m_Rank.Length; i++)
        {
            TemporaryData.m_Rank[i].m_PlayerId = _playerIds[i].m_Id;
            TemporaryData.m_Rank[i].m_Parameter = _tempGetters[i].GetRankingParameter();

            Debug.Log("m_PlayerId  �F" + TemporaryData.m_Rank[i].m_PlayerId + "TemporaryData.m_Rank[i].m_Parameter  �F" + TemporaryData.m_Rank[i].m_Parameter);
        }
    }
}
