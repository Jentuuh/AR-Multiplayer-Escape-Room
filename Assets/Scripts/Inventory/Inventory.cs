using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Inventory class that holds the individual item slots
 * @author Tim
*/
public class Inventory : AbstractInventory
{ 
    public event System.EventHandler<InventoryEventArgs> ItemEquipped; // event that gets triggered if an item is equipped

    private void Start() {
        SlotCount = 5;
        initialize();
    }
    
    /**
     * Equips item in given slot
     */
    public void equipItem(int slot) {
        
        InvItem item = new InvItem();
        try{
            item = getItem(slot);

            // raise item added event 
            if (ItemEquipped != null)
                ItemEquipped(this, new InventoryEventArgs(item));
        }
        catch(ArgumentOutOfRangeException e){

        }
        catch(Exception e){
            
        }
    }
}
