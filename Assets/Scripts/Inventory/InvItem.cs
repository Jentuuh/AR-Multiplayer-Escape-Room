 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Class wich represent an item in the inventory
 * @author Tim
*/
public class InvItem : MonoBehaviour
{    
    [SerializeField]
    private string _Name = null;

    [SerializeField]
    private Sprite _Image = null;

    [SerializeField]
    private string _PrefabName = null;

    [SerializeField]
    private GameObject _InHandVisualization = null;

    public string Name {
        get 
        { 
            return _Name;
        }
        set{
            _Name = value;
        }
    }
    public Sprite Image {
        get 
        {
            return _Image;
        }
        set{
            _Image = value;
        }
    }

    public String PrefabName {
        get 
        {
            return _PrefabName;
        }
        set{
            _PrefabName = value;
        }
    }

    public GameObject InHandVisualization {
        get 
        {
            return _InHandVisualization;
        }
        set{
            _InHandVisualization = value;
        }
    }

    /**
     * Method that gets called when item is picked up
     */ 
    public void onPickup()
    {
        gameObject.SetActive(false); // disable object when picked up
    }


    /**
     * Method that gets callded when item is equipped
     */
     public void onEquip() {
        
     }

     
    /**
     * Method that gets called when item is unequipped
     */
     public void onUnequip() {
        
     }
}

/**
 * Wrapper class for the data that gets passed with an event
 */
public class InventoryEventArgs : System.EventArgs
{
    public InvItem Item;

    public InventoryEventArgs(InvItem item) {
        Item = item;
    }
}