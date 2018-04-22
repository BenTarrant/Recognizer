using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour {

    public ParticleSystem muzzleFlash; // reference for the muzzle flash particle system

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0)) // if the left mouse button is clicked
        {
            muzzleFlash.Play();
        }

        
    }
}
