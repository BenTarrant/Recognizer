using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //audio
    public AudioSource Sourceaudio; // reference for audio source in IDE


    //Player Movement and Looking
    public float movementSpeed = 5.0f; // public reference to movement speed
    public float mouseSensitivity = 4.0f;// public reference for mouse sensitivity
    public float mouseVertSensitivityRed = 1.2f; // public reference for vertical mouse sensitivity
    float verticalRotation = 0;
    public float upDownRange = 60.0f; // these are used to define the range the camera can be moved with the mouse on Y axis
    public Text Sensitivitytext;

    //Shooting references
    public Camera FPScamera; //Reference to the camera attached to the player for raycasting
    public float range = 100f; // range reference for how far the ray should be cast - public so different weapons can have different range
    public GameObject ImpactEffect; // reference for the impact effect particle system
    private float fl_delay; //reference for a function to implement cool-down timing
    public float fl_cool_down = 1; // reference for cool-down effect for gun firing
    public MuzzleFlash flash; // a reference for the MuzzleFlash script to play specific muzzle flashes based on weapon equipped
    public AudioClip Shot; // audio clip reference for gunshot
    public AudioClip EnemyHit; // audio clip reference for enemies being hit

    //pickup
    public Object[] Pickup; //Specify an object arrray for pickups

    //Ammo
    private float RifleAmmo; // float for rifle ammo
    public Text AmmoText; // reference for the Ui displaying ammo

    //Access other scripts
    private EnemyEntity Enemy; // reference to the Enemy Controller script, specified in IDE
    //private Timer setScore;

    //Weapon Switching
    public GameObject[] weapons; // Creates an array of weapons
    public int currentWeapon; // Sets each weapon as an int to be referenced
    public bool HasWeapon2; // creates a true/false situation for playing having weapon 2 in 'inventory'
    public AudioClip WeaponSwitch; // audio for weapon switching
    public AudioClip WeaponPickup; // audio for weapon pick up

    //Health
    public Text Healthtext; // reference for health UI
    public float Health = 15; // reference and sets the player's initial health
    public AudioClip HealthPickup; // audio for picking up health
    public AudioClip PlayerHit; // audio clip reference for player getting shot
    public GameObject Splatter;


    void Start()
    {
        PauseMenu.GameIsPaused = false;
        Pickup = null;
        SetCursorState(); // Apply requested cursor state
        HasWeapon2 = false; //set so player doesn't have Weapon 2 (Rifle)
        EnemyEntity.TotalKills = 0; // Resets the kill counter to zero
        Sensitivitytext.text = "Sensitivity: " + Mathf.Round(mouseSensitivity);
        Healthtext.text = "Health: " + Mathf.Round(Health); // sets the initial starting heath from private float above and displays it in the referenced text component
        GetComponent<AudioSource>(); // get the audio source compnent to allow audio to be played
        Pickup = Resources.LoadAll("", typeof(GameObject)); // gather the prefabs in the resources folder and load them as gameobjects to be later used
    }

    void SetCursorState()
    {
        Cursor.lockState = CursorLockMode.Locked; // Stops cursor moving during play
        Cursor.visible = false; // sets the cursore to invisible in build
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused == false) // if the game isn't paused, the player can move
        {
            //Camera Roatation
            if (Input.GetAxis("XboxX") >0.1f || Input.GetAxis("XboxX") <-0.1f)
            {
                float rotLeftRight = Input.GetAxis("XboxX") * mouseSensitivity; //set the mouse sensitivity on the X axis and rotate camera
                transform.Rotate(0, rotLeftRight, 0);
            }

            else if (Input.GetAxis("Mouse X") >0.1f || Input.GetAxis("Mouse X") <-0.1f)
            {
                float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity; //set the mouse sensitivity on the X axis and rotate camera
                transform.Rotate(0, rotLeftRight, 0);
            }          

            // // set the mouse sensitivity on the Y axis and rotate camera
            if (Input.GetAxis("XboxY") > 0.1f || Input.GetAxis("XboxY") < -0.1f)
                verticalRotation -= Input.GetAxis("XboxY") * (mouseSensitivity / mouseVertSensitivityRed);
            else if (Input.GetAxis("Mouse Y") > 0.1f || Input.GetAxis("Mouse Y") < -0.1f)
                verticalRotation -= Input.GetAxis("Mouse Y") * (mouseSensitivity / mouseVertSensitivityRed);


            verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange); // clamp the rotation along the Y based on definied range
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
        }


        //Mouse/Look sensitivity modifiers

        if  ((Input.GetButtonDown("Plus") || Input.GetButtonDown("XboxRB")))
        {
            mouseSensitivity += 1;
            Sensitivitytext.text = "Sensitivity: " + Mathf.Round(mouseSensitivity);
            
            
        }

        if ((Input.GetButtonDown("Minus") || Input.GetButtonDown("XboxLB")))
        {
            mouseSensitivity -= 1;
            Sensitivitytext.text = "Sensitivity: " + Mathf.Round(mouseSensitivity);
            
            
        }

        //Player Shooting
        if ( (Input.GetButton("Shoot") || Input.GetAxis("XboxR2") <-0.1f)  && Time.time > fl_delay) // if the left mouse button is held down or clicked and the cooldown has passed
        {
            //using button rather than button down allows fully automatic firing of the rifle which is far more appealing for players

            Shoot(); // run shoot method
            fl_delay = Time.time + fl_cool_down; //specifiy cooldown

            if (weapons[2].gameObject.activeSelf == true) // if weapon 2 is active (equipped)
            {
                RifleAmmo = RifleAmmo - 1; //rifle loses one ammo per shot
                AmmoText.text = "Ammo: " + Mathf.Round(RifleAmmo); //update the Ammo float in UI
                flash.Flash2(); // run weapon 2's muzzle flash

                if (RifleAmmo <= 0)
                {
                    weapons[2].gameObject.SetActive(false); // unequip the rifle
                    HasWeapon2 = false; //switch the rifle boolean to false
                    AmmoText.text = (""); //turn off the UI for ammo
                    weapons[1].gameObject.SetActive(true); //set the pistol to equipped
                    Sourceaudio.clip = WeaponSwitch; //define the relevant clip
                    Sourceaudio.Play(); // play the relevant audio clip
                    fl_cool_down = 0.2f; //set the cooldown
                }
            }

            else //otherwise
            {
                flash.Flash1(); //run weapon 1's muzzle flash

                // This works for two weapons due to the ability to use true/false checks, however it would need to be revisted 
                //if more than two weapons were to be introduced.
            }
        }

        //Player Change Weapons

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetButtonDown("Fire2")) // When the number 1 key above the letter keys is pressed
        {
            AmmoText.text = (""); //update the UI to not be visible (pistol has infinite ammo, no use confusing the player)
            changeWeapon(1); // change to Weapon 1
            range = 50.0f; // set a new range
            fl_cool_down = 0.2f; // set a new cooldown
            Sourceaudio.clip = WeaponSwitch; //define the relevant audio clip
            Sourceaudio.Play(); // play the relevant audio clip
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetButton("Fire3")) // When the number 2 key above the letter keys is pressed
        {
            if (HasWeapon2 == true) // check to see if Player has picked up Weapon 2 (Rifle)
            {
                AmmoText.text = "Ammo: " + Mathf.Round(RifleAmmo); //update the UI
                changeWeapon(2); // change to Weapon 2
                Sourceaudio.clip = WeaponSwitch; //define the relevant audio clip
                Sourceaudio.Play(); // play the relevant audio clip
                range = 50.0f; // Set a new range
                fl_cool_down = 0.1f; // set a new cooldown
            }
        }
    }

    void Shoot() // shoot method
    {
        Sourceaudio.clip = Shot; //define the relevant audio clip
        Sourceaudio.Play(); // play the relevant audio clip

        RaycastHit hit;//fire a raycast
        if (Physics.Raycast(FPScamera.transform.position, FPScamera.transform.forward, out hit, range))//if the raycast hits something within range
        {
            Instantiate(ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal)); // instantiate attached impact particle effect with correct rotation based on normals of collider

            if (hit.transform.gameObject.tag == "Enemy") // if the raycast hits an object tagged enemy
            {
                Enemy = hit.transform.gameObject.GetComponent<EnemyEntity>(); //run the hitbyray function within the enemy base class with the below modifiers


                if (weapons[2].gameObject.activeSelf == true) // if the rifle is equipped
                {

                    Enemy.HitByRay(2);//run the function HitByRay in the script attached to the enemy dealing 2 damage
                    Sourceaudio.clip = EnemyHit; //define the relevant audio clip
                    Sourceaudio.Play(); // play the relevant audio clip
                }

                else
                {

                    Enemy.HitByRay(1); //run the function HitByRay in the script attached to the enemy dealing 1 damage
                    Sourceaudio.clip = EnemyHit; //define the relevant audio clip
                    Sourceaudio.Play(); // play the relevant audio clip
                }
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

            if (collision.gameObject.tag == "Sword")
            {
                HitByEnemy(10); // sword hits = 10 damage to player
            }

            if (collision.gameObject.tag == "Bullet")
            {
                HitByEnemy(1); // bullet hits = 1 damage to player
            }

            if (collision.gameObject.tag == "Pickup") // check to see if the item collided with is designated as a pickup
            {
                //then run through to check what type of pick up it is
                //this check means additional if statements can be added to implement new pick ups

                if (collision.gameObject.name.Contains(Pickup[1].name)) // if it's the Rifle pickup in the array
                {
                    RifleAmmo = RifleAmmo + 30; //add ammo to rifle
                    AmmoText.text = "Ammo: " + Mathf.Round(RifleAmmo); //update ammo text to reflect the add
                    Destroy(collision.gameObject); // destroy the pickup
                    HasWeapon2 = true; // set the rifle boolean to true
                    range = 50.0f; // Set a new range
                    fl_cool_down = 0.1f; // set a new cooldown
                    Sourceaudio.clip = WeaponPickup; //define relevant audio clip
                    Sourceaudio.Play(); //play relevant audio clip
                    changeWeapon(2); // switch to the Rifle weapon (2 in array)

                }

                if (collision.gameObject.name.Contains(Pickup[0].name)) // if its the health pickup
                {
                    Destroy(collision.gameObject); // destroy the pick up
                    Sourceaudio.clip = HealthPickup; //define relevant audio clip
                    Sourceaudio.Play(); //play relevant audio clip
                    Health = Health + 5; // add 5 to health float
                    Healthtext.text = "Health: " + Mathf.Round(Health); // update UI to reflect this
                }
            }
        }
    }


    //Shot by Enemy and Death
    public void HitByEnemy(int vDamage) //hit by enemy function with damage variable
    {
        Health -= vDamage; // minus the damange variable from the health float whenever this method is called
        Healthtext.text = "Health: " + Mathf.Round(Health); // update UI to reflect this
        Sourceaudio.clip = PlayerHit; //define relevant audio clip
        Sourceaudio.Play(); //play relevant audio clip
        StartCoroutine(SplatterEffect());
        //

        if (Health <= 0) // death
        {
            
            PlayerPrefs.SetString("lastLoadedScene", SceneManager.GetActiveScene().name); //get the current scene and set it to a string (to allow correct reloading)
            SceneManager.LoadScene("RestartScreen"); // Load restart screen
        }
    }

    void OnParticleCollision(GameObject other) // when player collides with a particle system (the teleport)
    {
        Timer.instace.UpdateHighScore(); //only set the high score when the player enters the teleport

        //while this finishes the game currently, it can easily be expanded to allow for multiple levels accessed by reaching the teleport
    }

    IEnumerator SplatterEffect()
    {
        Splatter.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Splatter.gameObject.SetActive(false);
    }
}

