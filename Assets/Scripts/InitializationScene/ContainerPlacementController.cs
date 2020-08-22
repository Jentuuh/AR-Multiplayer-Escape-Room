using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Experimental.XR;
using GoogleARCore;
using System;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;

/**
* @author jentevandersanden
* This class has control over the placement indicator in the InitializationScene 
* and the placement of containers throughout the scene. 
*/
public class ContainerPlacementController : MonoBehaviour
{
    private const int MAX_CONTAINERS = 10; 

    [SerializeField]
    private GameObject placement_indicator;         // The GameObject that contains a Quad which represents the placement indicator of the containers

    [SerializeField]
    private GameObject container_prefab;            // The prefab of the container footprint that will be placed around the room

    [SerializeField]
    private GameObject confirm_button;              // The confirm button in the bottom center of the screen

    [SerializeField]
    private GameObject done_button;                 // The 'done' button in the bottom center of the screen

    [SerializeField]
    private GameObject waiting_room_panel;          // The UI panel that represents the waiting room

     [SerializeField]
    private Text host_text;                         // The text that the host gets to see in the waiting room.

    [SerializeField]
    private Text client_text;                       // The text that the host gets to see in the waiting room.

    private GameObject[] containers_to_place;       // An array of the loaded containers that belong to this level and need to be placed.
    private string[] messages_to_display;           // Array of messages that belong to each container that needs to be placed.

    private Pose[] container_poses;                 // The poses that the user will assign to each container

    private GameObject[] placed_containers;         // An array of the already instantiated container footprint prefabs

    private Pose placementpose;                     // The pose of the placement indicator
    private bool placement_is_valid;                // Boolean value that says whether we can place a container(if a plane is detected or not)

    private int containercount;                     // The amount of containers we've already added to the scene

    private LevelController container_data;         // The levelcontroller intializes all the data that belongs to a level.

    private bool is_host;                           // Tells whether this player hosts the current room.

    [SerializeField]
    private GameObject message_text;


    // Start is called before the first frame update
    void Start()
    {
        // Check whether this player hosts this room
        is_host = PhotonNetwork.IsMasterClient;

        // Initialize data, placement validity is initially false by default, and obviously the end notification shouldn't be displayed yet
        container_data = new LevelController();
        placement_is_valid = false;

        // Allocate space for the data arrays of the containers
        placed_containers = new GameObject[MAX_CONTAINERS];
        messages_to_display = new string[MAX_CONTAINERS];
        containers_to_place = new GameObject[MAX_CONTAINERS];


        // We get the containers of the character that was selected by the player.
        messages_to_display = container_data.getMessages(PlayerPrefs.GetString("character"));
        containers_to_place = container_data.getContainers(PlayerPrefs.GetString("character"));
       
        container_poses = new Pose[MAX_CONTAINERS];

        // Set the amount of containers already placed to 0
        containercount = 0;
        message_text.GetComponent<Text>().text = messages_to_display[containercount];

        // Check initially for the case that there's no containers to place at all (this shouldn't be the case, but this is to prevent
        // that IF this happens the user could place an infinite amount of container footprints.)
        if(containers_to_place[containercount] == null){
            confirm_button.SetActive(false);
            done_button.SetActive(true);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (placement_indicator != null) {
            updatePlacementPose();
            updatePlacementIndicator();
        }
    }

// Updates the underlying Pose of the placement indicator
    private void updatePlacementPose()
    {
        // We want to raycast from the center of the screen
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3 (0.5f, 0.5f));

        // This will store the point that we hit with our raycast
        TrackableHit hit;

        // Filter for the hits that will be found
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds | TrackableHitFlags.PlaneWithinPolygon;

        // We raycast and store the result in the variable 'hit'.
        if(Frame.Raycast(screenCenter.x,screenCenter.y,raycastFilter,out hit)){

                // Update the Pose of the indicator
                placement_is_valid = true;
                placementpose = hit.Pose;
        }
        else{
            placement_is_valid = false;
        }
    }

// Updates the location of the visual placement indicator
    private void updatePlacementIndicator(){

        // Checks if the indicator can be placed (e.g. a plane was detected)
        if(placement_is_valid && placement_indicator != null){
            placement_indicator.SetActive(true);

            // Update visual position
            placement_indicator.transform.SetPositionAndRotation(placementpose.position, placementpose.rotation);
            // If we can find the container, then we change the scale to the container's scale
            if(containers_to_place[containercount].transform.Find("Container") != null){
                placement_indicator.transform.localScale = containers_to_place[containercount].transform.Find("Container").transform.localScale;
            }
        }
        else{
            placement_indicator.SetActive(false);
        }
    }

    // Callback function that's called when the player presses the 'Confirm' button.
    public void onConfirmClick(){

        // Make sure the point we're looking at is valid to place a container on
            if(placement_is_valid){
                placeContainerFootprint();
                message_text.GetComponent<Text>().text = messages_to_display[containercount];
            }
    }

    // Callback function that's called when the player presses the 'Done' button.
    public void onDoneClick(){

        // Update Custom room property of initialized players 
        incrementInitialized();

        // Remove placement indicator
        GameObject.Destroy(placement_indicator);
        done_button.SetActive(false);
        waiting_room_panel.SetActive(true);

        if(is_host){
            host_text.enabled = true;
        }
        else{
            client_text.enabled = true;
        }
    }

    // Places a container prefab in the scene (on the Pose you're currently looking to).
    private void placeContainerFootprint(){

        // We place a prefab footprint on the place we placed a container, so the user gets some kind of feedback
        // Additionally, we add it to a list so we can delete the footprints and replace them by the actual containers later
        placed_containers[containercount] = Instantiate(container_prefab, placementpose.position, placementpose.rotation);
        
        if(containers_to_place[containercount].transform.Find("Container") != null){
            placed_containers[containercount].transform.localScale = containers_to_place[containercount].transform.Find("Container").transform.localScale;
        }

        // We copy the placement indicator's Pose without reference
        Pose copy_pose = new Pose();
        copy_pose.position = placementpose.position;
        copy_pose.rotation = placementpose.rotation;

        // Then we add it to our list of Poses for the containers
        container_poses[containercount] = copy_pose;
        containercount += 1;

        // If we placed all the container footprints we had to place, replace the 'confirm button' by the 'done button'
        if(containers_to_place[containercount] == null){
            confirm_button.SetActive(false);
            done_button.SetActive(true);
            message_text.SetActive(false);
        }
    }

    // Deletes all previously placed containers, this function will be used when we'll replace the footprints
    // with actual containers.
    public void deleteContainerFootprints(){
        foreach(GameObject container in placed_containers){
            Destroy(container);
        }
    }


    // For every container footprint in the scene, this function places a corresponding actual container.
    public void placeContainers(){
        // First we delete all the placeholder footprints in the scene
        deleteContainerFootprints();
        int i = 0;

        // Check if we aren't checking empty spots in the array, and if we haven't reached the end of the array
        while(i < containercount){

            // Then we replace the footprints by the actual corresponding containers
            Instantiate(containers_to_place[i], container_poses[i].position, container_poses[i].rotation);
            i += 1;
        }
        containercount = 0;
    }

    // Function that disables the waiting room UI panel.
    public void disableWaitingRoomUI(){
        waiting_room_panel.SetActive(false);
    }


    // Increments the amount of initialized players in a room.
    private void incrementInitialized(){
        int value = (int)PhotonNetwork.CurrentRoom.CustomProperties["initialized"];
        int newValue  = value  + 1;

        ExitGames.Client.Photon.Hashtable setValue = new ExitGames.Client.Photon.Hashtable();
        setValue.Add("initialized", newValue);

        ExitGames.Client.Photon.Hashtable expectedValue = new ExitGames.Client.Photon.Hashtable();
        expectedValue.Add("initialized", value);

        PhotonNetwork.CurrentRoom.SetCustomProperties(setValue, expectedValue);
    }   

}
