using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPlayer : MonoBehaviour {

    public float movementSpeed = 5.0f; // public reference to movement speed
    public float mouseSensitivity = 5.0f;// public reference for mouse sensitivity

    public GameObject Weapon;


    // Use this for initialization
    void Start () {

        Weapon.gameObject.SetActive(false);

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

        void OnTriggerEnter(Collider collision) //when the colliding with a trigger collider
    {

        if (collision.gameObject.tag == "Pickup") // check to see if the item collided with is designated as a pickup
        {
            //then run through to check what type of pick up it is
            //this check means additional if statements can be added to implement new pick ups like Health and Ammo

            if (collision.gameObject.name == "Menu_Pickup_Weapon_Pistol") // if it's the Pistol pickup
            {
                Destroy(collision.gameObject); // destroy the pickup
                Weapon.gameObject.SetActive(true); // Arm the Player
                StartCoroutine(LoadLevel());
            }
        }
    }

    IEnumerator LoadLevel()
    {
        
        yield return new WaitForSeconds(2.5f);// wait for 2.5 seconds
        SceneManager.LoadScene("Level01"); // load the scene title X

    }
}
