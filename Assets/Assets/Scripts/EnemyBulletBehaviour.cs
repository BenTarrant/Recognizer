using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 2.0f);
    }

    void OnCollisionEnter(Collision collision) //when the colliding with a collider
    {

        if (collision.gameObject.tag == "Player") // if the raycast hits an object tagged enemy
        {
            Debug.Log("HitPlayer");
            Destroy(gameObject);
        }

        else
        {
            Destroy(gameObject, 1.0f);
        }

    }
}
