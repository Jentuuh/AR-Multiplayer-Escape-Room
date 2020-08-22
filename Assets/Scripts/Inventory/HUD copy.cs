/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 * This is a class that holds all the parts for the HUD, such as inventory or settings
 * @author Tim
 *
public class HUD : MonoBehaviour
{
    // TODO: add settings element
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private SharedInventory sharedInventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory.ItemAdded += InventoryScript_ItemAdded;
        inventory.ItemRemoved += InventoryScript_ItemRemoved;
        sharedInventory.ItemAdded += InventoryScript_ItemAdded;
        sharedInventory.ItemRemoved += InventoryScript_ItemRemoved;
    }

    /**
     * Method that updates the inventory when an item gets added
     * @param sender, origin of the event
     * @param e wrapper with the items that are passed with the event
     *
    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e) {
        updateInventory(sender);
    }

    /**
     * Method that updates the inventory when an item gets removed
     * @param sender, origin of the event
     * @param e wrapper with the items that are passed with the event
     *
    private void InventoryScript_ItemRemoved(object sender, InventoryEventArgs e) {
        updateInventory(sender);
    }

    private void updateInventory(object sender) {
        AbstractInventory inventory = (AbstractInventory)sender;
        Transform inventoryPanel = this.transform.Find(inventory.name); // get the type of inventory, SharedInventory or Inventory

        Transform slot, HUD_item;
        Image image;
        DragHandler dragHandler;
        for(int i = 0; i < inventory.getSize(); i++) {
            slot = inventoryPanel.GetChild(i);

            // If slot exists update image
            if (slot != null) {
                if(slot.GetChild(0).childCount > 0){
                    HUD_item = slot.GetChild(0).GetChild(0);
                    image = HUD_item.GetComponent<Image>(); // get the image element of a slot, slot->border->itemImage
                    dragHandler = HUD_item.GetComponent<DragHandler>();

                    // Sync slot image with inventory slot data
                    InvItem item = inventory.getItem(i);

                    // If item exists
                    if (item != null) {
                        image.sprite = item.Image;
                        image.enabled = true;
                    }
                    // If no item
                    else {
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
    *
    public void onEquip(int slot) {
        inventory.equipItem(slot);
    }
}
*/