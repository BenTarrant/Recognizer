using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KillCounter : MonoBehaviour {

    public Text CounterText; // reference the UI text
    public static KillCounter instace; // a static variable for other scripts to access
    private static int kills;
    //blic float totalKills = 0f; // reference for total number of kills


    // Use this for initialization
    void Start () {


        instace = this; // set the static variable to this script
        kills = 0;

    }
	
	// Update is called once per frame
	void Update () {

        kills = EnemyEntity.TotalKills;
        CounterText.text = "Kills: " + kills.ToString();

    }
}
