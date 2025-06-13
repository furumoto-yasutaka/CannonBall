using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBomb 
{
    #region �v���p�e�B

    /// <summary>
    /// ���e����ǂ̂��炢�Ŕ�������̂�
    /// </summary>
    /// <returns></returns>
    float GetAliveTime();


    /// <summary>
    /// �����ŃG���A������З�
    /// </summary>
    /// <returns></returns>
    float GetBombDamage();

    /// <summary>
    /// ���e���������Ă���̂��ǂ���
    /// </summary>
    /// <returns></returns>
    bool GetisExprosition();

    #endregion


    void StartImpact(Vector3 _target);

}
