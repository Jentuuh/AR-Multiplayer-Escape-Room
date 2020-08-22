using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using ExitGames.Client.Photon;

/**
 * Class that handles the logic when an item is released after dragging
 * @author Tim
 */
public class DropHandler : MonoBehaviour, IDropHandler
{
    [SerializeField]
    AbstractInventory inventory;

    public AbstractInventory getInventory() { return inventory; }
    /**
     * Method that gets called if item is released
     */
    public virtual void OnDrop(PointerEventData eventData)
    {
        Debug.Log("DropHandler: item dropped");
        handleDrop(eventData);
        

    }



    private void handleDrop(PointerEventData eventData) {
        RectTransform invPanel = transform as RectTransform; // panel of the inventory
        if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        { // if drop is inside panel
            DragHandler dragItem = eventData.pointerDrag.GetComponent<DragHandler>();
            if (dragItem != null)
            {
                AbstractInventory senderInv = dragItem.getParent().parent.parent.GetComponent<DropHandler>().getInventory(); // get original inventory
                dragItem.showInRoom();
                if (!isSameInventory(senderInv))
                { // if item is not being dragged to the same inventory
                    InvItem item = dragItem.getItem();

                    Debug.Log("DropHandler: removeItem " + item.Name + " from " + senderInv.name);
                    if (inventory is SharedInventory)
                    {
                        /*
                        photonView.RPC("notifyRoomAdded", RpcTarget.Others, item.PrefabName);
                        inventory.addItem(item);
                        senderInv.removeItem(item); // remove item from old
                        notifyRoomAdded(item.PrefabName);
                        dragItem.getParent().parent.parent.GetComponent<DropHandler>().notifyRoomRemoved(item.PrefabName);
                        */

                        senderInv.removeItem(item); // remove item from old
                                                    // notifyRoomAdded(item.PrefabName); // notify room that item was added to shared inv
                        callAddedRPC(item.PrefabName);

                        Debug.Log("Inventory -> Shared Inventory");

                    }

                    else
                    { // if inventory is a normal inventory and item is dragged from shared inventory

                        /* original 
                    photonView.RPC("notifyRoomRemoved", RpcTarget.Others, item.PrefabName);
                    inventory.addItem(item);
                    senderInv.removeItem(item);
                    */
                        DropHandler sharedInv = dragItem.getParent().parent.parent.GetComponent<DropHandler>();
                        sharedInv.callRemovedRPC(item.PrefabName);
                        Debug.Log("Shared inventory -> inventory");

                        inventory.addItem(item);
                        // dragItem.getParent().parent.parent.GetComponent<DropHandler>().notifyRoomRemoved(item.PrefabName);


                    }


                    //Debug.Log("DropHandler: addItem " + item.Name + " to " + inventory.name);
                    // inventory.addItem(item); // add to new inventory       
                }
            }
        }

    }

    public void callAddedRPC(string name)
    {
        PhotonView photonView = PhotonView.Get(this);
        if (photonView != null)
            photonView.RPC("notifyRoomAdded", RpcTarget.All, name);
    }

    public void callRemovedRPC(string name)
    {
        PhotonView photonView = PhotonView.Get(this);
        if (photonView != null)
            photonView.RPC("notifyRoomRemoved", RpcTarget.All, name);
    }



    /*
     * @returns true if item is dragged from and to the same inventory
     */
    private bool isSameInventory(AbstractInventory senderInv)
    {

        return senderInv == inventory;

    }


    [PunRPC]
    void notifyRoomAdded(string prefabName)
    {
        if (inventory is SharedInventory)
        {
            GameObject newItem = Resources.Load("Prefabs/items/" + prefabName) as GameObject;
            if (newItem != null)
            {
                inventory.addItem(newItem.GetComponent<InvItem>());
            }
        }
    }

    [PunRPC]
    void notifyRoomRemoved(string prefabName)
    {
        if (inventory is SharedInventory)
        { // if the receiver is an inventory and the item needs to be removed from the shared inventory
            GameObject newItem = Resources.Load("Prefabs/Items/" + prefabName) as GameObject;
            if (newItem != null)
            {
                if (inventory.removeItem(newItem.GetComponent<InvItem>()))
                {
                    Debug.Log("Item Removed from SHARED INVENTORY");
                }
                else
                {
                    Debug.Log("Item NOT Removed from SHARED INVENTORY");

                }
            }
        }
    }
}
