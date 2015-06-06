using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    /// <summary>
    /// Camera Initialization with adjusted viewPort
    /// </summary>
    void Start()
    {
        float tileSize = 32f;
        float viewPortWidth = Screen.width - (Screen.width % tileSize);
        float viewPortHeight = Screen.height - (Screen.height % tileSize);

        Camera.main.transform.position = new Vector3((viewPortWidth - tileSize) / 2, (viewPortHeight - tileSize) / 2, -1f);
        Camera.main.orthographicSize = viewPortHeight / 2 ;
        Camera.main.pixelRect = new Rect(0, 0, viewPortWidth, viewPortHeight);    
    }
}
