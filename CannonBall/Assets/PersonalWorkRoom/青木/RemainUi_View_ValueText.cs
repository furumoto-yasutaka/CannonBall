using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainUi_View_ValueText : MonoBehaviour
{
    /// <summary> 数値画像 </summary>
    [SerializeField]
    private Sprite[] m_numberSprite;

    /// <summary>
    /// テキストを設定
    /// </summary>
    /// <param name="value"> 残機数 </param>
    public void SetText(int value)
    {
        int i = 0;

        do
        {
            int d = value % 10;
            SetSprite(i, m_numberSprite[d]);
            value /= 10;
            i++;
        }
        while (i < transform.childCount && value != 0);

        for (; i < transform.childCount; i++)
        {
            SetSprite(i, m_numberSprite[0]);
        }
    }

    /// <summary>
    /// 画像1枚を設定
    /// </summary>
    /// <param name="childIndex"> 何桁目か(何番目の子オブジェクトか) </param>
    /// <param name="sprite"> 設定したいスプライト </param>
    private void SetSprite(int childIndex, Sprite sprite)
    {
        SpriteRenderer image = transform.GetChild(childIndex).GetComponent<SpriteRenderer>();
        image.sprite = sprite;

        // スプライトがnullだったら表示しないようにする
        if (sprite == null)
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
        }
    }
}
