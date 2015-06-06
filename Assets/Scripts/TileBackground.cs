using UnityEngine;
using System.Collections;

public class TileBackground : MonoBehaviour
{
    public Transform tileBorder;
    public Transform tileMiddle;

    public float tileSize = 32f;

    //Tilemap width and height
    public int mapWidth = 0;
    public int mapHeight = 0;

    public Transform[,] map;

    void Start()
    {
        mapWidth = (int)(Screen.width / tileSize);
        mapHeight = (int)(Screen.height / tileSize);

        tileBorder = Resources.Load<Transform>("Prefabs/TileBorder");
        tileMiddle = Resources.Load<Transform>("Prefabs/TileMiddle");

        Debug.Log("Sreen Width:" + Screen.width + "Screen Height:" + Screen.height);
        Debug.Log("Map Width:" + mapWidth + "Map Height:" + mapHeight);

        if (!tileBorder || ! tileMiddle)
            Debug.LogWarning("Unable to find TilePrefab in your Resources folder.");

        map = new Transform[mapWidth, mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                Transform tile;
                if (x == 0 || x == mapWidth - 1 || y == 0 || y == mapHeight - 1)
                    tile = Instantiate(tileBorder, new Vector3(x * tileSize, y * tileSize, 0), Quaternion.identity) as Transform;
                else
                    tile = Instantiate(tileMiddle, new Vector3(x * tileSize, y * tileSize, 0), Quaternion.identity) as Transform;
                tile.parent = transform;
                map[x, y] = tile;
            }
        }
    }

    public Transform GetTileAt(int x, int y)
    {
        if (x < 0 || y < 0 || x > mapWidth || y > mapHeight)
        {
            Debug.LogWarning("X or Y coordinate is out of bounds!");
            return null;
        }
        return map[x, y];
    }
}
