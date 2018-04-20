using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour {

    public Transform Player;
    public float MoveSpeed = 4;
    public float MaxDist = 10;
    public float MinDist = 5;
    private Animator Enemy_Animate;
    public CharacterController CC_NPC;

    void Start()
    {
        CC_NPC = GetComponentInParent<CharacterController>();
        Enemy_Animate = GetComponent<Animator>();
    }


    void Update()
    {
        transform.LookAt(Player);

        if (Vector3.Distance(transform.position, Player.position) >= MinDist)
        {

            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
            NPC_Animate();



            if (Vector3.Distance(transform.position, Player.position) <= MaxDist)
            {
                NPC_Animate();
            }

        }
    }

    void NPC_Animate()
    {
        if (CC_NPC.velocity.x != 0 || CC_NPC.velocity.z != 0)
            Enemy_Animate.SetBool("bl_walking", true);
       else
           Enemy_Animate.SetBool("bl_walking", false);
    }
}

