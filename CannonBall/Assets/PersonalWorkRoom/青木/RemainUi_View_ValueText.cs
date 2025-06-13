using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainUi_View_ValueText : MonoBehaviour
{
    /// <summary> ���l�摜 </summary>
    [SerializeField]
    private Sprite[] m_numberSprite;

    /// <summary>
    /// �e�L�X�g��ݒ�
    /// </summary>
    /// <param name="value"> �c�@�� </param>
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
    /// �摜1����ݒ�
    /// </summary>
    /// <param name="childIndex"> �����ڂ�(���Ԗڂ̎q�I�u�W�F�N�g��) </param>
    /// <param name="sprite"> �ݒ肵�����X�v���C�g </param>
    private void SetSprite(int childIndex, Sprite sprite)
    {
        SpriteRenderer image = transform.GetChild(childIndex).GetComponent<SpriteRenderer>();
        image.sprite = sprite;

        // �X�v���C�g��null��������\�����Ȃ��悤�ɂ���
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
