using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class DeresolutionTimer : MonoBehaviour
{
    public float timeLeft = 120f; // reference for the amount of time left

    public Text Timertext; // reference the UI text

    public GameObject StartBarriers; // reference the starting barriers
    public GameObject FloorBarrier; // reference the barrier to the second floor
    public GameObject TeleportBarriers; // reference the barriers around the teleport


    void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level01")) // get the active scene and check if it's LevelO1
        {
            timeLeft -= Time.deltaTime; //time left minus delta time

            if (timeLeft >= 0) // if time left is greater or equal to 0
            {
                Timertext.text = "Materialization:" + Mathf.Round(timeLeft); // display the time left in referenced UI text
            }

            if (Mathf.Round(timeLeft) == 90) // when time left hits 90
            {
                Destroy(StartBarriers); // destory the starting barriers
            }

            if (Mathf.Round(timeLeft) == 60) // when time left hits 60
            {
                Destroy(FloorBarrier); // destory barriers to second floor
            }

            if (timeLeft <= 0) // when time left goes below 0
            {
                Destroy(TeleportBarriers); // destory the teleport barriers
                Timertext.text = (""); // stop displaying the UI

            }
        }
        
    }
}
