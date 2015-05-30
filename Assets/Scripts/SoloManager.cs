using UnityEngine;
using System.Collections;

public class SoloManager : MonoBehaviour {
	
	public GameObject snake = null;
	public GameObject food = null;

	public GameObject borderTop = null;
	public GameObject borderBottom = null;
	public GameObject borderLeft = null;
	public GameObject borderRight = null;

	public Sprite snakeTop = null;
	public Sprite snakeBottom = null;
	public Sprite snakeLeft = null;
	public Sprite snakeRight = null;

	private Vector2 m_direction = Vector2.right;


	// Use this for initialization
	void Start () {
		InvokeRepeating ("Move", 0f, 0.3f);
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
		SpriteRenderer renderer = snake.GetComponent<SpriteRenderer>() as SpriteRenderer;
		snake.transform.Translate (dir*32);

		if (dir == Vector2.up)
			renderer.sprite = snakeTop;
		else if (dir == -Vector2.up)
			renderer.sprite = snakeBottom;
		else if (dir == Vector2.right)
			renderer.sprite = snakeRight;
		else if (dir == -Vector2.right)
			renderer.sprite = snakeLeft;
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
