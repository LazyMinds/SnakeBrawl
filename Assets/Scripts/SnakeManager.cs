using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SnakeManager : MonoBehaviour
{

    public GameObject snakeHead = null;
    public GameObject snakeBody = null;
    public GameObject snakeTail = null;
    public GameObject food = null;
    public GameObject canvas = null;
    public Sprite spriteHorVerDirection = null;
    public Sprite spriteAngleDirection = null;

    public Transform borderTop = null;
    public Transform borderBottom = null;
    public Transform borderLeft = null;
    public Transform borderRight = null;

    private List<GameObject> snakeList = new List<GameObject>();
    private Vector2 direction = Vector2.up;
    private bool foodCollision = false;
    private GameObject bodyObject;
    private GameObject foodObject;

    enum Direction { LEFT, RIGHT, UP, DOWN };

    /// <summary>
    /// Initialization
    /// </summary>
    void Start()
    {
        snakeList.Add(snakeHead);
        snakeList.Add(snakeTail);
        InvokeRepeating("Move", 0f, 0.15f);
        Invoke("SpawnFood", 1f);
    }

    /// <summary>
    /// Update for every frame
    /// </summary>
    void Update()
    {
        if (Input.GetKey("up"))
        {
            if (direction != -Vector2.up)
                direction = Vector2.up;
        }
        else if (Input.GetKey("down"))
        {
            if (direction != Vector2.up)
                direction = -Vector2.up;
        }
        else if (Input.GetKey("left"))
        {
            if (direction != Vector2.right)
                direction = -Vector2.right;
        }
        else if (Input.GetKey("right"))
        {
            if (direction != -Vector2.right)
                direction = Vector2.right;
        }
    }

    /// <summary>
    /// Object rotation following the direction
    /// </summary>
    /// <param name="obj"></param>
    void Rotate(GameObject obj)
    {
        if (direction == Vector2.up)
            obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        else if (direction == -Vector2.up)
            obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        else if (direction == Vector2.right)
            obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
        else if (direction == -Vector2.right)
            obj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
    }

    /// <summary>
    /// Check the body direction
    /// </summary>
    /// <param name="current"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    Direction CheckBodyDirection(Vector2 current, Vector2 other)
    {
        if (other.x < current.x)
            return Direction.LEFT;
        else if (other.x > current.x)
            return Direction.RIGHT;
        else if (other.y > current.y)
            return Direction.UP;
        else
            return Direction.DOWN;
    }

    /// <summary>
    /// Update the body angle
    /// </summary>
    /// <param name="current"></param>
    /// <param name="previous"></param>
    /// <param name="next"></param>
    void UpdateBodyAngle(GameObject current, Direction previous, Direction next)
    {
        if ((previous == Direction.LEFT && next == Direction.RIGHT) || (previous == Direction.RIGHT && next == Direction.LEFT))
        {
            current.GetComponent<SpriteRenderer>().sprite = spriteHorVerDirection;
            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        else if ((previous == Direction.UP && next == Direction.DOWN) || (previous == Direction.DOWN && next == Direction.UP))
        {
            current.GetComponent<SpriteRenderer>().sprite = spriteHorVerDirection;
            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if ((previous == Direction.LEFT && next == Direction.DOWN) || (previous == Direction.DOWN && next == Direction.LEFT))
        {
            current.GetComponent<SpriteRenderer>().sprite = spriteAngleDirection;
            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else if ((previous == Direction.LEFT && next == Direction.UP) || (previous == Direction.UP && next == Direction.LEFT))
        {
            current.GetComponent<SpriteRenderer>().sprite = spriteAngleDirection;
            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        else if ((previous == Direction.DOWN && next == Direction.RIGHT) || (previous == Direction.RIGHT && next == Direction.DOWN))
        {
            current.GetComponent<SpriteRenderer>().sprite = spriteAngleDirection;
            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
        }
        else if ((previous == Direction.RIGHT && next == Direction.UP) || (previous == Direction.UP && next == Direction.RIGHT))
        {
            current.GetComponent<SpriteRenderer>().sprite = spriteAngleDirection;
            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }

    /// <summary>
    /// Snake movements
    /// </summary>
    void Move()
    {
        Vector2 oldHeadPosition = snakeHead.transform.position;
        snakeHead.transform.Translate(direction * 32, Space.World);
        Rotate(snakeHead);

        if (foodCollision)
        {
            bodyObject = Instantiate(snakeBody, oldHeadPosition, Quaternion.identity) as GameObject;
            snakeList.Insert(1, bodyObject);
            Rotate(snakeList.ElementAt(1));
            foodCollision = false;
        }
        else if (snakeList.Count > 1)
        {
            Vector2 previousPosition = oldHeadPosition;
            Vector2 currentPosition;
            for (int i = 1; i < snakeList.Count; i++)
            {
                currentPosition = snakeList.ElementAt(i).transform.position;
                snakeList.ElementAt(i).transform.position = previousPosition;
                previousPosition = currentPosition;
            }
        }

        if (snakeList.Count > 1)
        {
            for (int i = 1; i < snakeList.Count; i++)
            {
                GameObject current = snakeList[i];
                Direction previous = CheckBodyDirection(current.transform.position, snakeList[i - 1].transform.position);

                if (i + 1 < snakeList.Count)
                {
                    Direction next = CheckBodyDirection(current.transform.position, snakeList[i + 1].transform.position);
                    UpdateBodyAngle(current, previous, next);
                }
                else if (i == snakeList.Count - 1)
                {
                    switch (previous)
                    {
                        case Direction.LEFT:
                            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                            break;
                        case Direction.RIGHT:
                            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
                            break;
                        case Direction.UP:
                            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                            break;
                        case Direction.DOWN:
                            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 170));
                            break;
                    }
                    return;
                }
            }
        }
    }

    bool checkCollisionWithBody(int x, int y)
    {
        for (int i = 0; i < snakeList.Count; i++)
        {
            if (x == snakeList[i].transform.position.x && y == snakeList[i].transform.position.y)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Food spawning randomly on the board
    /// </summary>
    void SpawnFood()
    {
        int x = 0;
        int y = 0;

        do
        {
            x = (int)Random.Range(borderLeft.position.x + 32, borderRight.position.x - 32);
            y = (int)Random.Range(borderBottom.position.y + 32, borderTop.position.y - 32);

            x -= x % 32;
            y -= y % 32;
        } while (checkCollisionWithBody(x, y));

        foodObject = Instantiate(food, new Vector2(x, y), Quaternion.identity) as GameObject;
    }

    /// <summary>
    /// Collision detection
    /// </summary>
    /// <param name="coll"></param>
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.name.StartsWith("Food"))
        {
            foodCollision = true;
            Destroy(coll.gameObject);
            Invoke("SpawnFood", 0.5f);
        }
        else if (coll.name.StartsWith("BorderTop") || coll.name.StartsWith("BorderBottom") ||
                 coll.name.StartsWith("BorderLeft") || coll.name.StartsWith("BorderRight"))
        {
            DestroyGameObject();
            canvas.SetActive(true);
        }
        else if (coll.name.StartsWith("SnakeBody") || coll.name.StartsWith("SnakeTail"))
        {
            DestroyGameObject();
            canvas.SetActive(true);
        }
    }

    /// <summary>
    /// Destroy the game object on the board
    /// </summary>
    void DestroyGameObject()
    {
        for (int i = 0; i < snakeList.Count; i++)
            Destroy(snakeList[i].gameObject);
        Destroy(foodObject);
    }
}
