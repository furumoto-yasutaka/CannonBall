using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGame_InBombReaction : SingletonMonoBehaviour<BombGame_InBombReaction>
{
    [SerializeField, CustomLabel("デフォルトのマテリアル")]
    Material m_defaultMaterial;

    [SerializeField, CustomLabel("エリア内に存在している時のマテリアル")]
    Material m_inMaterial;

    [SerializeField, CustomLabel("ダメージを受けたときのマテリアル")]
    Material m_inExplosionMaterial;

    [SerializeField, CustomLabel("死亡時のマテリアル")]
    Material m_deathMaterial;

    BombGame_PlayAreaInfo[] m_playAreaInfo = new BombGame_PlayAreaInfo[BombGame_PlayAreaData.GetMaxAreaNumber()];


    public Material m_DefaultMaterial { get { return m_defaultMaterial; } }

    public Material m_InMaterial { get { return m_inMaterial; } }

    public Material m_InExplosionMaterial { get { return m_inExplosionMaterial; } }

    public Material m_DeathMaterial { get { return m_deathMaterial; } }


    protected override void Awake()
    {
        // シーンを跨いで存在する理由ない
        dontDestroyOnLoad = false;

        // 継承元の呼び出し
        base.Awake();
    }

    private void Start()
    {
        for (int i = 0; i < m_playAreaInfo.Length; i++)
        {
            m_playAreaInfo[i] = BombGame_PlayAreaData.Instance.GetStageObject()[i].GetComponent<BombGame_PlayAreaInfo>();
        }
    }

    private void Update()
    {
        List<int> inAreaNumbers = BombGame_PlayAreaData.Instance.GetInAreaNumbers();

        // メッシュの色を変更する
        // エリア全て変更する
        for (int i = 0; i < BombGame_PlayAreaData.GetMaxAreaNumber(); i++)
        {
            bool b;
            switch (m_playAreaInfo[i].m_State)
            {
                case BombGame_PlayAreaInfo.State.None:
                    // 爆弾がエリアにあるかチェックする
                    if (inAreaNumbers.Exists(x => x == i))
                    {
                        m_playAreaInfo[i].ChangeState(BombGame_PlayAreaInfo.State.InBomb);
                    }
                    break;
                case BombGame_PlayAreaInfo.State.InBomb:
                    // 爆弾がエリアにあるかチェックする
                    if (!inAreaNumbers.Exists(x => x == i))
                    {
                        m_playAreaInfo[i].ChangeState(BombGame_PlayAreaInfo.State.None);
                    }
                    break;
                case BombGame_PlayAreaInfo.State.InExplosion:
                    // タイマーの更新をする
                    m_playAreaInfo[i].UpdateInExplosionTimer();
                    break;
                case BombGame_PlayAreaInfo.State.Death:
                    break;
            }
        }
    }
}
