using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{

    public int score = 0;
    public int highScore = 0;
    string highScoreKey = "Best Time: ";
    public Text Timer;
    public Text BestTime;

    void Start()
    {
        //Get the highScore from player prefs if it is there, 0 otherwise.
        highScore = PlayerPrefs.GetInt(highScoreKey);
        BestTime.text = "Best Time: " + Mathf.Round(highScore);
    }

    void Update()
    {
        Timer.text = score.ToString();
    }

    public void OnUpdateHighScore()
    {

        //If our scoree is greter than highscore, set new higscore and save.
        if (score > highScore)
        {
            PlayerPrefs.SetInt(highScoreKey, score);
            PlayerPrefs.Save();
        }
    }

}
