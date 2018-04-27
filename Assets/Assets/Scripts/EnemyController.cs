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

    public void EnemyMove()
    {

        if (Recognized == true)
        {
            agent.SetDestination(target.transform.position);
            Enemy_Animate.SetBool("bl_walking", true);
            agent.isStopped = false;

            if (Vector3.Distance(target.position, this.transform.position) < 8)
            {
                Debug.Log("AttackPlayer"); // attack the player function to go here
                Enemy_Animate.SetBool("bl_walking", false); // can set attack animation to true when implemented
                agent.isStopped = true; // makes the agent pause its process
            }
        }


        else
        {
            Patrol();
        }
    }


    void Patrol()
    {

        //Are there any waypoints defined?
        if (Waypoints.Length > 0)
        {  

            agent.SetDestination(Waypoints[in_next_wp].transform.position);
            Enemy_Animate.SetBool("bl_walking", true);

            // if we get close move to WP target the next
            if (Vector3.Distance(Waypoints[in_next_wp].transform.position, transform.position) < 1)
            {
                if (in_next_wp < Waypoints.Length - 1)
                    in_next_wp++;
                else
                    in_next_wp = 0;
            }
        }
    }

    public void HitByRay()
    {
        Health -= 1;

        if (Health == 0)
       {
          Instantiate(Deresolution, transform.position, transform.rotation); //instatiate the deresolution protocol at game object location
          Destroy(gameObject); //destroy the object this is attached to
       }
        
    }

}

