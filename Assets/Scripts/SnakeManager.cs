﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SnakeManager : MonoBehaviour {

	//Prefabs
	public GameObject snake = null;
	public GameObject snakeTail = null;
	public GameObject food = null;
	public GameObject canvas = null;

	//Border
	public Transform borderTop = null;
	public Transform borderBottom = null;
	public Transform borderLeft = null;
	public Transform borderRight = null;

	List<Transform> snakeTailList = new List<Transform>();
	private Vector2 m_direction = Vector2.right;
	private bool foodCollision = false;

	void Start () {
		InvokeRepeating ("Move", 0f, 0.15f);
		Invoke("SpawnFood", 1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("up"))
			m_direction = Vector2.up;
		else if (Input.GetKey("down"))
			m_direction = -Vector2.up;
		else if (Input.GetKey ("left"))
			m_direction = -Vector2.right;
		else if (Input.GetKey("right"))
			m_direction = Vector2.right;
	}

	void Move() {
		Vector2 v = snake.transform.position;
		snake.transform.Translate (m_direction*32, Space.World);

		if (m_direction == Vector2.up)
			snake.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
		else if (m_direction == -Vector2.up)
			snake.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 180));
		else if (m_direction == Vector2.right)
			snake.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 270));
		else if (m_direction == -Vector2.right)
			snake.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 90));
		
		if (foodCollision) {
			GameObject tail =(GameObject)Instantiate(snakeTail, v, Quaternion.identity);
			snakeTailList.Insert(0, tail.transform);
			foodCollision = false;
		}
		else if (snakeTailList.Count > 0) {
			snakeTailList.Last().position = v;
			snakeTailList.Insert(0, snakeTailList.Last());
			snakeTailList.RemoveAt(snakeTailList.Count-1);
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
		} else if (coll.name.StartsWith("SnakeTail")){
			Destroy(snake.gameObject);
			canvas.SetActive(true);
		}
	}
}
