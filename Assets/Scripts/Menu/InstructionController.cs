using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionController : MonoBehaviour
{

    [SerializeField]
    private GameObject instructionBar;

    private Text mInstructionText;

    private int timeVisible;

    // Start is called before the first frame update
    void Start()
    {
        if (instructionBar != null)
        {
            mInstructionText = instructionBar.GetComponentInChildren<Text>();
        }
        else {
            Debug.Log("InstructionBarController: Instructionbar not set");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * Shows instructionbar
     * @param instructionMessage text that' ll be displayed in the bar
     * @param duration duration for wich the bar will be seen, insert 0 to show it until manually turned of
     */
    public void setInstructionBar(string instructionMessage, int duration)
    {
        if (duration != 0)
        {
            mInstructionText.text = instructionMessage;
            setBarVisible(true);
            timeVisible = duration;
            StartCoroutine(visibleFor());



        }
        else {
            mInstructionText.text = instructionMessage;
            setBarVisible(true);
        }

    }

    private IEnumerator visibleFor() {
        while (timeVisible >= 0)
        {

   
            yield return new WaitForSeconds(1f);
            timeVisible--;
        }

        setBarVisible(false);
    }

    public void setBarVisible(bool isVisible) {
        instructionBar.SetActive(isVisible);
    }



}
