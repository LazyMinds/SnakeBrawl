using UnityEngine;
using System.Collections;

public class SoloManager : MonoBehaviour {
	
	public GameObject snake = null;
	public GameObject food = null;

	public GameObject borderTop = null;
	public GameObject borderBottom = null;
	public GameObject borderLeft = null;
	public GameObject borderRight = null;

	private Vector2 m_direction = Vector2.right;


	// Use this for initialization
	void Start () {
		InvokeRepeating ("Move", 0f, 0.15f);
		InvokeRepeating ("Food", 3f, 3f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey ("up"))
		{
			m_direction = Vector2.up;
		}
		else if (Input.GetKey("down"))
		{
			m_direction = -Vector2.up;
		}
		else if (Input.GetKey ("left"))
		{
			m_direction = -Vector2.right;
		}
		else if (Input.GetKey("right"))
		{
			m_direction = Vector2.right;
		}
	}

	void Move()
	{
		Vector2 dir = m_direction;
		snake.transform.Translate (dir*32, Space.World);

		if (dir == Vector2.up)
			snake.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
		else if (dir == -Vector2.up)
			snake.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 180));
		else if (dir == Vector2.right)
			snake.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 270));
		else if (dir == -Vector2.right)
			snake.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 90));
	}

	void Food()
	{
		int x = Random.Range((int)borderLeft.transform.position.x+16, (int)borderRight.transform.position.x);
		int y = Random.Range((int)borderTop.transform.position.y-16, (int)borderBottom.transform.position.y);
		x -= x % 32;
		y -= y % 32;


		Instantiate(food, new Vector2(x, y), Quaternion.identity);
	}
}
