using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{


    void Start()
    {
        Destroy(gameObject, 2.0f); // destory the bullet after 2 seconds.
    }

    void OnCollisionEnter(Collision collision) //when the colliding with a collider
    {

        if (collision.gameObject.tag == "Player") // if the bullet hits a game object tagged Player
        {
            Destroy(gameObject); // destroy the bullet
        }


    }
}
