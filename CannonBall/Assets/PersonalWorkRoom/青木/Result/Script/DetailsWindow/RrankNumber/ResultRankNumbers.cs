using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultRankNumbers : MonoBehaviour
{
    [SerializeField]
    Sprite[] m_sprites;

    Image[]  m_images;

    private void OnEnable()
    {
        m_images = new Image[transform.childCount];

        for (int i = 0; i < m_images.Length; i++)
        {
            m_images[i] = transform.GetChild(i).GetComponent<Image>();
        }

        int imageIndex = 0;
        int[] len = TemporaryData.GetRankCount();   // èáà âΩéÌóﬁÅH
        for (int i = 0; i < len.Length; i++)
        {
            TemporaryData.Rank[] rank = TemporaryData.GetRank(i + 1);
            for (int j = 0; j < rank.Length; j++)
            {
                m_images[imageIndex].sprite = m_sprites[i];
                imageIndex++;
            }
        }
    }
}
