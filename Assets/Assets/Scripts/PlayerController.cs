using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Player Movement and Looking
    public float movementSpeed = 5.0f; // public reference to movement speed
    public float mouseSensitivity = 5.0f;// public reference for mouse sensitivity
    float verticalRotation = 0;
    public float upDownRange = 60.0f;

    //Shooting references
    public Camera FPScamera; //Reference to the camera attached to the player for raycasting
    public float range = 100f; // range reference for how far the ray should be cast - public so different weapons can have different range
    public GameObject ImpactEffect; // reference for the impact effect particle system
    private float fl_delay; //reference for a function to implement cool-down timing
    public float fl_cool_down = 1; // reference for cool-down effect for gun firing
    public MuzzleFlash flash; // a reference for the MuzzleFlash script to play specific muzzle flashes based on weapon equipped

    //Access EnemyEntity
    private EnemyEntity Enemy; // reference to the Enemy Controller script, specified in IDE

    //Weapon Switching
    public GameObject[] weapons; // Creates an array of weapons
    public int currentWeapon; // Sets each weapon as an int to be referenced
    public bool HasWeapon2; // creates a true/false situation for playing having weapon 2 in 'inventory'

    //Health
    public Text Healthtext;
    private float Health = 2;
    public GameObject Deresolution;

    void Start()
    { 
        SetCursorState(); // Apply requested cursor state
        HasWeapon2 = false; //set so player doesn't have Weapon 2 (Rifle)
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

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

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
            range = 50.0f; // set a new range
            fl_cool_down = 0.5f; // set a new cooldown
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
                Enemy = hit.transform.gameObject.GetComponent<EnemyEntity>();
                Enemy.HitByRay();//run the function HitByRay in the EnemyController script attached to the enemy

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
    private void OnCollisionEnter(Collision collision)//void OnTriggerEnter(Collider collider) //when the colliding with a trigger collider
    {
        if (collision.transform.tag != "Ground" || collision.transform.tag != "Wall")
        {
            Debug.Log("Hit Something");

            if (collision.gameObject.tag == "Sword")
            {
                HitByEnemy(10);
            }

            if (collision.gameObject.tag == "Bullet")
            {
                HitByEnemy(1);
            }

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

    //BulletCollision
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.transform.tag != "Ground" || collision.transform.tag != "Wall")
    //    {
    //        Debug.Log("HitByBullet");
    //        HitByEnemy(); 
    //    }
    //}

    //Shot by Enemy and Death
    public void HitByEnemy(int vDamage)
    {
        Debug.Log("I was hit by an enemy");
        Health -= vDamage;
        Healthtext.text = "Health: " + Mathf.Round(Health);

        if (Health <= 0)
        {
            Instantiate(Deresolution, transform.position, transform.rotation); //instatiate the deresolution protocol at game object location
            PlayerPrefs.SetString("lastLoadedScene", SceneManager.GetActiveScene().name); //get the current scene and set it to a string (to allow correct reloading)
            SceneManager.LoadScene("RestartScreen"); // Load restart screen
        }
    }

    void OnParticleCollision(GameObject other)
    {

        SceneManager.LoadScene("CompleteScreen");

    }
}

