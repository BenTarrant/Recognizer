using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour {

    public float BitSpeed = 250.0f; // reference to the speed of the bits in the IDE

    
    void Start () {

        DestroyObjectDelayed(); // start function 

    }

    private void Update()
    {
        var bit = (gameObject); //set a refernece to the gameobject 
        bit.GetComponentInChildren<Rigidbody>().velocity = transform.up * BitSpeed; //fetch the GO childrens rigidbodies and apply velocity in up vector multiplied by bitspeed float
    }


    void DestroyObjectDelayed()
    {
        
        Destroy(gameObject, 2.5f); // Kills the game object in 2.5 seconds after loading the object
    }
}
