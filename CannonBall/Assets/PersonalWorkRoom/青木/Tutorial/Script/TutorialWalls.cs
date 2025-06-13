using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialWalls : MonoBehaviour
{
    [SerializeField]
    Material m_defaultWallMaterial;

    [SerializeField]
    Material m_hitWallMaterial;

    TutorialWall[] walls;

    int m_completeCount = 0;


    private void Start()
    {
        walls = new TutorialWall[transform.childCount];
        for (int i = 0; i < walls.Length; i++)
        {
            walls[i] = transform.GetChild(i).gameObject.GetComponent<TutorialWall>();
        }
    }


    public void WallHit(TutorialWall _tutorialWall)
    {
        // �Փ˂�����}�e���A����ύX����
        _tutorialWall.ChangeHitMaterial(m_hitWallMaterial);


        // �������������J�E���g
        m_completeCount++;

        // �S�����ǂɏՓ˂���
        if (m_completeCount >= walls.Length)
        {
            // ���̃`���[�g���A�����I��������A���̃`���[�g���A���̏���������
            TutorialManager.Instance.ChangeNextLevel();
        }
    }
}
