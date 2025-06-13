using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

public class BombManager : SingletonMonoBehaviour<BombManager>
{
    [SerializeField, CustomLabel("�X�|�[���̑�C")]
    GameObject[] m_SpawObject;

    //int m_spawObjectNumber;


    [SerializeField]
    BombGame_BombSpawMap[] m_SpawMap;

    // �g�p����X�N���^�u���I�u�W�F�N�g�̔z��
    private int m_CurrentSpawMapIndex = 0;

    public void SetCurrentSpawMapIndex(int _index) { m_CurrentSpawMapIndex = _index; }

    public float BombMultSpeed { get; set; }

    private int m_bombReferenceUsedNumber;  // �X�|�[�����锚�e�̔ԍ�


    class FixBombInfo
    {
        public int m_num;           // ���e�́Z��ڂ̃X�|�[���ł�����o��
        public int m_bombInfo;      // m_bombReferenceList�ɓo�^����Ă��遛���̔��e���Ăяo��
    }
    List<FixBombInfo> m_fixBombList = new List<FixBombInfo>();



    List<BombCharacter> m_bombCharacters = new List<BombCharacter>();


    float m_randomSum;

    int m_spawCount = 0;

    public bool m_spawFlag { get; set; }

    public List<BombCharacter> GetNowExistBombCharacters() { return m_bombCharacters; }


    protected override void Awake()
    {
        dontDestroyOnLoad = false;

        base.Awake();
    }

    void Start()
    {
        // �����_���̍ő�l�����Z
        for (int i = 0; i < m_SpawMap[m_CurrentSpawMapIndex].m_SpawBombMap.Count; i++)
        {
            for(int j = 0; j < m_SpawMap[m_CurrentSpawMapIndex].m_SpawBombMap[i].m_BombType.Length; j++)
            {
                m_randomSum += m_SpawMap[m_CurrentSpawMapIndex].m_SpawBombMap[i].m_BombFrequency;
            }
        }

        
        // �m��o���p�x�ݒ�
        // ���x���f�U�C�����ŁZ��ڂŁ����̔��e�̎�ނ��X�|�[��������A�Ƃ�
        AddFixBomb(0, 0);

        m_bombReferenceUsedNumber = NextBombReferenceUsedNumber();


        //// UniRx�Ō��m����I�Ȃ��Ƃɂ���
        //if (ReadyGoAnimationCallback.Instance.m_IsFinish)
        //{
        //    BombSpaw();

        //    // �T�E���h�x����
        //    AudioManager.Instance.PlaySe("�Q�[�g�J�x����", false);

        //    // �X�|�[�����J�E���g����
        //    m_spawCount++;

        //}

        m_spawFlag = true;

        BombMultSpeed = 1;
    }


    private void Update()
    {
        // ���̔��e���X�|�[�������Ă��ȉ��̃t���O
        if (!ReadyGoAnimationCallback.Instance.m_IsFinish)
        {
            return;
        }

        // �c��0.1�b������������X�|�[�������Ȃ�
        if (Timer.Instance.m_TimeCounter <= 0.1f)
        {
            return;
        }


        // �X�|�[���\���ǂ���
        bool ok = true;
        for (int i = 0; i < m_SpawObject.Length; i++)
        {
            if (m_SpawObject[i].GetComponent<CannonCharacter>().CanShot() == false)
            {
                ok = false;
                break;
            }
        }

        // �X�|�[������ꍇ
        if (m_spawFlag && ok)
        {
            m_bombCharacters.Clear();

            // ��������
            // �X�|�[�����锚�e�̔ԍ����Z�b�g���ꂽ�ԍ�
            m_bombReferenceUsedNumber = NextBombReferenceUsedNumber();

            BombSpaw();
            //StartCoroutine(SpawShot());

            // �T�E���h�x����
            AudioManager.Instance.PlaySe("�Q�[�g�J�x����", false);

            // �X�|�[�����J�E���g����
            m_spawCount++;

            m_spawFlag = false;

        }
    }

    private void BombSpaw()
    {
        int[] bombSpawAreaCount = { 1, 1, 1, 1 };

        // ���e�̎�ޕ��J��Ԃ�
        for (int bombSpawTypeIndex = 0; bombSpawTypeIndex < m_SpawMap[m_CurrentSpawMapIndex].m_SpawBombMap[m_bombReferenceUsedNumber].m_BombType.Length; bombSpawTypeIndex++)
        {
            // ���e�̌����J��Ԃ�
            for (int bombType = 0; bombType < m_SpawMap[m_CurrentSpawMapIndex].m_SpawBombMap[m_bombReferenceUsedNumber].m_BombType[bombSpawTypeIndex].m_Volume; bombType++)
            {
                int inAreaNum = BombGame_PlayAreaData.GetMaxAreaNumber();

                // �X�|�[���ꏊ
                // ���e�l�����_��
                inAreaNum = RandSpawNum(bombSpawAreaCount);
                for (int i = 0; i < bombSpawAreaCount.Length; i++)
                {
                    if (i != inAreaNum)
                    {
                        bombSpawAreaCount[i]+= 10;
                    }
                }

                // ���e���X�|�[��
                CannonCharacter cannonCharacter = m_SpawObject[inAreaNum].GetComponent<CannonCharacter>();
                cannonCharacter.Shot(m_SpawMap[m_CurrentSpawMapIndex].m_SpawBombMap[m_bombReferenceUsedNumber].m_BombType[bombSpawTypeIndex].m_BombPrefab);
            }
        }

    }


    int RandSpawNum(int[] _bombSpawAreaCount)
    {
        int randSum = 0;
        for (int i = 0; i < _bombSpawAreaCount.Length; i++)
        {
            randSum += _bombSpawAreaCount[i];
        }

        int result = 0;
        int rand = Random.Range(0, randSum);

        for (int i = 0; i < _bombSpawAreaCount.Length; i++)
        {
            if (_bombSpawAreaCount[i] >= rand)
            {
                result = i; 
                
                break;
            }
            rand -= _bombSpawAreaCount[i];
        }

        return result;

    }


    private void AddFixBomb(int _spawNum, int _bombInfoNum)
    {
        FixBombInfo bombInfo = new FixBombInfo();
        bombInfo.m_num = _spawNum;
        bombInfo.m_bombInfo = _bombInfoNum;
        
        m_fixBombList.Add(bombInfo);
    }


    private int NextBombReferenceUsedNumber()
    {
        // ���̔��e�̃^�C�v���m�肵�Ă��邩
        for (int i = 0; i < m_fixBombList.Count; i++)
        {
            // �ݒ肳�ꂽ���e�̔ԍ���������
            if (m_spawCount == m_fixBombList[i].m_num)
            {
                return m_fixBombList[i].m_bombInfo;

            }
        }

        return SpawRandom();
    }



    private int SpawRandom()
    {
        float rand = Random.Range(0.0f, m_randomSum);


        for (int i = 0; i < m_SpawMap[m_CurrentSpawMapIndex].m_SpawBombMap.Count; i++)
        {
            if (m_SpawMap[m_CurrentSpawMapIndex].m_SpawBombMap[i].m_BombFrequency >= rand)
            {
                //Debug.Log("rand" + rand);
                return i;
            }
            rand -= m_SpawMap[m_CurrentSpawMapIndex].m_SpawBombMap[i].m_BombFrequency;
        }
        //Debug.Log("SpawRandom" + i);
        return m_SpawMap[m_CurrentSpawMapIndex].m_SpawBombMap.Count - 1;
    }

    public void RemoveBombList(BombCharacter _bombCharacter)
    {
        m_bombCharacters.Remove(_bombCharacter);

        // ���e���X�g�Ɉ�����e���Ȃ������炪���݂��Ă��Ȃ�������
        if (m_bombCharacters.Count <= 0)
        {
            m_spawFlag = true;
        }
    }





}
