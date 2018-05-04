using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostPixelEnemy : EnemyEntity
{
    public float MoveSpeed = 4; // reference for enemy movement speed
    public float ImpactDist = 10; // reference for when enemy should speed up




    public override void Start()
    {
        base.Start(); // ensure base class is initialised
    }

    void Update()
    {
        transform.LookAt(target); // ensure enemy is looking at target
        transform.position += transform.forward * MoveSpeed * Time.deltaTime; // move the enemy forward smoothly at specified movespeed

        if (Vector3.Distance(transform.position, target.position) <= ImpactDist) // if the enemy is within impact range
        {
            MoveSpeed += 5.0f; // add additional 5 to movespeed (speed up)
        }

    }

    void OnCollisionEnter(Collision collision) // when the enemy collides with the player
    {

        Instantiate(Deresolution, transform.position, transform.rotation); // instatiate the desrolution protocol at current transform
        Destroy(gameObject); // destory this GO

    }
}

