using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    enum STATE
    {
		MOVE,
		STOP,
    }



	[SerializeField]
	float m_speed = 1.0f;

	[SerializeField]
	Transform m_endPoint;

	STATE m_state = STATE.MOVE;

	Vector2 m_moveDirection ;

	private Rigidbody2D m_rb;

	private Vector2[] m_position;
	private Vector2 m_targetPosition;


	private int m_positionIndex = 1;

	void Start()
	{
		m_rb = GetComponent<Rigidbody2D>();

		m_position = new Vector2[2];
		m_position[0] = transform.position;
		m_position[1] = m_endPoint.position;

		m_targetPosition = m_position[m_positionIndex];


		m_moveDirection = new Vector2(m_targetPosition.x - transform.position.x, m_targetPosition.y - transform.position.y);
		m_moveDirection = m_moveDirection.normalized;
	}

	void Update()
	{
        switch (m_state)
		{
            case STATE.MOVE:

				Move();

				break;
            case STATE.STOP:

				m_state = STATE.MOVE;

				break;
            default:
                break;
        }
        //m_rigid.MovePosition(new Vector3(m_targetPosition.x + +Mathf.PingPong(Time.time, m_endPoint.position.x), m_targetPosition.y + Mathf.PingPong(Time.time, m_endPoint.position.y), m_defaultPos.z));
	}



	private void Move()
	{
		m_rb.velocity += m_speed * m_moveDirection;

		// 内積で通り過ぎたか計算
		Vector2 pos = new Vector2(m_targetPosition.x - transform.position.x, m_targetPosition.y - transform.position.y);
		//Debug.Log("ターゲット" + m_targetPosition);
		//Debug.Log("座標" + transform.position);

		//Debug.Log("座標ベクトル" + pos);
		//Debug.Log("方角" + m_moveDirection);
		//Debug.Log("内積" + m_moveDirection.x * pos.x + m_moveDirection.y * pos.y);

		//Debug.Log("------------------------------------------");

		if (m_moveDirection.x * pos.x + m_moveDirection.y * pos.y < 0)
        {
			m_state = STATE.STOP;

			transform.position = m_targetPosition;

			m_positionIndex++;
			m_targetPosition = m_position[m_positionIndex % m_position.Length];

			m_moveDirection = new Vector2(m_targetPosition.x - transform.position.x, m_targetPosition.y - transform.position.y);
			m_moveDirection = m_moveDirection.normalized;
		}
	}


}
