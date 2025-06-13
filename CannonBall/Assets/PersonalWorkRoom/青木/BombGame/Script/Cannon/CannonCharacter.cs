using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCharacter : MonoBehaviour
{

    [SerializeField]
    Animator m_animCannon;

    [SerializeField]
    Animator m_animGate;

    [SerializeField]
    Transform m_spawPoint;

    [SerializeField]
    Transform m_target;


    int m_spawCount = 0;


    private class SpawInfo
    {
        public GameObject m_BombObject;

    }

    List<GameObject> m_spawInfoStock = new List<GameObject>();
    int m_spawInfoStockIndex;



    /// <summary>
    /// BombManager����Ă΂��֐�
    /// </summary>
    public void Shot(GameObject _instantiate)
    {
        m_spawCount++;
        m_spawInfoStock.Add(_instantiate);
        m_animGate.SetTrigger("Open");
    }


    // �A�j���[�V�������Ŏg��
    // ���e����o����
    private void BombShot()
    {
        m_spawCount--;

        //Debug.Log("m_spawCount" + m_spawCount);

        if (m_spawInfoStock[m_spawInfoStockIndex] == null)
        {
            return;
        }
        GameObject obj = Instantiate(m_spawInfoStock[m_spawInfoStockIndex], m_spawPoint.position, Quaternion.identity, null);

        BombCharacter character = obj.GetComponent<BombCharacter>();

        BombManager.Instance.GetNowExistBombCharacters().Add(character);

        Vector3 vec = m_target.position - transform.position;
        character.BombStart(vec.normalized);

        //m_spawInfoStockIndex++;

        AudioManager.Instance.PlaySe("���e����", false);

        m_animCannon.SetInteger("ShotVolume", m_spawCount);
    }



    // �A�j���[�V�������Ŏg��
    private void BombShotSequenceFinish()
    {
        m_spawInfoStockIndex = 0;

        m_spawInfoStock.Clear();

        m_animGate.SetTrigger("Close");
    }


    // CannonGate�Ŏg��
    public void AnimationStart()
    {
        m_animCannon.SetTrigger("Forward");
    }


    public bool CanShot()
    {
        return (m_animGate.GetCurrentAnimatorStateInfo(0).IsName("Sleep") && m_animCannon.GetCurrentAnimatorStateInfo(0).IsName("Sleep")) ? true : false;
    }

}
