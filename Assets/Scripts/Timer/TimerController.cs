using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 * class that controlls the countdown timer 
 * @author Tim-Lukas Blom
 */
public class TimerController : MonoBehaviour
{
    [SerializeField]
    private int timeLeftInSec;

    [SerializeField]
    private Text timerText;

    [SerializeField]
    private GameObject timerUI;

    [SerializeField]
    private UIController controller;

    private bool isPaused;

    IEnumerator CountdownToEnd() {
        while (timeLeftInSec >= 0)
        {

            string time = "";
            time += ((int)timeLeftInSec/60).ToString();
            time += ":";
            if ((timeLeftInSec % 60) < 10)
            {
                time += "0";
            }
            time += (timeLeftInSec % 60).ToString();
            timerText.text = time;
            yield return new WaitForSeconds(1f);
            if (!isPaused)
            {
                timeLeftInSec--;
            }
        }

        timeEnded();
    
    }


    /**
     * placeholder for function that may be called at the end of the game
     */
    public void timeEnded() {
        controller.onEndTime();
    }

    public string getTime() {
        string time = "";
        time += ((int)timeLeftInSec / 60).ToString();
        time += ":";

        if ((timeLeftInSec % 60 )< 10)
        {
            time += "0";
        }
        time += (timeLeftInSec % 60).ToString();
        return time;
    }

    public void setTime(int minutes) {
        timeLeftInSec = minutes * 60; 
    
    }

    public void startTimer(){
        timerUI.SetActive(true);
        StartCoroutine(CountdownToEnd());
    }

    public void setPaused(bool ispaused) {
        isPaused = ispaused;
    }
}
