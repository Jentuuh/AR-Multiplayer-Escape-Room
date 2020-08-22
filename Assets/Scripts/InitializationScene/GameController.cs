using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/**
* @author jentevandersanden
* Sort of master class that controls the game flow. This class will check when the game is over, when someone leaves etc. ...
*/
public class GameController : MonoBehaviour
{

    [SerializeField]
    private UIController UI_Controller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Callback sent to all game objects when the player pauses.
    /// </summary>
    /// <param name="pauseStatus">The pause state of the application.</param>
    void OnApplicationPause(bool pauseStatus)
    {

        if(pauseStatus){
            PhotonNetwork.LeaveRoom();
        }else{
            UI_Controller.onEndYouLeft();
        }
    }

    /// <summary>
    /// Callback sent to all game objects before the application is quit.
    /// </summary>
    void OnApplicationQuit()
    {
        PhotonNetwork.LeaveRoom();
        UI_Controller.onEndYouLeft();
    }

    // This will be called on every client when someone solves the last puzzle.

    public void lastPuzzleSolved(){
        PhotonView view = PhotonView.Get(this);
        view.RPC("endGame", RpcTarget.All);
       
    }


    // Ends the game through the UI Controller.
    [PunRPC]
    public void endGame(){
        UI_Controller.onEndEscape();
    } 
}
