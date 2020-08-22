using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    
    private InvItem item{get;set;}
    private Transform returnParent;
    private int siblingIndex;

    public InvItem getItem(){return item;}
    public void setItem(InvItem newItem){item = newItem;    }
    public void setParent(Transform newParent){
        returnParent = newParent;}
    public Transform getParent(){return returnParent;}
    public void OnBeginDrag(PointerEventData eventData){
        // make the object stop blocking raycasts while dragging
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        setParent(this.transform.parent); // == border
        hideInRoom();
        Transform slot = this.transform.parent.parent; 
        siblingIndex = slot.GetSiblingIndex(); // 
        slot.SetAsLastSibling();
        slot.parent.SetAsLastSibling(); // make other inventory behind current inventory
    }

    public void OnDrag(PointerEventData eventData){
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData){
        this.transform.parent = getParent();
        this.transform.parent.parent.SetSiblingIndex(siblingIndex);
        transform.localPosition = Vector3.zero;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        
    }


    public void showInRoom() {
        PhotonView photonView = PhotonView.Get(this);
        if (photonView != null)
        {
            photonView.RPC("showItem", RpcTarget.Others, item.Name);
        }
    }
    private void hideInRoom() {

        PhotonView photonView = PhotonView.Get(this);
        if (photonView != null)
        {
            photonView.RPC("hideItem", RpcTarget.Others, item.Name);
        }
    }

    /**
     * Hides and makes the items with the same name unclickable for all players in the room
     */
    [PunRPC]
    private void hideItem(string itemName) {
        if (item.name == itemName)
        {
            GetComponent<Image>().enabled = false;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

    }


    /**
     * Showes and makes the items with the same name unclickable for all players in the room
     */
    [PunRPC]
    private void showItem(string itemName) {
        if (item.name == itemName) {
            GetComponent<Image>().enabled = true;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
    
}
