using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone_DangerRun : DeadZone
{
    public enum SinkDir
    { 
        Top = 0,
        Bottom,
        Left,
        Right,
    }


    [SerializeField, CustomLabel("沈む方向")]
    private SinkDir m_sinkDir = SinkDir.Bottom;

    private static readonly Vector3[] m_sinkDirVec = new Vector3[4]
    {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right,
    };

    private Animator[] m_animators = new Animator[4];


    private void Start()
    {
        for (int i = 0; i < m_animators.Length; i++)
        {
            m_animators[i] = transform.GetChild(0).GetChild(i).GetComponent<Animator>();
            m_animators[i].SetInteger("DirSwitch", (int)m_sinkDir);
        }
    }

    /// <summary> プレイヤーが触れた際のイベント </summary>
    /// <param name="collision"> プレイヤーのコリジョン </param>
    protected override void OnTriggerEnter_Player(Collider2D collision)
    {
        OnTriggerExit_Player(collision);
    }

    protected override void OnTriggerExit_Player(Collider2D collision)
    {
        Transform trans = collision.transform.root;

        trans.GetComponent<PlayerDeathCount>().AddCount();

        // 沈みアニメーション用の親オブジェクトを探す
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            if (transform.GetChild(0).GetChild(i).childCount == 0)
            {
                if (RespawnManager.CheckInstance())
                {
                    EffectContainer.Instance.EffectPlay(
                        "デンジャラン_薬品に沈んだ瞬間",
                        trans.position,
                        Quaternion.identity * Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.down, m_sinkDirVec[(int)m_sinkDir]), Vector3.forward),
                        transform.GetChild(1));
                    EffectContainer.Instance.EffectPlay(
                        "デンジャラン_薬品に沈んでいるときの煙",
                        trans.position,
                        Quaternion.identity * Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.down, m_sinkDirVec[(int)m_sinkDir]), Vector3.forward),
                        transform.GetChild(1));
                    AudioManager.Instance.PlaySe("デンジャラン_沈む音", false);
                    // コントローラー振動
                    VibrationManager.Instance.SetVibration(trans.GetComponent<PlayerId>().m_Id, 45, 0.8f);

                    // 死亡処理
                    trans.GetComponent<PlayerController>().Death(RespawnManager.Instance.m_RevivalInterval);
                    // 一旦プレイヤーの親に設定
                    trans.SetParent(transform.GetChild(0).GetChild(i));
                    // 沈みアニメーションを開始
                    m_animators[i].SetTrigger("Sink");
                }

                break;
            }
        }
    }
}
