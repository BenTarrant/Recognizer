using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    public Collider collide; // create a reference for the collider in the IDE
    public AudioClip ReLaunchClip; // create reference for the audio clip in the IDE
    public GameObject ReplayClickedTitle; // create a reference for the clicked version of the title in the IDE

    void Start()
    {
        collide = GetComponent<Collider>(); //retrieve the collider
        GetComponent<AudioSource>().clip = ReLaunchClip; // retrieve the audio clip in the audiosource
        SetCursorState(); // run cursor state method


    }

    void SetCursorState() // cursor state method
    {
        Cursor.lockState = CursorLockMode.None; // releases cursor
    }

    void Update()

    {
        if (Input.GetMouseButtonDown(0)) // when the left mouse button is pressed
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //check the position of the raycast from that click
            RaycastHit hit; // variable of raycasting hitting something

            if (collide.Raycast(ray, out hit, 100.0F)) // if tyhe raycast hits the collider attached to this GO
            {
                StartCoroutine(ReLoadLevel()); // start the LoadLevel Coroutine
                GetComponent<AudioSource>().Play(); // and play the audio clip
            }


        }

    }

    IEnumerator ReLoadLevel()
    {
        transform.position = new Vector3(-100.55f, 2.2f, -600.7f);// Move the Unclicked title (Blue) out of shot
        ReplayClickedTitle.SetActive(true); // Set the Clicked title (Orange) GO to active - the illusion of changing colour
        yield return new WaitForSeconds(2.5f);// wait for 2.5 seconds
        SceneManager.LoadScene("MenuScreen"); // load menu screen (to restart game sequence afresh)

    }
}
