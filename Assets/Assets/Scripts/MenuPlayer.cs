using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPlayer : MonoBehaviour {

    public float movementSpeed = 5.0f; // public reference to movement speed
    public float mouseSensitivity = 5.0f;// public reference for mouse sensitivity



    // Use this for initialization
    void Start () {


    }
	
	// Update is called once per frame
	void Update () {

        float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotLeftRight, 0);

        float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed; //sets the speed the player moves forward/back
        float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed; // sets the speed the player moves left/right
        Vector3 speed = new Vector3(sideSpeed, 0, forwardSpeed); //creats new vector 3 based on directional speeds
        speed = transform.rotation * speed;
        CharacterController cc = GetComponent<CharacterController>(); // finds the chracter controller
        cc.SimpleMove(speed);

    }

    void OnParticleCollision(GameObject other) // when player collides with a particle system (the teleport)
    {
         StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        
        yield return new WaitForSeconds(0.5f);// wait for 2.5 seconds
        SceneManager.LoadScene("Level01"); // load the scene title X

    }
}
