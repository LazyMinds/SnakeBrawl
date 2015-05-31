using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SnakeManager : MonoBehaviour {

	//Prefabs
	public GameObject snake = null;
	public GameObject snakeBody = null;
	public GameObject snakeTail = null;
	public GameObject food = null;
	public GameObject canvas = null;
	public Sprite spriteHorVerDirection = null;
	public Sprite spriteAngleDirection = null;


	//Border
	public Transform borderTop = null;
	public Transform borderBottom = null;
	public Transform borderLeft = null;
	public Transform borderRight = null;

	List<GameObject> snakeTailList = new List<GameObject>();
	private Vector2 m_direction = Vector2.up;
	private bool foodCollision = false;

	enum Direction {GAUCHE, HAUT, BAS, DROITE};

	void Start () {
		GameObject tail = Instantiate (snakeTail, snake.transform.position + new Vector3 (0, -32, 0), Quaternion.identity) as GameObject;
		snakeTailList.Add (tail);
		InvokeRepeating ("Move", 0f, 0.15f);
		Invoke("SpawnFood", 1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("up")) {
			if (m_direction != -Vector2.up)
				m_direction = Vector2.up;
		} else if (Input.GetKey ("down")) {
			if (m_direction != Vector2.up)
				m_direction = -Vector2.up;
		} else if (Input.GetKey ("left")) {
			if (m_direction != Vector2.right)
				m_direction = -Vector2.right;
		} else if (Input.GetKey ("right")) {
			if (m_direction != -Vector2.right)
				m_direction = Vector2.right;
		}
	}

	void Move() {

		// Deplacement du snake dans la direction en cour
		Vector2 oldHeadPosition = snake.transform.position;
		snake.transform.Translate (m_direction*32, Space.World);

		if (m_direction == Vector2.up)
			snake.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
		else if (m_direction == -Vector2.up)
			snake.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 180));
		else if (m_direction == Vector2.right)
			snake.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 270));
		else if (m_direction == -Vector2.right)
			snake.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 90));

		Quaternion r = snake.transform.rotation;
		
		if (foodCollision) {
			GameObject body =(GameObject)Instantiate(snakeBody, oldHeadPosition, Quaternion.identity);
			snakeTailList.Insert(0, body);
			foodCollision = false;
		}
		else if (snakeTailList.Count > 0) { // on delete une partie du corp, donc on met la queue a la place de la partie delete

			snakeTailList.Last ().transform.position = oldHeadPosition;
			snakeTailList.Insert (0, snakeTailList.Last ());
			snakeTailList.RemoveAt (snakeTailList.Count - 1); 
		}

		if (snakeTailList.Count > 0) {
			for (int i = 0; i<snakeTailList.Count; i++)
			{
				Vector2 beforePosition;
				if (i == 0)
				{
					beforePosition = snake.transform.position;
				}
				else
				{
					beforePosition = snakeTailList[i-1].transform.position;
				}
				GameObject current = snakeTailList[i];

				Direction before_dir = Direction.HAUT;
				if (beforePosition.x < current.transform.position.x && beforePosition.y == current.transform.position.y)
				{
					before_dir = Direction.GAUCHE;
				}
				else if (beforePosition.x > current.transform.position.x && beforePosition.y == current.transform.position.y)
				{
					before_dir = Direction.DROITE;
				}
				else if (beforePosition.x == current.transform.position.x && beforePosition.y > current.transform.position.y)
				{
					before_dir = Direction.HAUT;
				}
				else if (beforePosition.x == current.transform.position.x && beforePosition.y < current.transform.position.y)
				{
					before_dir = Direction.BAS;
				}

				if (snakeTailList.Count-1 == i)
				{
					if (before_dir == Direction.GAUCHE)
					{
						current.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 90));
					}
					if (before_dir == Direction.DROITE)
					{
						current.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 270));
					}
					if (before_dir == Direction.HAUT)
					{
						current.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
					}
					if (before_dir == Direction.BAS)
					{
						current.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 170));
					}
					current.GetComponent<SpriteRenderer>().sprite = snakeTail.GetComponent<SpriteRenderer>().sprite;
					return;
				}
				Transform after = snakeTailList[i+1].transform;


				Direction after_dir = Direction.HAUT;
				if (after.transform.position.x < current.transform.position.x && after.transform.position.y == current.transform.position.y)
				{
					after_dir = Direction.GAUCHE;
				}
				else if (after.transform.position.x > current.transform.position.x && after.transform.position.y == current.transform.position.y)
				{
					after_dir = Direction.DROITE;
				}
				else if (after.transform.position.x == current.transform.position.x && after.transform.position.y > current.transform.position.y)
				{
					after_dir = Direction.HAUT;
				}
				else if (after.transform.position.x == current.transform.position.x && after.transform.position.y < current.transform.position.y)
				{
					after_dir = Direction.BAS;
				}

				if ((before_dir == Direction.GAUCHE && after_dir == Direction.DROITE) || (before_dir == Direction.DROITE && after_dir == Direction.GAUCHE))
				{
					current.GetComponent<SpriteRenderer>().sprite = spriteHorVerDirection;
					current.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 90));
				}
				else if ((before_dir == Direction.HAUT && after_dir == Direction.BAS) || (before_dir == Direction.BAS && after_dir == Direction.HAUT))
				{
					current.GetComponent<SpriteRenderer>().sprite = spriteHorVerDirection;
					current.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
				}
				else if (before_dir == Direction.GAUCHE && after_dir == Direction.BAS)
				{
					current.GetComponent<SpriteRenderer>().sprite = spriteAngleDirection;
					current.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 180));
				}
				else if (before_dir == Direction.GAUCHE && after_dir == Direction.HAUT)
				{
					current.GetComponent<SpriteRenderer>().sprite = spriteAngleDirection;
					current.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 90));
				}
				else if (before_dir == Direction.BAS && after_dir == Direction.GAUCHE)
				{
					current.GetComponent<SpriteRenderer>().sprite = spriteAngleDirection;
					current.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 180));
				}
				else if (before_dir == Direction.BAS && after_dir == Direction.DROITE)
				{
					current.GetComponent<SpriteRenderer>().sprite = spriteAngleDirection;
					current.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 270));
				}
				else if (before_dir == Direction.DROITE && after_dir == Direction.HAUT)
				{
					current.GetComponent<SpriteRenderer>().sprite = spriteAngleDirection;
					current.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
				}
				else if (before_dir == Direction.DROITE && after_dir == Direction.BAS)
				{
					current.GetComponent<SpriteRenderer>().sprite = spriteAngleDirection;
					current.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 270));
				}
				else if (before_dir == Direction.HAUT && after_dir == Direction.GAUCHE)
				{
					current.GetComponent<SpriteRenderer>().sprite = spriteAngleDirection;
					current.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 90));
				}
				else if (before_dir == Direction.HAUT && after_dir == Direction.DROITE)
				{
					current.GetComponent<SpriteRenderer>().sprite = spriteAngleDirection;
					current.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
				}

			}
		}
	}

	void SpawnFood() {
		int x = (int)Random.Range(borderLeft.position.x + 32, borderRight.position.x -32);
		int y = (int)Random.Range(borderBottom.position.y + 32,borderTop.position.y -32);

		x -= x % 32;
		y -= y % 32;

		Instantiate(food,new Vector2(x, y),Quaternion.identity);
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.name.StartsWith ("Food")) {
			foodCollision = true;
			Destroy (coll.gameObject);
			Invoke ("SpawnFood", 0.5f);
		} else if (coll.name.StartsWith ("BorderTop") || coll.name.StartsWith ("BorderBottom") || 
		           coll.name.StartsWith ("BorderLeft") || coll.name.StartsWith ("BorderRight")) {
			Destroy(snake.gameObject);
			canvas.SetActive(true);
		} else if (coll.name.StartsWith("SnakeBody") || coll.name.StartsWith("SnakeTail")){
			Destroy(snake.gameObject);
			canvas.SetActive(true);
		}
	}
}
