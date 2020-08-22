using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

/**
 * <description>
 * @author Joris Bertram
 */
public class SelectionController : MonoBehaviour
{
    [SerializeField]
    private DebugSystem debug;

    [SerializeField]
    private Camera arCamera;
    
    [SerializeField]
    private GameObject equippedItem;

    [SerializeField]
    private Inventory inventory;

    [SerializeField]
    private InvItem testItem;

    private bool addedTestItem = false;

    void Start()
    {
        inventory.ItemEquipped += InventoryScript_ItemEquipped;
    }

    // Update is called once per frame
    void Update()
    {
        /* Add TestItem if not null */
        if (!addedTestItem && testItem != null) {
            inventory.addItem(testItem);
            addedTestItem = true;
        }

        /* Send out a ray from touch position or mousePosition respectively (or none if none apply) */
        Touch touch;
        Ray ray;
        if (Input.touchCount == 1 && (touch = Input.GetTouch(0)).phase == TouchPhase.Began) 
        {
            ray = arCamera.ScreenPointToRay(touch.position);
        }
        else if (Input.GetMouseButtonDown (0)) 
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        else 
        {
            return;
        }

        /* Gets the first hit from the ray */
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) 
        {
            //debug.log("Selection: hit something");
            handleInput(hit.transform.GetComponent<InputScript>());
            handleItem(hit.transform.GetComponent<InvItem>());      
            handleNote(hit.transform.GetComponent<NoteScript>()); 
        }
    }

    private void handleInput(InputScript input) {
        /* If not input or puzzle is solved do nothing */
        if (input == null || input.isPuzzleSolved())
            return;

        //debug.log("Selection: hit input");
        /* See if input is storing item or instantly uses it */
        if (input.isStoringItem()) 
        {
            handleStoreInput(input);
        }
        else 
        {
            handleUseInput(input);
        }       
    }

    // Switch equippedItem with stored input item
    private void handleStoreInput(InputScript input) {
            GameObject temp = input.getStoredItem();
            
            /* Add stored input item to inventory if exists */
            if (temp != null)
                handleItem(temp.GetComponent<InvItem>());

            input.touched(equippedItem);

            if (equippedItem != null) {
                //debug.log("Selection: Removed" + equippedItem.GetComponent<InvItem>().Name);
                inventory.removeItem(equippedItem.GetComponent<InvItem>());
            }
            equippedItem = null; 
    }

    // Use and delete equippedItem
    private void handleUseInput(InputScript input) {
        // When input isn't already correct
        if (!input.getState()) {
            input.touched(equippedItem);

            if (equippedItem != null && input.isRightItem(equippedItem)) 
            {
                //debug.log("Selection: Removed" + equippedItem.GetComponent<InvItem>().Name);
                inventory.removeItem(equippedItem.GetComponent<InvItem>());
                equippedItem = null; 
            }
        }
    }

    private void handleItem(InvItem item) {
        // If not item do nothing
        if(item == null) {
            //debug.log("Selection: item none");
            return;
        }
        //debug.log("Selection: hit item");
        //debug.log("Selection: Added" + item.Name);
        
        // Hide gameObject of item
        item.onPickup();
        inventory.addItem(item);
    }

    // Function that will be called when a note is touched
    private void handleNote(NoteScript note){
        // If not a note, do nothing 
        if(note == null){
            return;
        }
        note.showNoteImage();
    }

    public void InventoryScript_ItemEquipped(object sender, InventoryEventArgs e) {
        // If item exists equip it else equip nothing
        if (e.Item != null)
            equippedItem = e.Item.gameObject;
        else
            equippedItem = null;
    }
}
