/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
originial hud
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
        AbstractInventory inv = (AbstractInventory)sender;
        Transform inventoryPanel = transform.Find(inv.name); // get the name of the object, SharedInventory or Inventory
        foreach (Transform slot in inventoryPanel)
        {
            if(slot.GetChild(0).childCount > 0) {
                Transform itemScripts = slot.GetChild(0).GetChild(0);
                Image image = itemScripts.GetComponent<Image>(); // get the image element of a slot, slot->border->itemImage
                DragHandler DragHandler = itemScripts.GetComponent<DragHandler>();

                // if the slot is empty
                if(!image.enabled) {
                    image.sprite = e.Item.Image;
                    image.enabled = true;

                    // Store reference of item in draghandler 
                    DragHandler.setItem(e.Item);
                    DragHandler.setParent(slot.GetChild(0));

                    break;
            }
        }
        }
        
    }

    /**
     * Method that updates the inventory when an item gets removed
     * @param sender, origin of the event
     * @param e wrapper with the items that are passed with the event
     *
    private void InventoryScript_ItemRemoved(object sender, InventoryEventArgs e) {
        AbstractInventory inv = (AbstractInventory)sender;
        Transform inventoryPanel = transform.Find(inv.name);
        foreach (Transform slot in inventoryPanel)
        {
            if(slot.GetChild(0).childCount > 0) {
                Transform itemScripts = slot.GetChild(0).GetChild(0);
                Image image = itemScripts.GetComponent<Image>(); // get the image element of a slot, slot->border->itemImage
                DragHandler DragHandler = itemScripts.GetComponent<DragHandler>();
                
                // if the slot is empty
                if(image.sprite == e.Item.Image) {
                    image.sprite = null;
                    image.enabled = false;

                    // delete reference to item in itemDragHandler
                    DragHandler.setItem(null);
                    break;
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
}*/
