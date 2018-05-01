using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteEnemy : EnemyEntity {

    private Animator Enemy_Animate;

    // Use this for initialization
    void Start () {
        Enemy_Animate = GetComponent<Animator>();
        Enemy_Animate.SetBool("bl_walking", true);
        agent.SetDestination(target.transform.position);
    }
	
	// Update is called once per frame
	void Update () {

        transform.LookAt(target);

        if (Vector3.Distance(target.position, this.transform.position) < 5)
        {
            Debug.Log("AttackPlayer"); // attack the player function to go here
            agent.isStopped = true; // makes the agent pause its process (is set back to false when player goes >8 units)
            Enemy_Animate.SetBool("bl_attacking", true);
        }

        else
        {
            agent.isStopped = false;
            Enemy_Animate.SetBool("bl_attacking", false);
            Enemy_Animate.SetBool("bl_walking", true);
            agent.SetDestination(target.transform.position + new Vector3(0, 0, 3));
        }

    }
}
