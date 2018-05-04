using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpEnemy : EnemyEntity // inherits from the enemy entity base class
{

    private Animator Enemy_Animate; // reference for the animator component

    public GameObject BulletPrefab; // reference for the bullet prefab in IDE
    public Transform BulletStart; // reference the transform where the bullet will spawn
    public float BulletSpeed; // reference the speed of the bullet when instatiated - public so can be adjusted
    public float fl_delay; // reference for the delay float
    public float fl_cool_down = 1; // when is affected by the cool down float

    // Use this for initialization

    public override void Start()
    {
        base.Start(); // ensure the base class functionality is being initialised

        Enemy_Animate = GetComponent<Animator>(); // grab the animator component
        Enemy_Animate.SetBool("bl_walking", true); // set the walking bool for animator to true
        agent.SetDestination(target.transform.position); // assign the target (from enemy entity) as the agent's destination
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target); // ensure the enemy is looking at the target

        if (Vector3.Distance(target.position, this.transform.position) < 10) // if the enemy is less than 10 units away from the target
        {
            Enemy_Animate.SetBool("bl_walking", false); // enemy is no longer running the walking animation
            agent.isStopped = true; // makes the agent pause its process
            Enemy_Animate.SetBool("bl_attacking", true); // run the attacking animation
            Attack(); // initiate the attack method
        }

        else
        {
            agent.isStopped = false; // allow the agent to continue its process
            Enemy_Animate.SetBool("bl_attacking", false); //enemy is no longer running attack animation
            Enemy_Animate.SetBool("bl_walking", true); // enemy is running walking animation
            agent.SetDestination(target.transform.position); // reassign the agent destination just to make sure it's following the player still
        }
    }

    void Attack() // attack method
    {
        transform.LookAt(target); // ensure the enemy is still looking at the target


        if (Time.time > fl_delay) // if the delay of the cooldown has passed
        {
            var bullet = (GameObject)Instantiate(BulletPrefab, BulletStart.position, Quaternion.identity); // assign the bullet variable and instatiate the prefab at the referenced transform
            bullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletSpeed; // fetch the rigidbody of the bullet and move it forward, multiplied by bullet speed float
            fl_delay = Time.time + fl_cool_down; // restart the cooldown timer
        }

    }
}
