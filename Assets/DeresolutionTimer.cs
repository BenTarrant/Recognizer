using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class DeresolutionTimer : MonoBehaviour
{
    public float timeLeft = 300.0f;

    public Text Timertext;



    void Update()
    {
        timeLeft -= Time.deltaTime;
        Timertext.text = "Deresolution In:" + Mathf.Round(timeLeft);
        if (timeLeft < 0)
        {
            SceneManager.LoadScene("RestartScreen");
        }
    }
}
