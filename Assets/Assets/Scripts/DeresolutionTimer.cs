using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class DeresolutionTimer : MonoBehaviour
{
    public float timeLeft = 120f;
    public AudioSource audioSource;
    public AudioClip CombatantReleased;
    public AudioClip FloorReleased;
    public AudioClip TeleportMaterialised;
    public Text Timertext;
    public bool RoboticSoundPlayed;
    public bool RoboticSoundPlayed2;
    public bool RoboticSoundPlayed3;



    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft >= 0)
        {
            Timertext.text = "Materialization:" + Mathf.Round(timeLeft);
        }
        if (Mathf.Round(timeLeft) == 90)
        {
            Debug.Log("Released");
            if (RoboticSoundPlayed == false)
            {
                audioSource.PlayOneShot(CombatantReleased);
                RoboticSoundPlayed = true;
            }
        }
        if (Mathf.Round(timeLeft) == 60)
        {
            if (RoboticSoundPlayed2 == false)
            {
                audioSource.PlayOneShot(FloorReleased);
                RoboticSoundPlayed2 = true;
            }

        }
        if (timeLeft <= 0)
        {
            if (RoboticSoundPlayed3 == false)
            {
                audioSource.PlayOneShot(TeleportMaterialised);
                RoboticSoundPlayed3 = true;
                Timertext.text = ("");
            }
        }
        
    }
}
