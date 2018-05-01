using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpEnemy : EnemyEntity {

    private Animator Enemy_Animate;

    public GameObject BulletPrefab;
    public Transform BulletStart;
    public float BulletSpeed;
    public float fl_delay;
    public float fl_cool_down = 1;

    // Use this for initialization
    void Start () {
        Enemy_Animate = GetComponent<Animator>();
        Enemy_Animate.SetBool("bl_walking", true);
        agent.SetDestination(target.transform.position);
    }
	
	// Update is called once per frame
	void Update () {

        transform.LookAt(target);

        if (Vector3.Distance(target.position, this.transform.position) < 10)
        {
            Debug.Log("AttackPlayer"); // attack the player function to go here
            Enemy_Animate.SetBool("bl_walking", false); // can set attack animation to true when implemented (is set back to true when player goes >8 units)
            agent.isStopped = true; // makes the agent pause its process (is set back to false when player goes >8 units)
            Enemy_Animate.SetBool("bl_attacking", true);
            Attack();
        }

        else
        {
            agent.isStopped = false;
            Enemy_Animate.SetBool("bl_attacking", false);
            Enemy_Animate.SetBool("bl_walking", true);
            agent.SetDestination(target.transform.position);
        }
    }

    void Attack()
    {
        transform.LookAt(target);


        if (Time.time > fl_delay)
        {
            var bullet = (GameObject)Instantiate(BulletPrefab, BulletStart.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletSpeed;
            fl_delay = Time.time + fl_cool_down;
        }

    }
}
