
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    * @author Tim
    * Abstract class with the foundations for an inventory
    */
public abstract class AbstractInventory: MonoBehaviour
{
    private int slotCount;
    private InvItem[] mInvItems;
    public event System.EventHandler<InventoryEventArgs> ItemAdded; // event that gets triggered if an item is added
    public event System.EventHandler<InventoryEventArgs> ItemRemoved; // event that gets triggered if an item is added
    
    protected void initialize() {
        // Make inventory
        mInvItems = new InvItem[slotCount];
        for (int i = 0; i < slotCount; i++)
            mInvItems[i] = null;
    }

    public int getSize(){return mInvItems.Length;}
    /**
    * Getters
    */
    public InvItem getItem(int slot){return mInvItems[slot];}
    public int SlotCount{
        get{
            return slotCount;
        }
        set{
            slotCount = value;
        }
    }

    /**
    * Adds an InvItem to the Inventory
    * @param item, item to be added to the inventory
    */ 
    public void addItem(InvItem item)
    {
        if(item == null) {
            return;
        }
        // search for empty slot
        for (int i = 0; i < slotCount; i++) {
            // when empty spot found
            if (mInvItems[i] == null) {
                mInvItems[i] = item;
                Debug.Log("Inventory: added " + item.Name + " on slot " + i);

                // raise item added event 
                if (ItemAdded != null)
                    ItemAdded(this, new InventoryEventArgs(item));
                return; 
            }
        }
    }

    /**
    * Removes an InvItem from the Inventory
    * @param item, item to be removed from the inventory
    */ 
    public bool removeItem(InvItem item)
    {
        // if inventory contains item
        for (int i = 0; i < slotCount; i++) {
            if(mInvItems != null){
            if (mInvItems[i] == item) {
                Debug.Log("Inventory: removed " + item.Name + " from slot " + i);
                mInvItems[i] = null;

                // raise item remove event 
                if (ItemRemoved != null)
                    ItemRemoved(this, new InventoryEventArgs(item));
                return true;
            }
            }
        }
        //If no item was found check for items with same name
        for (int i = 0; i < slotCount; i++) {
            if(mInvItems != null){
                if (mInvItems[i].Name == item.Name) {
                    Debug.Log("Inventory: removed " + item.Name + " from slot " + i);
                    mInvItems[i] = null;

                    // raise item remove event 
                    if (ItemRemoved != null)
                        ItemRemoved(this, new InventoryEventArgs(item));
                    return true;
                }
            }
        }
        return false;
    }

    /**
     * remove item at index
     */
    public bool removeItemIndex(int index) {
        if (index > 0)
        {


            if (mInvItems[index] != null)
            {
                mInvItems[index] = null;
                return true;
            }
            return false;
        }
        return false;
    }

    /**
     * get the index of an item inside inventory
     * @returns index if item is inside inventory 
     * @returns -1 if item is not in inventory
     */
    public int getIndexOf(InvItem item) {
        for (int i = 0; i < slotCount; i++)
        {
            if (mInvItems != null)
            {
                if (mInvItems[i] == item)
                {
                    return i;
                }
            }
        }
        return -1;
    }
}


