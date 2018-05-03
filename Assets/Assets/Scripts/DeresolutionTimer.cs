using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class DeresolutionTimer : MonoBehaviour
{
    public float timeLeft = 120f;

    public Text Timertext;

    public GameObject StartBarriers;
    public GameObject FloorBarrier;
    public GameObject TeleportBarriers;


    void Update()
    {
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
