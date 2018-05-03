using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public float timeLeft = 120f;

    public Text Timertext;

    public GameObject StartBarriers;
    public GameObject FloorBarrier;
    public GameObject TeleportBarriers;

    private static GameManager instance = null;
    public static GameManager GM


    {
        get { return instance; }
    }


    void Awake()
    {

        if (instance != null && instance != this) 
        {
            Destroy(this.gameObject);
            return;

        } 
        
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    // Use this for initialization
    void Start () {

        timeLeft = 120.0f;
    }
	
	// Update is called once per frame
	void Update () {

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level01"))
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft >= 0)
            {
                Timertext.text = "Materialization:" + Mathf.Round(timeLeft);
            }

            if (Mathf.Round(timeLeft) == 90)
            {
                Destroy(StartBarriers);
            }

            if (Mathf.Round(timeLeft) == 60)
            {
                Destroy(FloorBarrier);
            }

            if (timeLeft <= 0)
            {
                Destroy(TeleportBarriers);
                Timertext.text = ("");

            }
        }

    }
}
