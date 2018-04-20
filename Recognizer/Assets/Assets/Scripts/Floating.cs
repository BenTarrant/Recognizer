using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour {

    public float FloatStrength; // reference for speed of float
    public float RandomRotationStrength; // reference for speed of rotation 

    private void Start()
    {
        GetComponent<Rigidbody>(); // fetch the rigidbody
    }


    void FixedUpdate()
    {
        transform.GetComponent<Rigidbody>().AddForce(Vector3.up * FloatStrength); //apply force UP multiplied by float strength
        transform.Rotate(RandomRotationStrength, RandomRotationStrength, RandomRotationStrength); // apply random rotation 
    }
}
