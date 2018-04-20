using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Derezzed : MonoBehaviour {

    public GameObject Deresoloution; // sets a reference for the game object to be instatiated in the IDE



    private void OnCollisionEnter(Collision collision)//when a collsion is detected by the collider
    {

        if (collision.gameObject.tag == "Bullet") // and if the collider is tagged Bullet
        {
            Instantiate(Deresoloution, transform.position, transform.rotation); //instatiate the deresolution protocol at game object location
            Destroy(gameObject); //destroy the object this is attached to
        }
    }
}
