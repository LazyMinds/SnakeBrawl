using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static bool snakeFoodCollision;
    public static bool snakeBombCollision;

    public static bool botFoodCollision;
    public static bool botBombCollision;

    private List<Transform> snakeList = new List<Transform>();
    private List<Transform> botList = new List<Transform>();
    private List<Transform> borderList = new List<Transform>();
    private GameObject food = null;
    private GameObject bomb = null;
    private float borderRotation = 0;
    private float tileSize = 0;
    private int mapWidth = 0;
    private int mapHeight = 0;
    private Element[,] matrix = null;
    private Vector2 snakeDirection = Vector2.up;
    private Vector2 botDirection = Vector2.down;

    private float botDifficulty = 0.20f;

    private enum Direction
    {
        UNDEFINED,
        LEFT,
        RIGHT,
        UP,
        DOWN
    };

    private enum Element
    {
        EMPTY,
        BORDER,
        SNAKE,
        BOT,
        FOOD,
        BOMB
    }

    void Start()
    {
        InitializeBoard();
        InitializeBorder();
        InitializeBot();
        InitializeSnake();
        InitializeFood();
    }

    void Update()
    {
        Direction inputDir = getInputResult();
        if (inputDir == Direction.UP)
        {
            if (snakeDirection != -Vector2.up)
                snakeDirection = Vector2.up;
        }
        else if (inputDir == Direction.DOWN)
        {
            if (snakeDirection != Vector2.up)
                snakeDirection = -Vector2.up;
        }
        else if (inputDir == Direction.LEFT)
        {
            if (snakeDirection != Vector2.right)
                snakeDirection = -Vector2.right;
        }
        else if (inputDir == Direction.RIGHT)
        {
            if (snakeDirection != -Vector2.right)
                snakeDirection = Vector2.right;
        }
    }

    void DebugMatrix()
    {
        String debugString = null;

        for (int y = mapHeight - 1; y >= 0; --y)
        {
            for (int x = 0; x < mapWidth; ++x)
            {
                debugString += (int)matrix[x, y] + " ";
            }
            debugString += "\n";
        }
        Debug.Log(debugString);
    }

    void InitializeBoard()
    {
        tileSize = CameraManager.tileSize;
        mapWidth = CameraManager.mapWidth;
        mapHeight = CameraManager.mapHeight;
        matrix = new Element[mapWidth, mapHeight];

        for (int y = 0; y < mapHeight; ++y)
        {
            for (int x = 0; x < mapWidth; ++x)
            {
                matrix[x, y] = Element.EMPTY;
            }
        }
    }

    void InitializeBorder()
    {
        Transform tileBorder = Resources.Load<Transform>("Prefabs/TileBorderBlock");

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                Transform tile = null;
                if ((x == 0 && y == 0) || (x == mapWidth - 1 && y == 0) || (x == 0 && y == mapHeight - 1) ||
                    (x == mapWidth - 1 && y == mapHeight - 1) || x == 0 || x == mapWidth - 1 || y == 0 || y == mapHeight - 1)
                {
                    matrix[x, y] = Element.BORDER;
                    tile = Instantiate(tileBorder, new Vector3(x * tileSize, y * tileSize, 0), Quaternion.identity) as Transform;
                    borderList.Add(tile);
                }
            }
        }
        InvokeRepeating("RotateBorder", 0f, 0.05f);
    }

    void RotateBorder()
    {
        foreach (Transform tile in borderList)
        {
            borderRotation += 10;
            tile.rotation = Quaternion.Euler(new Vector3(0, 0, borderRotation));
        }
    }

    void InitializeSnake()
    {
        Transform snakeHead = Resources.Load<Transform>("Prefabs/SnakeHead");
        Transform snakeTail = Resources.Load<Transform>("Prefabs/SnakeTail");

        int x = mapWidth / 2;
        int y = 5;

        Transform head = Instantiate(snakeHead, new Vector3(x * tileSize, y * tileSize, 0), Quaternion.identity) as Transform;
        Transform tail1 = Instantiate(snakeTail, new Vector3(x * tileSize, (y - 1) * tileSize, 0), Quaternion.identity) as Transform;
        Transform tail2 = Instantiate(snakeTail, new Vector3(x * tileSize, (y - 2) * tileSize, 0), Quaternion.identity) as Transform;
        Transform tail3 = Instantiate(snakeTail, new Vector3(x * tileSize, (y - 3) * tileSize, 0), Quaternion.identity) as Transform;
        matrix[x, y] = Element.SNAKE;
        matrix[x, y - 1] = Element.SNAKE;
        matrix[x, y - 2] = Element.SNAKE;
        matrix[x, y - 3] = Element.SNAKE;
        snakeList.Add(head);
        snakeList.Add(tail1);
        snakeList.Add(tail2);
        snakeList.Add(tail3);
        InvokeRepeating("SnakeUpdate", 0f, 0.20f);
    }

    void SnakeUpdate()
    {
        Vector2 temp;
        Vector2 oldPos = snakeList[0].position;
        snakeList[0].Translate(snakeDirection * tileSize);

        int x = (int)(snakeList[0].position.x / tileSize);
        int y = (int)(snakeList[0].position.y / tileSize);
        matrix[x, y] = Element.SNAKE;

        for (int i = 1; i < snakeList.Count; ++i)
        {
            temp = oldPos;
            oldPos = snakeList[i].position;
            snakeList[i].position = temp;
        }

        x = (int)(oldPos.x / tileSize);
        y = (int)(oldPos.y / tileSize);
        matrix[x, y] = Element.EMPTY;

        if (snakeFoodCollision)
        {
            Transform snakeTail = Resources.Load<Transform>("Prefabs/SnakeTail");
            Transform tail = Instantiate(snakeTail, oldPos, Quaternion.identity) as Transform;
            snakeList.Add(tail);
            matrix[x, y] = Element.SNAKE;
            snakeFoodCollision = false;
        }
        else if (snakeBombCollision)
        {
            int posx = (int)(botList[botList.Count - 1].position.x / tileSize);
            int posy = (int)(botList[botList.Count - 1].position.y / tileSize);
            Destroy(botList[botList.Count - 1].gameObject);
            botList.RemoveAt(botList.Count - 1);
            matrix[posx, posy] = Element.EMPTY;
            snakeBombCollision = false;

            if (botList.Count == 0)
            {
                CancelInvoke("BotUpdate");
                botDifficulty -= 0.05f;
                InitializeBot();
            }
        }
    }

    void InitializeBot()
    {
        Transform botHead = Resources.Load<Transform>("Prefabs/botHead");
        Transform botTail = Resources.Load<Transform>("Prefabs/botTail");

        int x = mapWidth / 2;
        int y = 15;

        Transform head = Instantiate(botHead, new Vector3(x * tileSize, y * tileSize, 0), Quaternion.identity) as Transform;
        Transform tail1 = Instantiate(botTail, new Vector3(x * tileSize, (y + 1) * tileSize, 0), Quaternion.identity) as Transform;
        Transform tail2 = Instantiate(botTail, new Vector3(x * tileSize, (y + 2) * tileSize, 0), Quaternion.identity) as Transform;
        Transform tail3 = Instantiate(botTail, new Vector3(x * tileSize, (y + 3) * tileSize, 0), Quaternion.identity) as Transform;
        matrix[x, y] = Element.BOT;
        matrix[x, y + 1] = Element.BOT;
        matrix[x, y + 2] = Element.BOT;
        matrix[x, y + 3] = Element.BOT;
        botList.Add(head);
        botList.Add(tail1);
        botList.Add(tail2);
        botList.Add(tail3);
        InvokeRepeating("BotUpdate", 0f, botDifficulty);
    }

    void BotUpdate()
    {
        int x = (int)(botList[0].position.x / tileSize);
        int y = (int)(botList[0].position.y / tileSize);
        if (matrix[x, y - 1] != Element.BORDER && botDirection != Vector2.up)
        {
            botDirection = Vector2.down;
        }
        else if (matrix[x, y + 1] != Element.BORDER && botDirection != Vector2.down)
        {
            botDirection = Vector2.up;
        }
        else if (matrix[x - 1, y] != Element.BORDER && botDirection != Vector2.right)
        {
            botDirection = Vector2.left;
        }
        else if (matrix[x + 1, y] != Element.BORDER && botDirection != Vector2.left)
        {
            botDirection = Vector2.right;
        }

        Vector2 temp;
        Vector2 oldPos = botList[0].position;
        botList[0].Translate(botDirection * tileSize);

        x = (int)(botList[0].position.x / tileSize);
        y = (int)(botList[0].position.y / tileSize);
        matrix[x, y] = Element.BOT;

        for (int i = 1; i < botList.Count; ++i)
        {
            temp = oldPos;
            oldPos = botList[i].position;
            botList[i].position = temp;
        }

        x = (int)(oldPos.x / tileSize);
        y = (int)(oldPos.y / tileSize);
        matrix[x, y] = Element.EMPTY;

        if (botFoodCollision)
        {
            Transform botTail = Resources.Load<Transform>("Prefabs/BotTail");
            Transform tail = Instantiate(botTail, oldPos, Quaternion.identity) as Transform;
            botList.Add(tail);
            matrix[x, y] = Element.BOT;
            botFoodCollision = false;
        }
        else if (botBombCollision)
        {
            int posx = (int)(snakeList[snakeList.Count - 1].position.x / tileSize);
            int posy = (int)(snakeList[snakeList.Count - 1].position.y / tileSize);
            Destroy(snakeList[snakeList.Count - 1].gameObject);
            snakeList.RemoveAt(snakeList.Count - 1);
            matrix[posx, posy] = Element.EMPTY;
            botBombCollision = false;

            if (snakeList.Count == 0)
            {
                Application.LoadLevel("GameOverScene");
            }
        }

        DebugMatrix();
    }

    void InitializeFood()
    {
        food = Resources.Load<GameObject>("Prefabs/Food");
        bomb = Resources.Load<GameObject>("Prefabs/Bomb");

        snakeFoodCollision = false;
        snakeBombCollision = false;

        botFoodCollision = false;
        botBombCollision = false;

        InvokeRepeating("SpawnFood", 1f, 5f);
        InvokeRepeating("SpawnBomb", 0.5f, 5f);
    }

    bool IsEmpty(int x, int y)
    {
        if (matrix[x, y] == Element.EMPTY)
            return true;
        return false;
    }

    void SpawnFood()
    {
        int x = 0;
        int y = 0;

        do
        {
            x = (int)(UnityEngine.Random.Range(1, mapWidth - 1));
            y = (int)(UnityEngine.Random.Range(1, mapHeight - 1));
        } while (!IsEmpty(x, y));

        Instantiate(food, new Vector2(x * tileSize, y * tileSize), Quaternion.identity);
        matrix[x, y] = Element.FOOD;
    }

    void SpawnBomb()
    {
        int x = 0;
        int y = 0;

        do
        {
            x = (int)(UnityEngine.Random.Range(1, mapWidth - 1));
            y = (int)(UnityEngine.Random.Range(1, mapHeight - 1));
        } while (!IsEmpty(x, y));

        Instantiate(bomb, new Vector2(x * tileSize, y * tileSize), Quaternion.identity);
        matrix[x, y] = Element.BOMB;
    }

    Direction getInputResult()
    {
        if (Input.GetKey("up"))
            return Direction.UP;
        else if (Input.GetKey("down"))
            return Direction.DOWN;
        else if (Input.GetKey("left"))
            return Direction.LEFT;
        else if (Input.GetKey("right"))
            return Direction.RIGHT;
        return Direction.UNDEFINED;
    }
}
