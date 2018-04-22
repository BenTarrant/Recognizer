using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float movementSpeed = 5.0f; // public reference to movement speed
    public float mouseSensitivity = 5.0f;// public reference for mouse sensitivity

    //Shooting references
    public Camera FPScamera; //Reference to the camera attached to the player for raycasting
    public float range = 100f; // range reference for how far the ray should be cast - public so different weapons can have different range
    public GameObject ImpactEffect; // reference for the impact effect particle system
    private float fl_delay;
    public float fl_cool_down = 1;
    //public MuzzleFlash flash;

    //Access EnemyController
    public EnemyController Enemy; // reference to the Enemy Controller script, specified in IDE

    //Weapon Switching
    public GameObject[] weapons;
    public int currentWeapon;

    void Start()
    {
        // Apply requested cursor state
        SetCursorState();
        changeWeapon(1);
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
        if (Input.GetMouseButtonDown(0) && Time.time > fl_delay) // if the left mouse button is clicked
        {

           Shoot(); // run shoot method
           fl_delay = Time.time + fl_cool_down;

            //CURRENTLY REQUIRES DIFFERENT MUZZLE FLASH AND COOLDOWNS FOR EACH WEAPON - NEED TO CALL FLASH() FUNCTION IN MUZZLEFLASH SCRIPT SOMEHOW

        }



        //Player Change Weapons

        if (Input.GetKeyDown(KeyCode.Alpha1)) // when the right mouse button is clicked
        {
            changeWeapon(1);
            range = 30.0f;
            fl_cool_down = 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // when the right mouse button is clicked
        {
            changeWeapon(2);
            range = 50.0f;
            fl_cool_down = 0.1f;
        }




    }

    void Shoot() // shoot method
    {
        RaycastHit hit;//fire a raycast
        if (Physics.Raycast(FPScamera.transform.position, FPScamera.transform.forward, out hit, range))//if the raycast hits something within range
        {
            Debug.Log(hit.transform.name); // print the name of collider the ray has hit
            Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal)); // instantiate attached impact particle effect with correct rotation based on normals of collider

            if (hit.transform.gameObject.tag == "Enemy") // if the raycast hits an object tagged enemy
            {
                Enemy.HitByRay();//run the function HitByRay in the EnemyController script attached to the enemy CURRENTLY ONLY WORKS WITH ONE ENEMY DUE TO ATTACH METHOD
            }
        }

    }

    public void changeWeapon(int num)
    {
        currentWeapon = num;
        for (int i = 1; i < weapons.Length; i++)
        {
            if (i == num)
                weapons[i].gameObject.SetActive(true);
            else
                weapons[i].gameObject.SetActive(false);
        }


    }

    //pickup functionality
    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "Pickup") // check to see if the item collided with is designated as a pickup
        {
        //then ren through to check what type of pick up it is

            if (collision.gameObject.name == "Pickup_Weapon_Rifle") // if it's the Rifle pickup
            {
                Destroy(collision.gameObject); // destroy the pickup
                changeWeapon(2); // switch to the Rifle weapon (2 in array)
            }

            if (collision.gameObject.name == "Pickup_Weapon_Pistol") // if it's the Pistol pickup
            {
                Destroy(collision.gameObject); // destroy the pickup
                changeWeapon(1); // Switch to the Pistol 
            }
        }
    }
}
