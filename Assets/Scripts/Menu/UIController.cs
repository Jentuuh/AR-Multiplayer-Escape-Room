using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private RoomController roomController;

    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private GameObject endScreen;

    [SerializeField]
    private TimerController timeController;

    [SerializeField]
    private List<GameObject> uiItems = new List<GameObject>();

    [SerializeField] 
    private List<GameObject> chatAndMenuButton = new List<GameObject>();

    private Text mEndTitle, mEndDiscription;

    bool game_ended = false;
    bool game_not_started = true;

    // Start is called before the first frame update
    void Start()
    {
        if (endScreen != null)
        {
            mEndTitle = endScreen.transform.Find("EndTitle").GetComponent<Text>();
            mEndDiscription = endScreen.transform.Find("EndDiscription").GetComponent<Text>();

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    /**
     * When the menu button is clicked, opens the menu and hides other ui items
     */
    public void onMenuButtonClick()
    {
        if (menu != null)
        {
            // hide ui items
            hideUI();

            //show menu
            menu.SetActive(true);

        }
        else
        {
            Debug.Log("UIController: menu was null");
        }
    }

    /**
     * Method for when the "return to game" button is clicked 
     */
    public void onReturnClick()
    {
        //hide menu
        menu.SetActive(false);

        //show other ui items again
        if(game_ended || game_not_started){
            showMenuAndChatButtons();
            return;
        }else{
            showUI();
        }

    }


    private void hideUI() {
        foreach (GameObject i in uiItems)
        {

            i.SetActive(false);

        }
    }


    private void showUI() {

        foreach (GameObject i in uiItems)
        {
            i.SetActive(true);
        }

    }

    private void showMenuAndChatButtons(){
        foreach(GameObject i in chatAndMenuButton){
            i.SetActive(true);
        }
    }

    /**
     * Method for when the "Leave game" button is clicked
     */
    public void onLeaveClick() { 

        if (roomController != null)
        {
            roomController.leaveButtonOnClick();
        }

    }

    private void onGameEnd() {
        game_ended = true;
        hideUI();
        timeController.setPaused(true);
        endScreen.SetActive(true);
    }


    public void onGameStarted(){
        game_not_started = false;
    }

    public void onEndEscape() {
        // If the game already ended, return
        if(game_ended){
            return;
        }
        onGameEnd();
        mEndTitle.text = "You' ve Escaped!";
        mEndDiscription.text = "With "+ timeController.getTime() + " minutes still on the clock"; // TODO: add time

    }

    public void onEndTime() {
         // If the game already ended, return
        if(game_ended){
            return;
        }
        onGameEnd();
        mEndTitle.text = "Time ran out!";
        mEndDiscription.text = "Woops, your time ran out, let's hope nothing bad happens..."; 

    }

    public void onEndLeave() {
         // If the game already ended, return
        if(game_ended){
            return;
        }
        onGameEnd();
        mEndTitle.text = "Someone Left!";
        mEndDiscription.text = "Someone ruined the fun for everyone and left the game..."; 
    }

    public void onEndYouLeft(){
                 // If the game already ended, return
        if(game_ended){
            return;
        }
        onGameEnd();
        mEndTitle.text = "You left the game.";
        mEndDiscription.text = "Sorry, but we cannot reconnect you to the game, return to the menu."; 

    }

}
