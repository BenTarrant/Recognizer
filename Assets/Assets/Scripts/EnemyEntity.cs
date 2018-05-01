using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyEntity : MonoBehaviour {

    public float Health;
    public GameObject Deresolution;
    public NavMeshAgent agent;
    public Transform target;




    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

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
