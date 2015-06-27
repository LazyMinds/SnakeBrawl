using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    /// <summary>
    /// Camera Initialization with adjusted viewPort
    /// </summary>

    public static int fixedWidth = 480;
    public static int fixedHeight = 800;
    public static float tileSize = 32f;

    public static int mapWidth = fixedWidth / 32;
    public static int mapHeight = (fixedHeight - 96)/ 32;
    
    void Start()
    {
        Screen.SetResolution(fixedWidth, fixedHeight, true);
        Camera.main.transform.position = new Vector3((fixedWidth - tileSize) / 2, (fixedHeight - tileSize) / 2, -1f);
        //Camera.main.transform.position = new Vector3(fixedWidth / 2, fixedHeight / 2, -1f);
        Camera.main.orthographicSize = fixedHeight / 2;
        Camera.main.rect = new Rect(0, 0, fixedWidth, fixedHeight);
    }
}
