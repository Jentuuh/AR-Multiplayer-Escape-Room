using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteScript : MonoBehaviour
{
    [SerializeField]
    private GameObject noteImage;

    [SerializeField]
    private GameObject hideNoteButton;

    
    // Start is called before the first frame update
    void Start()
    {
        noteImage.SetActive(false);
    }

    public void showNoteImage()
    {
        noteImage.SetActive(true);
        hideNoteButton.SetActive(true);
    }

    public void hideNoteImage()
    {
        noteImage.SetActive(false);
        hideNoteButton.SetActive(false);
    }
}
