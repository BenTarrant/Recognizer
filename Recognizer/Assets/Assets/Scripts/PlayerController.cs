using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 5.0f; // public reference to movement speed
    public float mouseSensitivity = 5.0f;// public reference for mouse sensitivity

    public GameObject BulletPrefab;
    public Transform BulletStart;
    public float BulletSpeed = 5.0f;

    //CursorLockMode wantedMode;

    void Start()
    {
        // Apply requested cursor state
        SetCursorState();
    }

    
    void SetCursorState()
    {
        Cursor.lockState = CursorLockMode.Locked; // Stops cursor moving during play
    }


    // Update is called once per frame
    void Update()
    {

        
        //Camera Roatation
        float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity; 
        transform.Rotate(0, rotLeftRight, 0);
        // causes mouse to move horizontal roatation based on mouse sensitivty float
        //Could eventually be incorporated into a UI slider so player can edit


        //Player Movement
        float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed; //sets the speed the player moves forward/back
        float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed; // sets the speed the player moves left/right
        Vector3 speed = new Vector3(sideSpeed, 0, forwardSpeed); //creats new vector 3 based on directional speeds
        speed = transform.rotation * speed;
        CharacterController cc = GetComponent<CharacterController>(); // finds the chracter controller
        cc.SimpleMove(speed); // uses the defined direction and defined speed to move the character controller attached to player

        //Player Shooting
        if (Input.GetMouseButtonDown(0)) // if the left mouse button is clicked
        {
            var bullet = (GameObject)Instantiate(BulletPrefab, BulletStart.position, Quaternion.identity); // instatiate the bulletprefab set in IDE
            bullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletSpeed; // give it the velocity Bullet Speed defined in IDE
            Destroy(bullet, 2.0f); // destroy game object after X seconds

            //GameObject.Instantiate(digitalExplosion, transform.position, transform.rotation);
            //Potentially used when an explosion prefab is designed - Deresolution.

        }



        //Player Change Weapons

        if (Input.GetMouseButtonDown(1)) // when the right mouse button is clicked
            Debug.Log("Pressed right click");



    }

}
