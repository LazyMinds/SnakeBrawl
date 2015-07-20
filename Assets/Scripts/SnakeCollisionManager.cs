using UnityEngine;
using System.Collections;

public class SnakeCollisionManager : MonoBehaviour
{
    /// <summary>
    /// Collision detection
    /// </summary>
    /// <param name="coll"></param>
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.name.StartsWith("Food"))
        {
            GameManager.snakeFoodCollision = true;
            Destroy(coll.gameObject);
        }
        else if (coll.name.StartsWith("Bomb"))
        {
            GameManager.snakeBombCollision = true;
            Destroy(coll.gameObject);
        }
        else if (coll.name.StartsWith("TileBorderBlock") || coll.name.StartsWith("SnakeTail"))
        {
            Application.LoadLevel("GameOverScene");
        }
    }
}
