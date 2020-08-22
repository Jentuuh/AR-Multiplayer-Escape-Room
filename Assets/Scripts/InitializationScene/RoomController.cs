using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
* @author jentevandersanden, jorisbertram
* This class controls the GUI for the waiting room that a player ends up in after joining 
* a game and initializing their room.
*/
public class RoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text roomname_text;

    [SerializeField]
    private GameObject start_button;

    [SerializeField]
    private GameObject inventory_HUD;

    [SerializeField]
    private GameObject timer_controller;

    [SerializeField]
    private GameObject UI_controller;  

    [SerializeField]
    private GameObject Tutorial_controller;  
    private string roomname;

    private int level_id;

    private byte playercount;

    private byte maxplayers;

    private bool is_host;

    private bool game_started;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        playercount = PhotonNetwork.CurrentRoom.PlayerCount;
        maxplayers = PhotonNetwork.CurrentRoom.MaxPlayers;
        roomname = PhotonNetwork.CurrentRoom.Name;
        is_host = PhotonNetwork.IsMasterClient;
        level_id = (int)PhotonNetwork.CurrentRoom.CustomProperties["levelID"];
        Debug.Log(level_id.ToString());
        
        game_started = false;

        roomSetupGUI();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        roomSetupGUI();
    }

    // Initializes GUI for Room Setup
    private void roomSetupGUI() {
        roomname_text.text = roomname;

        // If this player is the master client of the room and the room is full
        if(is_host && playercount.ToString() == maxplayers.ToString() && maxplayers.ToString() == allPlayersInitialized().ToString()){
            // Display the button to start the game
            start_button.SetActive(true);
        }
    }

    // Handles what happens when the user clicks the 'leave' button
    public void leaveButtonOnClick() {
        Debug.Log("Leaving Game...");
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
    }

    public void startButtonOnClick(){
        
        // Check if we're not playing the tutorial
        if(level_id != 1){
            // Send an RPC to everyone that the game started.
            PhotonView p_view = PhotonView.Get(this);
            p_view.RPC("gameStarted", RpcTarget.All);
        }
        else{
            startTutorial();
        }
    }

    [PunRPC]
    void gameStarted(){
        // TODO : Implement that game starts (waiting room UI disappears, Inventory appears, Chat appears, timer starts...)
        game_started = true;
        // Notify UI Controller that the game started
        UI_controller.GetComponent<UIController>().onGameStarted();
        
        inventory_HUD.SetActive(true);
        GameObject.Find("ContainerPlacementController").GetComponent<ContainerPlacementController>().Invoke("disableWaitingRoomUI", 0.5f);
        GameObject.Find("ContainerPlacementController").GetComponent<ContainerPlacementController>().Invoke("placeContainers", 0.5f);
        timer_controller.GetComponent<TimerController>().startTimer();
    }


    // CALLBACK FUNCTIONS
    public override void OnJoinedRoom() {
        roomname = PhotonNetwork.CurrentRoom.Name;
        Debug.Log("Joined Room with Roomname: " + roomname);
        
        Debug.Log("Initializing room...");
        roomSetupGUI();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        playercount = PhotonNetwork.CurrentRoom.PlayerCount;
        
        Debug.Log(newPlayer.NickName + " has entered the room.");
    }


    public override void OnPlayerLeftRoom(Player newPlayer){
        decrementInitialized();
        playercount = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log(newPlayer.NickName + " has left the room.");

        // We know linking the UI Controller to the Room Controller as a member value increases the coupling a lot, but at the moment we see no other 
        // way to reach this function cleanly than doing it this way.
        UI_controller.GetComponent<UIController>().onEndLeave();


        // Remove the start button
        if(is_host){
            start_button.SetActive(false);
        }
    }

    public override void OnCreatedRoom(){
        Debug.Log("Created room '" + PhotonNetwork.CurrentRoom.Name + "'");
    }

    private int allPlayersInitialized(){
        int value = (int)PhotonNetwork.CurrentRoom.CustomProperties["initialized"];
        return value;
    }

    // Function that's called when the level ID is 1, which means we're playing the tutorial
    private void startTutorial(){
        game_started = true;
        // Hide waiting room UI
        GameObject.Find("ContainerPlacementController").GetComponent<ContainerPlacementController>().Invoke("disableWaitingRoomUI", 0.1f);
        // Show the first instruction in the tutorial
        Tutorial_controller.GetComponent<TutorialController>().showFirstInstruction();
        timer_controller.GetComponent<TimerController>().startTimer();
    }



    // Decrements the amount of initialized players in a room.
    private void decrementInitialized(){
        int value = (int)PhotonNetwork.CurrentRoom.CustomProperties["initialized"];
        int newValue  = value  - 1;

        ExitGames.Client.Photon.Hashtable setValue = new ExitGames.Client.Photon.Hashtable();
        setValue.Add("initialized", newValue);

        ExitGames.Client.Photon.Hashtable expectedValue = new ExitGames.Client.Photon.Hashtable();
        expectedValue.Add("initialized", value);

        PhotonNetwork.CurrentRoom.SetCustomProperties(setValue, expectedValue);
    }   
}
