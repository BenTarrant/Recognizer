using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyEntity : MonoBehaviour
{

    public float Health;
    public GameObject Deresolution;
    public GameObject Rifle;
    public NavMeshAgent agent;
    [HideInInspector] public Transform target; //Hides in the inspector so it isn't overidden by each enemy's start function but remains public so it can still be called


    // Use this for initialization
    public virtual void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        //print("Position: " + target.transform.position + ", name: " + target.name);
    }

    public virtual void HitByRay(int vDamage) // when the NPC registers a raycast
    {
        Health -= vDamage; // lose health equal to damage received

        if (Health == 0)
        {
            Instantiate(Deresolution, transform.position + (transform.up * 1), transform.rotation); //instatiate the deresolution protocol at game object location
            if ((gameObject.GetComponent("BruteEnemy") as BruteEnemy) != null)
            {
                Instantiate(Rifle, transform.position + (transform.up * 1), transform.rotation);
                Destroy(gameObject); //destroy the object this is attached to
            }

            else
            {
                Destroy(gameObject); //destroy the object this is attached to
            }
                
        }
    }
}
