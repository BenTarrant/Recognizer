using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryOnLoad : MonoBehaviour {

    public static bool created = false; // static check to see if singleton is already created


    void Awake()
    {
        if (!created) //if there isn't already a singleton
        {
            DontDestroyOnLoad(this.gameObject); // don't destory THIS singleton on load
            created = true; //set the created bool to true to disable future singleton creation
        }

        else
        {
            Destroy(this.gameObject);
        }
    }
}
