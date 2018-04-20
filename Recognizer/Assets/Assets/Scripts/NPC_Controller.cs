using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour {

    private Animator NPC_Animator;
    private CharacterController CC_NPC;

    // Use this for initialization
    void Start () {

        CC_NPC = GetComponent<CharacterController>();
        NPC_Animator = GetComponentInChildren<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void NPC_Animate()
    {
        // Set walking state for Animator
        if (CC_NPC.velocity.x != 0 || CC_NPC.velocity.z != 0)
            NPC_Animator.SetBool("bl_walk", true);
        else
            NPC_Animator.SetBool("bl_walk", false);
    }//-----
}
