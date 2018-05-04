﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timePassed = 0f; // reference for the amount of time left

    public Text Timertext; // reference the UI text

    public GameObject StartBarriers; // reference the starting barriers
    public GameObject FloorBarrier; // reference the barrier to the second floor
    public GameObject TeleportBarriers; // reference the barriers around the teleport


    void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level01")) // get the active scene and check if it's LevelO1
        {
            timePassed += Time.deltaTime; //time left minus delta time

            if (timePassed >= 0) // if time left is greater or equal to 0
            {
                Timertext.text = "Survived For: " + Mathf.Round(timePassed); // display the time left in referenced UI text
            }

            if (Mathf.Round(timePassed) == 30) // when survived for 30 seconds
            {
                Destroy(StartBarriers); // destory the starting barriers
            }

            if (Mathf.Round(timePassed) == 100) // when survived for 100 seconds
            {
                Destroy(FloorBarrier); // destory barriers to second floor
            }

            if (timePassed == 150) //when survived for 150 seconds
            {
                Destroy(TeleportBarriers); // destory the teleport barriers

            }
        }
        
    }
}
