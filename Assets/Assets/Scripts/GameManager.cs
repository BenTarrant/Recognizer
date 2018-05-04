using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    private static GameManager instance = null;
    public static GameManager GM // enables other scripts to access singleton if necessary


    {
        get { return instance; }
    }


    void Awake()
    {

        if (instance != null && instance != this) //if this is a duplicate GM
        {
            Destroy(this.gameObject); //destory the duplicate
            return; // return to the start of function

        } 
        
        else
        {
            instance = this; // set this to the instance of the singleton
        }

        DontDestroyOnLoad(this.gameObject); // don't destory this between scenes
    }
}
