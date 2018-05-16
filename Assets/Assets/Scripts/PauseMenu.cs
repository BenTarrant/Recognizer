using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // if the esc button is pressed
        {
            if (GameIsPaused)
            {
                Resume();
                LockCursorState(); // run cursor state method
            }
            else
            {
                Pause();
                UnlockCursorState(); // run cursor state method
            }

        }
    }



    void UnlockCursorState()
    {
        Cursor.lockState = CursorLockMode.None; // releases the cursor
        Cursor.visible = true; //makes the cursor visible
    }

    void LockCursorState()
    {
        Cursor.lockState = CursorLockMode.Locked; // releases the cursor
        Cursor.visible = false; //makes the cursor visible
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        LockCursorState();
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        LockCursorState();
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        DontDestoryOnLoad.created = true;
        SceneManager.LoadScene("MenuScreen");
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame");
        Application.Quit();
    }
}
