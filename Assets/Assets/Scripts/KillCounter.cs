using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KillCounter : MonoBehaviour {

    public Text CounterText; // reference the UI text
    public static KillCounter instace; // a static variable for other scripts to access
    private static int kills;
    


    // Use this for initialization
    void Start () {


        instace = this; // set the static variable to this script
        kills = 0; // kills start off at zero at start of this script (not neccessary as handled by PlayerControler also)

    }
	
	// Update is called once per frame
	void Update () {

        kills = EnemyEntity.TotalKills; // sets the value of kills to be whatever value Enemy Entity has collected
        CounterText.text = "Kills: " + kills.ToString(); // sets fetched value to string

    }
}
