using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteEnemy : EnemyEntity //inherits from the enemy entity base class
{
    private Animator Enemy_Animate; // reference the animator compontent in the IDE 

    // Use this for initialization
    public override void Start()
    {
        base.Start(); // ensure the base class is being initialised

        Enemy_Animate = GetComponent<Animator>(); // grab the animator component
        Enemy_Animate.SetBool("bl_walking", true); // set the walking boolean to tru for animator
        agent.SetDestination(target.transform.position); // set the agent's destination as the target
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target); // snure enemy is looking at the player

        if (Vector3.Distance(target.position, transform.position) < 5) // if the enemy is less than 5 units away from target
        {
            agent.isStopped = true; // makes the agent pause its process (is set back to false when player goes >5 units)
            Enemy_Animate.SetBool("bl_attacking", true); // run the attacking animation
            Enemy_Animate.SetBool("bl_walking", false); //ensure the walking animation is not running
        }

        else if (Vector3.Distance(target.position, transform.position) >= 5) // if the target is 5 or more units away
        {
            agent.isStopped = false; // restart the agents process
            Enemy_Animate.SetBool("bl_attacking", false); // ensure it is no longer running the attacking animation
            Enemy_Animate.SetBool("bl_walking", true); // run the walking animation
            agent.SetDestination(target.transform.position + new Vector3(0, 0, 3)); // resets the target destination with a slight offset to stop clipping issues.
        }
    }
}
