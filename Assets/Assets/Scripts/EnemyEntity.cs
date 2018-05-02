using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyEntity : MonoBehaviour
{

    public float Health;
    public GameObject Deresolution;
    public NavMeshAgent agent;
    [HideInInspector] public Transform target; //Hides in the inspector so it isn't overidden by each enemy's start function but remains public so it can still be called


    // Use this for initialization
    public virtual void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;

        if (!target)
        {
            Debug.Log("No Target in EnemyEntity.cs");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //print("Position: " + target.transform.position + ", name: " + target.name);
    }

    public virtual void HitByRay() // when the NPC registers a raycast
    {
        Health -= 1; // lose 1 health

        if (Health == 0)
        {
            Instantiate(Deresolution, transform.position, transform.rotation); //instatiate the deresolution protocol at game object location
            Destroy(gameObject); //destroy the object this is attached to
        }
    }
}
