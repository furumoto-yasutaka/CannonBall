/*******************************************************************************
*
*	�^�C�g���F	�v���C���[�̑��Ƒ��`�[���̋��E�ǂƂ̓����蔻��𖳎�������X�N���v�g
*	�t�@�C���F	PlayerLegSuccerTeamAreaWallIgnore.cs
*	�쐬�ҁF	�Ö{ �ח�
*	������F    2023/10/05
*
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLegSuccerTeamAreaWallIgnore : MonoBehaviour
{
    private List<Collider2D> m_anotherTeamAreaWallList = new List<Collider2D>();


    private void OnEnable()
    {
        foreach (Collider2D wallCol in m_anotherTeamAreaWallList)
        {
            Collider2D legCol = GetComponent<Collider2D>();

            Physics2D.IgnoreCollision(legCol, wallCol);
        }
    }

    /// <summary> �ǂ̏����� </summary>
    /// <param name="wallParent"> �ǂ̐e�I�u�W�F�N�g </param>
    /// <param name="teamColor"> �ǂ̃`�[���� </param>
    public void InitWallList(Transform wallParent, SuccerTeamInfoSetter.TeamColor teamColor)
    {
        for (int i = 0; i < wallParent.childCount; i++)
        {
            if (i == (int)teamColor) { continue; }

            m_anotherTeamAreaWallList.Add(wallParent.GetChild(i).GetComponent<Collider2D>());
        }
    }
}
