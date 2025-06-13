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
        // 衝突したらマテリアルを変更する
        _tutorialWall.ChangeHitMaterial(m_hitWallMaterial);


        // 成功した物をカウント
        m_completeCount++;

        // 全員が壁に衝突した
        if (m_completeCount >= walls.Length)
        {
            // このチュートリアルが終了したら、次のチュートリアルの準備をする
            TutorialManager.Instance.ChangeNextLevel();
        }
    }
}
