using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCharacter : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_IBombObject;

    /// <summary> ���̔��e���X�|�[�������Ă������̃t���O </summary>
    bool m_isSpawNext = false;


    /// <summary>
    /// ���܁A���e������G���A
    /// </summary>
    public int m_InAreaNumber { get; set; }

    #region �v���p�e�B

    /// <summary> IBomb���p���������e����A���̔��e���X�|�[��������t���O�𗧂����� </summary>
    public void BootisSpawNext() { m_isSpawNext = true; }

    /// <summary> BombManager����A���̔��e���X�|�[������t���O���󂯎�邽�߂� </summary>
    public bool GetisSpawNext() { return m_isSpawNext; }

    #endregion

    private void Start()
    {

    }

    //private void Update()
    //{
    //    // �v���C�G���A�̊p�x�̑傫�����v�Z
    //    float areaDistanceAngle = 360.0f / BombGame_PlayAreaData.GetMaxAreaNumber();


    //    //���e�̏ꏊ�i�x�N�g���Ƃ��Ďg�������j
    //    Vector2 bombPos = BombManager.Instance.GetNowExistBombCharacters()[0].transform.position;

    //    // ���v���Ɋp�x���o��
    //    float angle = Vector3.SignedAngle(Vector3.up, bombPos, Vector3.forward);

    //    // �p�x���O�`360�x�͈̔͂ɂɐ��K������
    //    angle = Mathf.Repeat(angle, 360.0f);


    //    // ���e�̌��݂̋��ꏊ��T��
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
