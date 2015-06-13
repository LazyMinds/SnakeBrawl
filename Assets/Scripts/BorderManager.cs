using UnityEngine;
using System.Collections;

public class BorderManager : MonoBehaviour
{
    public float tileSize = CameraManager.tileSize;
    public int mapWidth = 0;
    public int mapHeight = 0;

    void Start()
    {
        mapWidth = CameraManager.mapWidth;
        mapHeight = CameraManager.mapHeight;

        Transform tileRed = Resources.Load<Transform>("Prefabs/TileRedBlock");
        Transform tileYellow = Resources.Load<Transform>("Prefabs/TileYellowBlock");

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                Transform tile = null;
                if ((x == 0 && y == 0) || (x == mapWidth - 1 && y == 0) || (x == 0 && y == mapHeight -1) || (x == mapWidth - 1 && y == mapHeight - 1))
                    tile = Instantiate(tileYellow, new Vector3(x * tileSize, y * tileSize, 0), Quaternion.identity) as Transform;
                else if (x == 0 || x == mapWidth - 1 || y == 0 || y == mapHeight - 1)
                    tile = Instantiate(tileRed, new Vector3(x * tileSize, y * tileSize, 0), Quaternion.identity) as Transform;
                if (tile)
                    tile.parent = transform;
            }
        }
    }
}
