using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Player Movement and Looking
    public float movementSpeed = 5.0f; // public reference to movement speed
    public float mouseSensitivity = 5.0f;// public reference for mouse sensitivity

    //Shooting references
    public Camera FPScamera; //Reference to the camera attached to the player for raycasting
    public float range = 100f; // range reference for how far the ray should be cast - public so different weapons can have different range
    public GameObject ImpactEffect; // reference for the impact effect particle system
    private float fl_delay; //reference for a function to implement cool-down timing
    public float fl_cool_down = 1; // reference for cool-down effect for gun firing
    public MuzzleFlash flash; // a reference for the MuzzleFlash script to play specific muzzle flashes based on weapon equipped

    //Access EnemyController
    public EnemyController Enemy; // reference to the Enemy Controller script, specified in IDE

    //Weapon Switching
    public GameObject[] weapons; // Creates an array of weapons
    public int currentWeapon; // Sets each weapon as an int to be referenced
    public bool HasWeapon2; // creates a true/false situation for playing having weapon 2 in 'inventory'

    void Start()
    {
        SetCursorState(); // Apply requested cursor state
        HasWeapon2 = false; //set so player doesn't have Weapon 2 (Rifle)
        //changeWeapon(1);
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
        if (Input.GetMouseButtonDown(0) && Time.time > fl_delay) // if the left mouse button is clicked and the cooldown has passed
        {

           Shoot(); // run shoot method
           fl_delay = Time.time + fl_cool_down; //specifiy cooldown

            if (weapons[2].gameObject.activeSelf == true) // if weapon 2 is active (equipped)
            {
                flash.Flash2(); // run weapon 2's muzzle flash
            }

            else //otherwise
            {
                flash.Flash1(); //run weapon 1's muzzle flash

                // This works for two weapons due to the ability to use true/false checks, however it would need to be revisted if more than two weapons wanted to be introduced.
            }

        }



        //Player Change Weapons

        if (Input.GetKeyDown(KeyCode.Alpha1)) // When the number 1 key above the letter keys is pressed
        {
            changeWeapon(1); // change to Weapon 1
            range = 30.0f; // set a new range
            fl_cool_down = 1.0f; // set a new cooldown
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) // When the number 2 key above the letter keys is pressed
        {
            if (HasWeapon2 == true) // check to see if Player has picked up Weapon 2 (Rifle)
            {
                changeWeapon(2); // change to Weapon 2
                range = 50.0f; // Set a new range
                fl_cool_down = 0.1f; // set a new cooldown
            }
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

    public void changeWeapon(int num) // function to change weapon
    {
        currentWeapon = num; // set current weapon using an int
        for (int i = 1; i < weapons.Length; i++)
        {
            if (i == num) //if the int is the currently active weapon
                weapons[i].gameObject.SetActive(true); //set that weapon game object to true
            else
                weapons[i].gameObject.SetActive(false); // else, set it to false - an 'on off' scenario to determine which weapon is equipped. Can be expanded as far as the array is expanded in IDE
            // reference this process with changeWeapon(*desired weapon number*);
        }


    }

    //pickup functionality
    void OnTriggerEnter(Collider collision) //when the colliding with a trigger collider
    {

        if (collision.gameObject.tag == "Pickup") // check to see if the item collided with is designated as a pickup
        {
        //then run through to check what type of pick up it is
        //this check means additional if statements can be added to implement new pick ups like Health and Ammo

            if (collision.gameObject.name == "Pickup_Weapon_Rifle") // if it's the Rifle pickup
            {
                Destroy(collision.gameObject); // destroy the pickup
                HasWeapon2 = true;
                range = 50.0f; // Set a new range
                fl_cool_down = 0.1f; // set a new cooldown
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
