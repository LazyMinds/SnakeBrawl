using UnityEngine;
using System.Collections;

public class BotCollisionManager : MonoBehaviour
{

    /// <summary>
    /// Collision detection
    /// </summary>
    /// <param name="coll"></param>
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.name.StartsWith("Food"))
        {
            GameManager.botFoodCollision = true;
            Destroy(coll.gameObject);
        } 
        else if (coll.name.StartsWith("Bomb"))
        {
            GameManager.botBombCollision = true;
            Destroy(coll.gameObject);
        }
    }
}
