/*******************************************************************************
*
*	タイトル：	プレイヤーが移動できる空間の管理に使うデータ置き場
*	ファイル：	BombGame_PlayAreaData.cs
*	作成者：	青木 大夢
*	制作日：    2023/10/05
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGame_PlayAreaData : SingletonMonoBehaviour<BombGame_PlayAreaData>
{

    #region フィールド

    [SerializeField, CustomLabel("ステージのHPスクリプトを12時の方角から反時計回り")]
    GameObject[] m_stageObject;



    // プレイヤーが移動できるエリアの数
    static readonly int m_AREA_NUM = 4;

    // 爆弾が存在しているエリアの番号を格納される
    List<int> m_inAreaNumbers = new List<int>();

    #endregion


    #region プロパティ

    /// <summary> プレイエリアの数　４で固定 </summary>
    /// <returns></returns>
    public static int GetMaxAreaNumber() { return m_AREA_NUM; }

    /// <summary> ステージのオブジェクト　このオブジェクトにスクリプトがある </summary>
    /// <returns></returns>
    public GameObject[] GetStageObject() { return m_stageObject; }

    /// <summary> 爆弾が存在しているエリアの番号の先頭を返す </summary>
    /// <returns></returns>
    public int GetSpawAreaNumber() { return m_inAreaNumbers[0]; }

    /// <summary> 爆弾が入っている番号のリスト </summary>
    /// <returns></returns>
    public List<int> GetInAreaNumbers() { return m_inAreaNumbers; }


    #endregion


    protected override void Awake()
    {
        // シーンを跨いで存在する理由ない
        dontDestroyOnLoad = false;

        // 継承元の呼び出し
        base.Awake();

        //// 初期位置の番号を入力
        //m_inAreaNumbers.Add(0);
    }

    private void Update()
    {

        // 前フレームのデータをリフレッシュ
        m_inAreaNumbers.Clear();


        // プレイエリアの角度の大きさを計算
        float areaDistanceAngle = 360.0f / m_AREA_NUM;

        List<BombCharacter> bombCharacters = BombManager.Instance.GetNowExistBombCharacters();

        // 爆弾一つ一つの現在の居場所を探す
        for (int i = 0; i < bombCharacters.Count; i++)
        {
            Vector2 bombPos = bombCharacters[i].transform.position;

            // 時計回りに角度を出す
            float angle = Vector3.SignedAngle(Vector3.up, bombPos, Vector3.forward);

            // 角度を０～360度の範囲にに正規化する
            angle = Mathf.Repeat(angle, 360.0f);


            // 爆弾の現在の居場所を探す
            for (int j = 0; j < m_AREA_NUM; j++)
            {
                if (angle <= areaDistanceAngle * (j + 1))
                {
                    m_inAreaNumbers.Add(j);
                    bombCharacters[i].m_InAreaNumber = j;

                    break;
                }
            }

        }
    }

}
