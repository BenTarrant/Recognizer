using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HighScorer : MonoBehaviour {

    public Text Finalscoretext; // reference the UI text


    void Start ()
    {
        Finalscoretext.text = "Your Score: " + (Timer.instace.score); // set the final score text to be the timer text highScore
    }
	

}
