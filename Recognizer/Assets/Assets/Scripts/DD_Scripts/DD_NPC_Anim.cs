using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DD_NPC_Anim : MonoBehaviour
{
    private Animator NPC_Animator;
    private CharacterController CC_NPC;

    // Use this for initialization
    void Start()
    {
        CC_NPC = GetComponent<CharacterController>();
        NPC_Animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        NPC_Animate();
    
        }

    void NPC_Animate()
    {
        if (CC_NPC.velocity.x != 0 || CC_NPC.velocity.z != 0)
            NPC_Animator.SetBool("bl_walking", true);
        else
            NPC_Animator.SetBool("bl_walking", false);
    }//-----

}
