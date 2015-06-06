using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Vector3 center = transform.position;

        center.x = Screen.width / 2 - 16f;
        center.y = Screen.height / 2 - 16f;
        center.z = -1f;
        Camera.main.transform.position = center;
    }
}
