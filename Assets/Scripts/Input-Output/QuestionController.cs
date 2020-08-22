using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/**
 * @author Joris Bertram
 */
public class QuestionController : MonoBehaviour
{
    [SerializeField]
    private Canvas questionCanvas;

    [SerializeField]
    private Text questionText;

    [SerializeField]
    private Text answerFieldPlaceHolder;

    [SerializeField]
    private Text answerFieldText;

    [SerializeField]
    private InputScript input;

    [SerializeField]
    private string question;

    [SerializeField]
    private string placeholder;

    // Answer must be put in InputPrefab

    void Start()
    {
        answerFieldPlaceHolder.text = placeholder;
        questionText.text = question;
    }

    public void onConfirmClick() {
        input.onTextInput(answerFieldText.text);

        if (input.getInputText() == answerFieldText.text)
            questionCanvas.gameObject.SetActive(false);
    }

    public void onCloseClick() {
        questionCanvas.gameObject.SetActive(false);
    }
}
