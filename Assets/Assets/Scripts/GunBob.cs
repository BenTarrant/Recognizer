using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBob : MonoBehaviour 
{

    private float timer = 0.0f;
    public float bobbingSpeed = 0.18f; // reference for how fast camera will bob
    public float bobbingAmount = 0.2f; //reference for how far up and down the camera will bob
    public float midpoint = 2.0f; // reference for the height of camera position

    void Update()
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal"); // gets input along the horizontal axis (Left, Right)
        float vertical = Input.GetAxis("Vertical"); // gets input along the vertical axis (Up Down)

        Vector3 cSharpConversion = transform.localPosition; //converts the original Javascript function to define a Vector 3 transform in c#

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0) // if there's no inputs from the horizontal or vertical movement
        {
            timer = 0.0f; // have no bobbing
        }
        else
        {
            waveslice = Mathf.Sin(timer); //define waveslice as movement from input
            timer = timer + bobbingSpeed; // ensure the bobbing speed in used
            if (timer > Mathf.PI * 2) 
            {
                timer = timer - (Mathf.PI * 2);
            }
        }
        if (waveslice != 0) // if waveslice value does not equal 0 - meaning the player/camera is moving
        {
            float translateChange = waveslice * bobbingAmount; //provide a change in values to affect the bobbing abount
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical); // provide a defined value of the inputs
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f); // clamp the inputs
            translateChange = totalAxes * translateChange; 
            cSharpConversion.y = midpoint + translateChange;
        }
        else
        {
            cSharpConversion.y = midpoint;
        }

        transform.localPosition = cSharpConversion;
    }
}

