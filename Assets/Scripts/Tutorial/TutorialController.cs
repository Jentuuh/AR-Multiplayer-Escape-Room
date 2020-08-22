using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* @author jentevandersanden
* This is the controller that handles everything regarding to the tutorial's UI.
*/
public class TutorialController : MonoBehaviour

{         
    [SerializeField]
    private GameObject inventory_HUD;
               
    [SerializeField]       // All the UI components belonging to the tutorial's instructions
    private List<GameObject> instructions_info = new List<GameObject>();


    private int current_stage = 0;              // Where we currently are in the tutorial


    [SerializeField]
    private List<GameObject> items_to_hide = new List<GameObject>();

    private void showAllUI(){
       foreach (GameObject i in items_to_hide)
        {
            if(i != null){
                i.SetActive(true);
            }
        }
    }

    // Hides all UI items from 'items_to_hide', except for a specified index.
    private void hideAllOthers(int j){
        for(int i = 0; i < items_to_hide.Count; i++ ){
            if (items_to_hide[i] != null && i != j){
                items_to_hide[i].SetActive(false);
            }
        }
    }

    public void showFirstInstruction(){

        // Show first instruction
        instructions_info[0].SetActive(true);
        // Set the HUD active, otherwise the inventories will never be visible
        inventory_HUD.SetActive(true);
        // Hide every UI element for the first instruction
        hideAllOthers(0);
    }

    // Function that's called when the 'continue' buttons on the message panels are clicked
    public void onContinueClick(){

        // Hide last instruction
        hideInstruction(current_stage);
        current_stage += 1;
        // Show new instruction
        showNextInstruction(current_stage);
        // We can't go out of bounds, because the last item in the list has a 'Start' button instead of a 'Continue' button

        // Show corresponding UI
        showAllUI();
        hideAllOthers(current_stage);
    }

    // Function that's called when the 'start' button on the last message panel is clicked.
    public void onStartClick(){
        // TODO: let bot send message in chat
        hideInstruction(current_stage);
        showAllUI();

        // Place actual containers
        GameObject.Find("ContainerPlacementController").GetComponent<ContainerPlacementController>().Invoke("placeContainers", 0.5f);
    }


    private void showNextInstruction(int i){
        instructions_info[i].SetActive(true);
    }
    private void hideInstruction(int i){
        instructions_info[i].SetActive(false);
    }
}
