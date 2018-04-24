using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour {

    public ParticleSystem muzzleFlash1;// reference for the muzzle flash 1 particle system
    public ParticleSystem muzzleFlash2;// reference for the muzzle flash 2 particle system

  public void Flash1() // function to play muzzle flash for weapon 1
   {
      muzzleFlash1.Play();

   }

    public void Flash2() // function to play muzzle flash for weapon 2
    {
        muzzleFlash2.Play();

    }
}
