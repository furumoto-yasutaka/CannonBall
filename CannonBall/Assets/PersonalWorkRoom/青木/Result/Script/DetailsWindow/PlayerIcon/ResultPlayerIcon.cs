using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPlayerIcon : MonoBehaviour
{
    [SerializeField]
    Sprite[] m_sprites;

    [SerializeField]
    Sprite[] m_pienSprites;

    Image[] m_images;

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
            if (i == 0)     // èáà Ç™àÍà ÇÃèÍçá
            {
                SetSprite(rank, ref imageIndex, m_sprites);
            }
            else
            {
                SetSprite(rank, ref imageIndex, m_pienSprites);
            }
        }
    }

    private void SetSprite(TemporaryData.Rank[] _rank, ref int _imageIndex, Sprite[] _sprite)
    {
        for (int j = 0; j < _rank.Length; j++)
        {
            m_images[_imageIndex].sprite = _sprite[_rank[j].m_PlayerId];

            _imageIndex++;
        }
    }

}
