using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    //Movement
    public NavMeshAgent agent;
    public Transform target;

    //Animation
    private Animator Enemy_Animate;

    //Patrolling
    public GameObject[] Waypoints;
    private int in_next_wp = 0;

    //Health and Deresolution
    public GameObject Deresolution; // sets a reference for the game object to be instatiated in the IDE
    public float Health = 2;

    //Attack
    public bool Recognized;


    // Use this for initialization
    void Start() {

        Enemy_Animate = GetComponent<Animator>();
        Enemy_Animate.SetBool("bl_walking", false);
        Recognized = false;

    }


    void Update()
    {

    }

    public void EnemyMove() //execute enemy move function
    {

        if (Recognized == true) // if the Recognized boolean is set to true (so if the player is within the NPC's line of sight)
        {
            agent.SetDestination(target.transform.position); // assign the player (target) as the agent's (NPC) target
            Enemy_Animate.SetBool("bl_walking", true); //ensure walking bool is set to true for animation (so we can set it to false later)
            agent.isStopped = false; // Ensure the agent is not stopped (so we can stop it later)

            if (Vector3.Distance(target.position, this.transform.position) < 8)
            {
                Debug.Log("AttackPlayer"); // attack the player function to go here
                Enemy_Animate.SetBool("bl_walking", false); // can set attack animation to true when implemented (is set back to true when player goes >8 units)
                agent.isStopped = true; // makes the agent pause its process (is set back to false when player goes >8 units)
            }
        }


        else
        {
            Patrol(); // execute patrol function
        }
    }


    void Patrol()
    {

        //Are there any waypoints defined?
        if (Waypoints.Length > 0)
        {  

            agent.SetDestination(Waypoints[in_next_wp].transform.position);//set the agents destination as the next Waypoint in the array
            Enemy_Animate.SetBool("bl_walking", true); // ensure the iswalking bool is set to true (so the NPC animates while approaching destinations)

            // if we get close move to WP target the next
            if (Vector3.Distance(Waypoints[in_next_wp].transform.position, transform.position) < 1)
            {
                if (in_next_wp < Waypoints.Length - 1)
                    in_next_wp++;
                else
                    in_next_wp = 0; // restart the waypoint rotation
            }
        }
    }

    public void HitByRay() // when the NPC registers a raycast
    {

        Debug.Log("I was hit by a Ray");
        Health -= 1; // lose 1 health

       if (Health == 0) // if health becomes 0

        Health -= 1;

        if (Health == 0)
       {
          Instantiate(Deresolution, transform.position, transform.rotation); //instatiate the deresolution protocol at game object location
          Destroy(gameObject); //destroy the object this is attached to
       }
        
    }

}

