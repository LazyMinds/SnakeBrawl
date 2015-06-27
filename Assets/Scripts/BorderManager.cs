using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BorderManager : MonoBehaviour
{
    public float tileSize = CameraManager.tileSize;
    public int mapWidth = 0;
    public int mapHeight = 0;

    private List<Transform> borderList = new List<Transform>();
    private float z = 0;

    void Start()
    {
        mapWidth = CameraManager.mapWidth;
        mapHeight = CameraManager.mapHeight;

        Transform tileBorder = Resources.Load<Transform>("Prefabs/TileBorderBlock");

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                Transform tile = null;
                if ((x == 0 && y == 0) || (x == mapWidth - 1 && y == 0) || (x == 0 && y == mapHeight -1) || 
                    (x == mapWidth - 1 && y == mapHeight - 1) || x == 0 || x == mapWidth - 1 || y == 0 || y == mapHeight - 1)
                {
                    tile = Instantiate(tileBorder, new Vector3(x * tileSize, y * tileSize, 0), Quaternion.identity) as Transform;
                    borderList.Add(tile);
                }
                if (tile)
                    tile.parent = transform;
            }
        }
        InvokeRepeating("RotateBorder", 0f, 0.05f);
    }

    void RotateBorder()
    {
        foreach (Transform tile in borderList)
        {
            z += 10;
            tile.rotation = Quaternion.Euler(new Vector3(0, 0, z));
        }
    }
}
