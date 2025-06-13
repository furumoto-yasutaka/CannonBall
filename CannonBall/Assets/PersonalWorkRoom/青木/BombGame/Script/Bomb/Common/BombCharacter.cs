using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCharacter : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_IBombObject;

    /// <summary> 次の爆弾をスポーンさせていいかのフラグ </summary>
    bool m_isSpawNext = false;


    /// <summary>
    /// いま、爆弾がいるエリア
    /// </summary>
    public int m_InAreaNumber { get; set; }

    #region プロパティ

    /// <summary> IBombを継承した爆弾から、次の爆弾をスポーンさせるフラグを立たせる </summary>
    public void BootisSpawNext() { m_isSpawNext = true; }

    /// <summary> BombManagerから、次の爆弾をスポーンするフラグを受け取るための </summary>
    public bool GetisSpawNext() { return m_isSpawNext; }

    #endregion

    private void Start()
    {

    }

    //private void Update()
    //{
    //    // プレイエリアの角度の大きさを計算
    //    float areaDistanceAngle = 360.0f / BombGame_PlayAreaData.GetMaxAreaNumber();


    //    //爆弾の場所（ベクトルとして使うだけ）
    //    Vector2 bombPos = BombManager.Instance.GetNowExistBombCharacters()[0].transform.position;

    //    // 時計回りに角度を出す
    //    float angle = Vector3.SignedAngle(Vector3.up, bombPos, Vector3.forward);

    //    // 角度を０～360度の範囲にに正規化する
    //    angle = Mathf.Repeat(angle, 360.0f);


    //    // 爆弾の現在の居場所を探す
    //    for (int i = 0; i < BombGame_PlayAreaData.GetMaxAreaNumber(); i++)
    //    {
    //        if (angle <= areaDistanceAngle * (i + 1))
    //        {
    //            m_InAreaNumber = i;
    //            break;
    //        }
    //    }

    //    //Debug.Log("m_InAreaNumber" + m_InAreaNumber);
    //}



    public void BombStart(Vector3 _target)
    {
        GetComponent<IBomb>().StartImpact(_target);
        //StartCoroutine(DistanceSpaw(_rotation));
    }
}
