using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

public class BombManager : SingletonMonoBehaviour<BombManager>
{
    [SerializeField, CustomLabel("スポーンの大砲")]
    GameObject[] m_SpawObject;

    //int m_spawObjectNumber;


    [SerializeField]
    BombGame_BombSpawMap[] m_SpawMap;

    // 使用するスクリタブルオブジェクトの配列
    private int m_CurrentSpawMapIndex = 0;

    public void SetCurrentSpawMapIndex(int _index) { m_CurrentSpawMapIndex = _index; }

    public float BombMultSpeed { get; set; }

    private int m_bombReferenceUsedNumber;  // スポーンする爆弾の番号


    class FixBombInfo
    {
        public int m_num;           // 爆弾の〇回目のスポーンでこれを出す
        public int m_bombInfo;      // m_bombReferenceListに登録されている○○の爆弾を呼び出す
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
        // ランダムの最大値を合算
        for (int i = 0; i < m_SpawMap[m_CurrentSpawMapIndex].m_SpawBombMap.Count; i++)
        {
            for(int j = 0; j < m_SpawMap[m_CurrentSpawMapIndex].m_SpawBombMap[i].m_BombType.Length; j++)
            {
                m_randomSum += m_SpawMap[m_CurrentSpawMapIndex].m_SpawBombMap[i].m_BombFrequency;
            }
        }

        
        // 確定出現頻度設定
        // レベルデザイン等で〇回目で△△の爆弾の種類をスポーンさせる、とか
        AddFixBomb(0, 0);

        m_bombReferenceUsedNumber = NextBombReferenceUsedNumber();


        //// UniRxで検知する的なことにする
        //if (ReadyGoAnimationCallback.Instance.m_IsFinish)
        //{
        //    BombSpaw();

        //    // サウンド警告音
        //    AudioManager.Instance.PlaySe("ゲート開閉警告音", false);

        //    // スポーンをカウントする
        //    m_spawCount++;

        //}

        m_spawFlag = true;

        BombMultSpeed = 1;
    }


    private void Update()
    {
        // 次の爆弾をスポーンさせてい以下のフラグ
        if (!ReadyGoAnimationCallback.Instance.m_IsFinish)
        {
            return;
        }

        // 残り0.1秒だったらもうスポーンさせない
        if (Timer.Instance.m_TimeCounter <= 0.1f)
        {
            return;
        }


        // スポーン可能かどうか
        bool ok = true;
        for (int i = 0; i < m_SpawObject.Length; i++)
        {
            if (m_SpawObject[i].GetComponent<CannonCharacter>().CanShot() == false)
            {
                ok = false;
                break;
            }
        }

        // スポーンする場合
        if (m_spawFlag && ok)
        {
            m_bombCharacters.Clear();

            // 生成制限
            // スポーンする爆弾の番号がセットされた番号
            m_bombReferenceUsedNumber = NextBombReferenceUsedNumber();

            BombSpaw();
            //StartCoroutine(SpawShot());

            // サウンド警告音
            AudioManager.Instance.PlaySe("ゲート開閉警告音", false);

            // スポーンをカウントする
            m_spawCount++;

            m_spawFlag = false;

        }
    }

    private void BombSpaw()
    {
        int[] bombSpawAreaCount = { 1, 1, 1, 1 };

        // 爆弾の種類分繰り返す
        for (int bombSpawTypeIndex = 0; bombSpawTypeIndex < m_SpawMap[m_CurrentSpawMapIndex].m_SpawBombMap[m_bombReferenceUsedNumber].m_BombType.Length; bombSpawTypeIndex++)
        {
            // 爆弾の個数分繰り返す
            for (int bombType = 0; bombType < m_SpawMap[m_CurrentSpawMapIndex].m_SpawBombMap[m_bombReferenceUsedNumber].m_BombType[bombSpawTypeIndex].m_Volume; bombType++)
            {
                int inAreaNum = BombGame_PlayAreaData.GetMaxAreaNumber();

                // スポーン場所
                // 爆弾四個ランダム
                inAreaNum = RandSpawNum(bombSpawAreaCount);
                for (int i = 0; i < bombSpawAreaCount.Length; i++)
                {
                    if (i != inAreaNum)
                    {
                        bombSpawAreaCount[i]+= 10;
                    }
                }

                // 爆弾をスポーン
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
        // 次の爆弾のタイプが確定しているか
        for (int i = 0; i < m_fixBombList.Count; i++)
        {
            // 設定された爆弾の番号だったら
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

        // 爆弾リストに一つも爆弾がなかったらが存在していなかったら
        if (m_bombCharacters.Count <= 0)
        {
            m_spawFlag = true;
        }
    }





}
