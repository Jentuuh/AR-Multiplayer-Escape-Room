using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

/*
* @author: jentevandersanden
* This class represents the actions of the Roomlisting prefab.
*/
public class RoomButton : MonoBehaviour
{

// Actual displayed text fields in the prefab
    [SerializeField]
    private Text room_name_text;
    [SerializeField]
    private Text room_size_text;
    [SerializeField]
    private Text level_name_text;

// Variables to save information about the room
    private string room_name;
    private string level_name;
    private int level_id;
    private int room_size;
    private int player_count;

// Makes sure that client joins the room that they clicked on 
    public void JoinRoomButtonOnClick(){

        if(PhotonNetwork.JoinRoom(room_name)){
            
            Debug.Log("Joined room: " + room_name);
            
            // Call onRoomButtonClick() from the LobbyController
            GameObject.Find("LobbyController").GetComponent<LobbyController>().onRoomButtonClick(level_id);
        }
        else{
            Debug.Log("Failed to join room: " + room_name);
        }

  
    }

// Initializes the visual UI prefab with the room's data
    public void SetRoom(string name, int levelID, string levelname, int size, int count){

        // Set all fields
        room_name = name;
        level_name = levelname;
        level_id = levelID;
        room_size = size;
        player_count = count;
        room_name_text.text = name + " :";
        level_name_text.text = levelname; 
        room_size_text.text = count + "/" + size;
    }
}
