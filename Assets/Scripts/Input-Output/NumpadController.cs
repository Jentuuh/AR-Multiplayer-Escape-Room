using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * @author Jente Vandersanden
 */
public class NumpadController : MonoBehaviour
{

    [SerializeField]
    private Text numText;

    private string numpad_text;

    [SerializeField]
    private GameObject inputcontroller;

    [SerializeField]
    private GameObject numPadUI;

    // Start is called before the first frame update
    void Start()
    {
        numText.text = "";
        
    }

    public void onNumButtonClick(int number){
        switch(number){
            case 1:
             numpad_text += "1";
             break;

             case 2:
             numpad_text += "2";
             break;

             case 3:
             numpad_text += "3";
             break;

             case 4:
             numpad_text += "4";
             break;

             case 5:
             numpad_text += "5";
             break;

             case 6:
             numpad_text += "6";
             break;

             case 7:
             numpad_text += "7";
             break;

             case 8:
             numpad_text += "8";
             break;

             case 9:
             numpad_text += "9";
             break;
        }
        numText.text = numpad_text; 
    }


    public void onBackSpaceClick(){
        numpad_text = numpad_text.Substring(0, numpad_text.Length - 1);
        numText.text = numpad_text;
    }

    public void onConfirmClick(){
        inputcontroller.GetComponent<InputScript>().onTextInput(numpad_text);
        if(inputcontroller.GetComponent<InputScript>().getInputText() == numpad_text){
            numPadUI.SetActive(false);
        }

    }

    public void onCloseClick(){
            numPadUI.SetActive(false);
    }
}

