using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;



/**
*   @author: jentevandersanden
*   This class represents the lobby controller, which handles the functionality in gathering rooms, 
*   listing them up and giving the client the option to join these rooms.
*/
public class LobbyController : MonoBehaviourPunCallbacks
{

// Private member variables
    [SerializeField]
    private GameObject lobby_joinButton;     // Button to join a lobby (and later eventually a room)
    [SerializeField]
    private GameObject lobby_createButton;  // Button to create a game
    [SerializeField]
    private GameObject lobby_panel;         // UI Panel that represents the lobby
    [SerializeField]
    private GameObject main_panel;          // UI panel that represents the main menu

    [SerializeField]
    private GameObject create_panel;       // UI panel that represents the create room menu

    [SerializeField]
    private GameObject level_selector;     // UI panel that represents the level selector

    [SerializeField]
    private GameObject character_select;    // UI panel that represents the character selector
    
    [SerializeField]
    private GameObject loading_button1;
    [SerializeField]
    private GameObject loading_button2;

    [SerializeField]
    private InputField nickname_textfield;                          // UI inputfield so the player can insert their nickname

    [SerializeField]
    private InputField roomname_textfield;                          // UI inputfield so the player can insert their roomname

    private string room_name;                                       // Variable in which the room name will be saved
    
    private Dictionary<string, RoomInfo> room_list_cache;           // Internally cached list of available rooms

    [SerializeField]
    private Transform room_container;                               // UI scroll transform which acts like a container for the room list

    [SerializeField]
    private GameObject room_list_object_prefab;                     // Prefab to represent a room in the list UI

    private Dictionary<string,GameObject> room_button_entries;      // Dictionary that keeps track of all the UI room listings (so we can reset them)

    [SerializeField]
    private GameObject button_prefab;                               // Prefab that represents a button, used in the dynamical instantiation of the character selector

    [SerializeField]
    private Transform character_container;                          // Container within the character selector that will contain the character

    private LevelController levels_data;                            // Data-object that contains the characters per level and the containers per character

    
    private bool is_host = false;                                   // Boolean value that tells whether this player is hosting (Master Client) a room.




    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // If we're already connected, we don't need to connect again but can immediately initialize the begin screen.
        if (PhotonNetwork.IsConnected){
            initializeBeginScreen();
        }
    

        // Initialize the data that belongs to the levels
        levels_data = new LevelController();
        
        room_list_cache = new Dictionary<string, RoomInfo>();
        room_button_entries = new Dictionary<string, GameObject>();
    }

    
    // Override callback function that will be called when the client gets connected to the Photon Master Servers
    public override void OnConnectedToMaster()
    {
        initializeBeginScreen();
    }

    // Callback that will be called when joining the room failed
    public override void OnJoinRoomFailed (short returnCode, string message){

        // If the host failed joining a room (creation failed), he needs to go back to the create panel
        if(is_host){
             // TODO: Display join failed message
            level_selector.SetActive(false);
            character_select.SetActive(false);
            create_panel.SetActive(true);
        }
        // If it's a client, we want to display the lobby panel again.
        else{
            // TODO: Display join failed message
            character_select.SetActive(false);
            lobby_panel.SetActive(true);

        }
        Debug.Log("Joining room failed: " + message);
    }
    
    // Callback that will be called when joining the lobby succeeded.
    public override void OnJoinedLobby(){
        Debug.Log("Lobby was joined!");
    }

    // Callback that will be called when joining a room succeeded.
    public override void OnJoinedRoom(){
        level_selector.SetActive(false);
        character_select.SetActive(true);
        instantiateCharacterSelector((int)PhotonNetwork.CurrentRoom.CustomProperties["levelID"]);
        Debug.Log("Room you're currently in:" + PhotonNetwork.CurrentRoom.ToString());
    }
    
    // Callback that will be called when leaving a room.
    public override void OnLeftRoom(){
        Debug.Log("Current room was left.");
    }

    // Callback that will be called when creating a room fails.
    public override void OnCreateRoomFailed(short returnCode, string message){
        Debug.Log("Create room failed: " + message);
    }

    
    // Updates the nickname PlayerPrefs and on the network
    public void updateNickName(string input)
    {   
        Debug.Log("Nickname was updated!");        
        PhotonNetwork.NickName = input;
        PlayerPrefs.SetString("NickName", input);
    }


    // Handles what happens when the user clicks the 'join game' button
    public void joinButtonOnClick()
    { 
        ClearRoomListView();
        is_host = false; 
        main_panel.SetActive(false);
        lobby_panel.SetActive(true);
        PhotonNetwork.JoinLobby();
        UpdateRoomListView();
    }

    // Is called when the user presses a RoomButton in the lobby, this will cause them to join this room.
    public void onRoomButtonClick(int level_id)
    {
        lobby_panel.SetActive(false);
        character_select.SetActive(true);
    }  

    // Handles what happens when the user clicks the 'host game' button
    public void hostButtonOnClick() 
    {
        PhotonNetwork.JoinLobby();
        is_host = true;
        main_panel.SetActive(false);
        create_panel.SetActive(true);
    }

    // Handles what happens when the user clicks the 'create room' button
    public void createButtonOnClick() 
    {
        string roomName = roomname_textfield.text;

        if (roomName != null) {

            // Check if roomName already exists
            if (!checkRoomName(roomName)) {

                // Save the room's name, and go to level selector
                room_name = roomName;
                create_panel.SetActive(false);
                level_selector.SetActive(true);
            }
            else{
                // TODO: give notification that this room name already exists
            }
        }
        
        //TODO: give warning when blank
    }


    // Function that initializes the start screen of the application
    private  void initializeBeginScreen(){
           // Turn off the loading buttons
        loading_button1.SetActive(false);
        loading_button2.SetActive(false);

        // Replace them with the real buttons
        lobby_joinButton.SetActive(true);
        lobby_createButton.SetActive(true);

        if(PlayerPrefs.HasKey("NickName"))
        {
            if(PlayerPrefs.GetString("NickName") == ""){
                // If the saved name in the playerprefs is an empty string, generate a random sample name
                PhotonNetwork.NickName = "TheLegend" + UnityEngine.Random.Range(0,10000);      
            }
            else
            {
                // Otherwise we get the name that was saved in playerprefs
                PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");     
            }
        }
        else
        {
            // If there was no name saved yet, we can also generate a random one
            PhotonNetwork.NickName = "TheLegend" + UnityEngine.Random.Range(0,10000);     
        }
        // We put the current nickname in the textfield 
        nickname_textfield.text = PhotonNetwork.NickName;   
    }

    // Creates room with given roomName
    // @param: roomName: The name of the room to be created
    // @param: level_id: The room's level ID
    // @param: max_players: The max. amount of players allowed in this room.
    private void createRoom(string roomName, int level_id, byte max_players) 
    {
        // Room options
        RoomOptions room_ops = new RoomOptions()
	       { 
			MaxPlayers = max_players,
			IsVisible = true,
            IsOpen = true,
            PlayerTtl = 0,
            EmptyRoomTtl = 0
	      };

          // Get the characters for this level (and thus room)
          string[] characters = levels_data.getCharacters(level_id);

          //Set custom properties to the room
          string[] roomprops = new string[2 + characters.Length];
          roomprops[0] = "levelID";
          roomprops[1] = "initialized";

          // Add the names of the characters as keys to the room's properties
          for (int i = 0; i < characters.Length ; i++){
              roomprops[i+2] = characters[i];
          }

          room_ops.CustomRoomPropertiesForLobby = roomprops;

          // Set values to the custom properties
          ExitGames.Client.Photon.Hashtable custom_props = new ExitGames.Client.Photon.Hashtable();
          custom_props.Add("levelID", level_id);
          custom_props.Add("initialized", 0);
          
          foreach(string name in characters){
              custom_props.Add(name, true);
          }

          room_ops.CustomRoomProperties = custom_props;

        // Create the room
        if(PhotonNetwork.CreateRoom(roomName, room_ops)){
            Debug.Log("Room " + roomName + " was created!");

        }
        else{
            Debug.Log("Something went wrong creating " + roomName);
        }
    }

    // Checks if roomName already exists
    private bool checkRoomName(string roomName)
    {
        foreach (RoomInfo room in room_list_cache.Values)
            if (room.Name == roomName)
                return true;
        return false;
    }


    // Callback function that handles what happens when an update in the list of available rooms comes through from the servers.
    // @param roomList: The list of Rooms retrieved from the Photon Master Server.
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    { 
        ClearRoomListView();
        UpdateCachedRoomList(roomList);
        UpdateRoomListView();        
    }


      // Updates the internally cached room list with the newest data received from the Photon Master servers.
      private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            foreach (RoomInfo info in roomList){

                // Remove room from cached room list if it got closed, became invisible or was marked as removed
                if (!info.IsOpen || !info.IsVisible || info.RemovedFromList){
                    if (room_list_cache.ContainsKey(info.Name)){
                        room_list_cache.Remove(info.Name);
                    }

                    continue;
                }

                // Update cached room info
                if (room_list_cache.ContainsKey(info.Name)){
                    room_list_cache[info.Name] = info;
                }
                // Add new room info to cache
                else{
                    room_list_cache.Add(info.Name, info);
                }
            }
        }


        // Takes the Room data from the internally cached room list and updates
        // the UI room listings based on this data.
        private void UpdateRoomListView()
        {
            foreach (RoomInfo info in room_list_cache.Values)
            {
                GameObject entry = Instantiate(room_list_object_prefab);
                entry.transform.SetParent(room_container);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<RoomButton>().SetRoom(info.Name, (int)info.CustomProperties["levelID"] , "Level", info.MaxPlayers, (byte)info.PlayerCount);

                room_button_entries.Add(info.Name, entry);
            }
        }

        // Resets the list of room listings in the UI container.
        // This method is necessary to prevent that there's duplicates of rooms displayed in the UI listings.
          private void ClearRoomListView()
        {
            foreach (GameObject entry in room_button_entries.Values)
            {
                Destroy(entry.gameObject);
            }

            room_button_entries.Clear();
        }

    // Predicate function that's used to search through the list of rooms by the room name
    static System.Predicate<RoomInfo> ByName(string name)
    {     
        return delegate(RoomInfo room){
            return room.Name == name;
        };
    }

    // Handles what needs to happen when the user presses the 'cancel' button, which will head them back to the main menu.
    public void cancelMatchmaking()
    {
        main_panel.SetActive(true);
        lobby_panel.SetActive(false);
        create_panel.SetActive(false);
        PhotonNetwork.LeaveLobby(); 
    }

    // This function is called when the user presses 'Back' in the character selector
    public void cancelCharacterSelection()
    {
        character_select.SetActive(false);
        
        // If the user was a host, they need to go back to level select, otherwise the user has to go back to the lobby
        if(is_host){
            level_selector.SetActive(true);
        }
        else{
            lobby_panel.SetActive(true);
             ClearRoomListView();
             UpdateRoomListView();
        }
        PhotonNetwork.LeaveRoom();  
        PhotonNetwork.JoinLobby();
    }

    // Callback function which is called when the user chooses a level, the corresponding character selector will be loaded.
    // The Photon Room will also be created. Note that this happens before the AR Room initialization starts, so other players
    // can already join the room and will be able to start initializing their rooms already.
    public void onLevelSelect(int level_id)
    {
        createRoom(room_name, level_id, levels_data.getPlayersPerLevel(level_id));
    }

    // Dynamically generates character select buttons, based on which level was selected.
    private void instantiateCharacterSelector(int level_id)
    {
        resetCharacterContainer(); 

        // Get all the characters that belong to the selected level (COPY THEM!)
        string[] characters_to_display = levels_data.getCharacters(level_id);
        
        // Create a new hashtable to store and set a custom room property which says which character was taken by this player
        ExitGames.Client.Photon.Hashtable character_taken = new ExitGames.Client.Photon.Hashtable();


        // Check which characters are already taken
        foreach(string name in characters_to_display){
            if(!((bool)PhotonNetwork.CurrentRoom.CustomProperties[name])){
                Debug.Log("Has to be removed!");
                // Remove the name from the array of characters to display
                characters_to_display = characters_to_display.Where(val => val != name).ToArray();
            }
        }


        // Generate a button for each character
        foreach (string character in characters_to_display){
            GameObject button1 = Instantiate(button_prefab, character_container);
            button1.GetComponentInChildren<Text>().text = character;

            // Tell the button what to do when it gets clicked
            Button buttonchild = button1.GetComponent<Button>();
            buttonchild.onClick.AddListener(delegate(){
                // Communicate to the room that this character is now taken.
                Debug.Log(PhotonNetwork.CurrentRoom.ToString());
                character_taken.Add(character, false);
                PhotonNetwork.CurrentRoom.SetCustomProperties(character_taken);

                // Send the updates over the network
                PhotonView photonview = PhotonView.Get(this);
                photonview.RPC("updateCharacterSelector", RpcTarget.Others, level_id);

                startRoomInitialization(character);
            });
        }
    }

    // Starts the room initialization, loads the corresponding containers that belong to the character.
    // @param character : The character that we want to initialize the room for.
    private void startRoomInitialization(string character)
    {
        // Save the selected character in the player prefs.
        PlayerPrefs.SetString("character", character);

        // Start the room initialization
        SceneManager.LoadScene("InitializationScene");
    }

    // Deletes all the buttons that were in the character selector (if there were any already), so we won't duplicate buttons
    private void resetCharacterContainer()
    {
       var all_children = character_container.GetComponentsInChildren<Button>();  

       foreach (Button child in all_children){
           GameObject.Destroy(child.gameObject);
       }
    }

    [PunRPC]
    void updateCharacterSelector(int level_id){
        resetCharacterContainer();
        instantiateCharacterSelector(level_id);
    }
}
