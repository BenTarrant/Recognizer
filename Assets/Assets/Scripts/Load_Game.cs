using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Load_Game : MonoBehaviour {

    public Collider coll; // create a reference for the collider in the IDE
    public GameObject Deresoloution; // sets a reference for the game object to be instatiated in the IDE
    public AudioClip LaunchClip; // create reference for the audio clip in the IDE
    public GameObject ClickedTitle; // create a reference for the clicked version of the title in the IDE
    public MenuPlayer menuPlayer;
    public GameObject StartWeapon;

    void Start()
    {
        coll = GetComponent<Collider>(); //retrieve the collider
        GetComponent<AudioSource>().clip = LaunchClip; // retrieve the audio clip in the audiosource
        GetComponent<MenuPlayer>();

        menuPlayer.enabled = false;
        StartWeapon.gameObject.SetActive(false);

    }

    void Update()

    {
        if (Input.GetMouseButtonDown(0)) // when the left mouse button is pressed
        {
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //check the position of the raycast from that click
           RaycastHit hit; // variable of raycasting hitting something

            if (coll.Raycast(ray, out hit, 100.0F)) // if tyhe raycast hits the collider attached to this GO
            {
                Debug.Log("Load Level");
                StartCoroutine(LoadLevel()); // start the LoadLevel Coroutine
                GetComponent<AudioSource>().Play(); // and play the audio clip
            }
            

        }

    }

    IEnumerator LoadLevel()
    {
        transform.position = new Vector3(-100.55f, 2.2f, -600.7f);// Move the Unclicked title (Blue) out of shot
        ClickedTitle.SetActive(true); // Set the Clicked title (Orange) GO to active - the illusion of changing colour
        yield return new WaitForSeconds(2.5f);// wait for 2.5 seconds
        menuPlayer.enabled = true;
        StartWeapon.gameObject.SetActive(true);

    }

}

