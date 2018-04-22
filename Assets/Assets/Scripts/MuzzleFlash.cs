using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour {

    public ParticleSystem muzzleFlash; // reference for the muzzle flash particle system

    // Use this for initialization
    void Start () {
		
	}


    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            muzzleFlash.Play();
        }
        
    }


    //public void Flash()
    //   {
    //      muzzleFlash.Play();

    //   }
}
