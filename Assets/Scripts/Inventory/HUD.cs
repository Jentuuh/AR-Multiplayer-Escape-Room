using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/**
 * This is a class that holds all the parts for the HUD, such as inventory or settings
 * @author Tim
 */
public class HUD : MonoBehaviour
{
    // TODO: add settings element
    [SerializeField]
    private Inventory mInventory;
    
    [SerializeField]
    private SharedInventory mSharedInventory;

    [SerializeField]
    private GameObject mEquippedName;

    // TODO: add settings element
    [SerializeField]
    private DebugSystem mDebug;

    

    // Start is called before the first frame update
    void Start()
    {
        mInventory.ItemAdded += InventoryScript_ItemAdded;
        mInventory.ItemRemoved += InventoryScript_ItemRemoved;
        mSharedInventory.ItemAdded += InventoryScript_ItemAdded;
        mSharedInventory.ItemRemoved += InventoryScript_ItemRemoved;
    }

    /**
     * Method that updates the inventory when an item gets added
     * @param sender, origin of the event
     * @param e wrapper with the items that are passed with the event
     */
    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e) {
        //debug.SendDebugMessageToConsole("HUD: Added " + e.Item.Name);
        updateInventory(sender);        
    }

    /**
     * Method that updates the inventory when an item gets removed
     * @param sender, origin of the event
     * @param e wrapper with the items that are passed with the event
     */
    private void InventoryScript_ItemRemoved(object sender, InventoryEventArgs e) {
        //debug.SendDebugMessageToConsole("HUD: Removed " + e.Item.Name);
        updateInventory(sender);
    }
    
    private void updateInventory(object sender) {
        AbstractInventory inventory = (AbstractInventory)sender;
        //debug.SendDebugMessageToConsole("HUD: update " + inventory.name);
        Transform inventoryPanel = this.transform.Find(inventory.name); // get the type of inventory, SharedInventory or Inventory
        Transform slot, itemImage;
        Image image;
        DragHandler dragHandler;
        for(int i = 0; i < inventory.getSize(); i++) {
            slot = inventoryPanel.Find("Slot"+i);
            Debug.Log("HUD: Updating " + slot.name);

            // If slot exists update image
            if (slot != null) {
                if(slot.GetChild(0).childCount > 0){
                    itemImage = slot.GetChild(0).GetChild(0);
                    image = itemImage.GetComponent<Image>(); // get the image element of a slot, slot->border->itemImage
                    dragHandler = itemImage.GetComponent<DragHandler>();

                    // Sync slot image with inventory slot data
                    InvItem item = inventory.getItem(i);

                    // If item exists
                    if (item != null) {
                        Debug.Log("HUD: Inventory slot " + i + " not empty -> update this");
                        image.sprite = item.Image;
                        image.enabled = true;
                    }
                    // If no item
                    else {
                        Debug.Log("HUD: Inventory slot " + i + " empty -> update this");
                        image.sprite = null;
                        image.enabled = false;
                    }

                    // Store reference of item in draghandler 
                    dragHandler.setItem(item);
                    dragHandler.setParent(slot.GetChild(0));
                }
            }
        }
    }

    /**
    * Function thats called when slot is touched in HUD
    */
    public void onEquip(int slot) {
        InvItem equippedItem = mInventory.getItem(slot);

        // Hide equipped name if no item
        if (equippedItem == null) 
        {
            mEquippedName.SetActive(false);
        }
        // Show item name
        else 
        {
            mEquippedName.GetComponentInChildren<Text>().text = equippedItem.Name;
            mEquippedName.SetActive(true);
        }
        
        mInventory.equipItem(slot);
    }
}
