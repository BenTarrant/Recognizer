using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostPixelEnemy : EnemyEntity
{
    public float MoveSpeed = 4;
    public float MaxDist = 10;
    public float MinDist = 0;




    void Start()
    {

    }

    void Update()
    {
        transform.LookAt(target);
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) <= MaxDist)
        {
            MoveSpeed += 5.0f;
        }

    }

    void OnCollisionEnter(Collision collision)
    {

      Debug.Log("HitPlayer");
      Instantiate(Deresolution, transform.position, transform.rotation);
      Destroy(gameObject);

    }
}

