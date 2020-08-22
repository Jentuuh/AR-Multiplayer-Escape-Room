using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


/**
*   @author: jentevandersanden
*   This class represents the main network controller, and is responsible for the major connection functionality, like the connection to the Photon Master Servers
*/
public class NetworkController : MonoBehaviourPunCallbacks
{
    // Private member variables
    private string game_version = "1.0";        // This client's game version. Users are separated based on the version of the game.

    // Start is called before the first frame update
    void Start()
    {   
        if(!PhotonNetwork.IsConnected){
            // Connect to Master Servers (using the Photon server settings)
            PhotonNetwork.ConnectUsingSettings();
        }
    }

// Override callback function that will be called when the client gets connected to the Photon Master Servers
    public override void OnConnectedToMaster(){
        Debug.Log("This client is now connected to the " + PhotonNetwork.CloudRegion + " server!");
    }

    // Override callback function that will be called when the client gets disconnected from the Photon Master Servers
        public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogWarningFormat("The client was disconnected from the server for the following reason: {0}", cause);
    }

}
