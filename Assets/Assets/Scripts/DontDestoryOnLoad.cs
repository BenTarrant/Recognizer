using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryOnLoad : MonoBehaviour {

    private static bool created = false; // static check to see if singleton is already created

    void Awake()
    {
        if (!created) //if there isn't already a singleton
        {
            DontDestroyOnLoad(this.gameObject); // don't destory THIS singleton on load
            created = true; //set the created bool to true to disable future singleton creation
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown (KeyCode.Escape)) // if the esc button is pressed
        {
            SetCursorState(); // run cursor state method
        }
    }

    void SetCursorState()
    {
      Cursor.lockState = CursorLockMode.None; // releases the cursor
      Cursor.visible = true; //makes the cursor visible
    }
}
