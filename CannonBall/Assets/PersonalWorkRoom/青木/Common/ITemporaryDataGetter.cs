//
// ここからデータを引っ張ってくる
// シーンが終わるタイミングで勝手に呼ばれる
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITemporaryDataGetter
{
    // ランキングを決めるためのパラメータ
    public float GetRankingParameter();
}
