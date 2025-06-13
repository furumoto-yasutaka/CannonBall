using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayer : MonoBehaviour
{
    private static PlayerController.Type[] m_initType = new PlayerController.Type[]
    {
        PlayerController.Type.Balance,
        PlayerController.Type.Power,
        PlayerController.Type.Speed,
        PlayerController.Type.Power,
    };

    [SerializeField, CustomArrayLabel(typeof(PlayerController.Type))]
    private GameObject[] m_playerPrefab;

    [SerializeField, CustomLabel("�J�����^�[�Q�b�g�O���[�v����R���|�[�l���g")]
    private CinemachineTargetGroupRegister m_targetGroupResister;


    private void Awake()
    {
        // �v���C���[��������
        for (int i = 0; i < 4; i++)
        {
            GameObject obj = Instantiate(m_playerPrefab[(int)m_initType[i]], new Vector3(0.0f, -100.0f, 0.0f), Quaternion.identity, transform);
            obj.GetComponent<PlayerController_CannonFight>().InitCameraTargetGroup(m_targetGroupResister);
            RankProcedure.Instance.InitPlayer(obj.GetComponent<PlayerPoint_CannonFight>(), i);
        }

        // �s�v�Ȃ̂ł��̃R���|�[�l���g���폜
        Destroy(this);
    }

    public static void SetInitType(int id, PlayerController.Type type)
    {
        m_initType[id] = type;
    }
}
