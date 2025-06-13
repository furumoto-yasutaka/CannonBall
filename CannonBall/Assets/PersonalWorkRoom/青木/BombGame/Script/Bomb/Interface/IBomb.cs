using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBomb 
{
    #region プロパティ

    /// <summary>
    /// 爆弾が後どのくらいで爆発するのか
    /// </summary>
    /// <returns></returns>
    float GetAliveTime();


    /// <summary>
    /// 爆発でエリアが壊れる威力
    /// </summary>
    /// <returns></returns>
    float GetBombDamage();

    /// <summary>
    /// 爆弾が爆発しているのかどうか
    /// </summary>
    /// <returns></returns>
    bool GetisExprosition();

    #endregion


    void StartImpact(Vector3 _target);

}
