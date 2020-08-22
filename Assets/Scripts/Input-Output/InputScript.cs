using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

/**
 * This class handles the input of a puzzel
 * @author Joris Bertram
 */
public class InputScript : MonoBehaviour 
{
    private int id;

    /* Handlers bools */

    [SerializeField]
    private bool onTouchBool;
    
    [SerializeField]
    private bool onTriggerBool;
        
    [SerializeField]
    private bool onTextInputBool;

    [SerializeField]
    private Canvas canvas;
    
    /* Types of input */

    [SerializeField]
    private bool storeItem;

    [SerializeField]
    private GameObject storedItem;

    [SerializeField]
    private GameObject useableItem;

    [SerializeField]
    private string inputText;

    /* Puzzle Controller */
    private PuzzleController controller;

    void Start() {
        // Update visualisation of stored item when input stores item
        if (storeItem)
            changeStoredItem(storedItem);
    }

    public void setController(PuzzleController controller) {
        this.controller = controller;
    }

    public void setID(int id) {
        this.id = id;
    }

    /**
     * Checks if given item is right item for input
     */
    public bool isRightItem(GameObject item) {
        return (useableItem != null && useableItem.tag == item.tag);
    }

    /**
     * Function that is called when input is touched by a player
     */
    public void touched(GameObject item) {   
        Debug.Log("Input: touched");

        // If puzzle already solved do nothing
        if (isPuzzleSolved())
            return;

        if (onTouchBool) {
            if (storeItem) 
            {
                changeStoredItem(item);
            }
            if (useableItem == null || isRightItem(item)) 
            {
                controller.setInputState(id, true);
            }
            else
            {
                controller.setInputState(id, false);
            }
        }
        // If we're dealing with a textual input, we want to display the numpad
        if(onTextInputBool){
            canvas.gameObject.SetActive(true);
        }
    }

    /**
     * Function that is called when object triggers input
     */
    private void OnTriggerEnter(Collider other) {
        // If puzzle already solved do nothing
        if (isPuzzleSolved())
            return;

        Debug.Log("Trigger activated");

        if (onTriggerBool) {
            if (useableItem == null || isRightItem(other.gameObject)) 
            {
                controller.setInputState(id, true);
            }
            else 
            {
                controller.setInputState(id, false);
            }
        }        
    }

    private void OnTriggerStay(Collider other) {

    }

    /**
     * Function that is called when object doesn't trigger input anymore
     */
    private void OnTriggerExit(Collider other) {
        Debug.Log("TriggerExit called");
        // If puzzle already solved do nothing
        if (isPuzzleSolved())
            return;

        Debug.Log("Trigger deactivated");

        controller.setInputState(id, false);
    }

    /**
     * Function that is called when input gets text as input
     */
    public void onTextInput(string text) {
        if (onTextInputBool) {
            if (inputText == text) 
            {
                controller.setInputState(id, true);
            }
            else 
            {
                controller.setInputState(id, false);
            }
        }
    }


    /**
     * Gets the puzzle's state from the puzzle controller
     */
    public bool isPuzzleSolved() {
        return controller.isSolved();
    }

    public bool isStoringItem() {
        return storeItem;
    }

    /**
     * Gets the input's state from the puzzle controller
     */
    public bool getState() {
        return controller.getInputState(id);
    }

    public GameObject getStoredItem() {
        return storedItem;
    }

    public string getInputText() {
        return inputText;
    }


    private void changeStoredItem(GameObject item) {
        storedItem = item;

        // If has previous item destroy it
        if (gameObject.transform.childCount > 0) 
        {
            GameObject oldItem = gameObject.transform.GetChild(0).gameObject;
            Destroy(oldItem);
        }

        // If new item exists show it
        if (item != null) 
        {
            GameObject newItem = Instantiate(item, gameObject.transform.position, gameObject.transform.rotation);

            // Make new item child of input
            newItem.transform.parent = gameObject.transform;

            // Show new item
            newItem.SetActive(true);

            // Turn new item box collider off
            newItem.GetComponent<BoxCollider>().enabled = false;
        }
    }
        
}
