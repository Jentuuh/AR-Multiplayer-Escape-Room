using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @author Joris Bertram
 */
public class HandController : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;
    
    [SerializeField]
    private GameObject playerHand;

    void Start()
    {
        inventory.ItemEquipped += onEquip;
    }

    private void onEquip(object sender, InventoryEventArgs e) {
        clearHand();
        // If item exists change playerHand
        if (e.Item != null)
            putInHand(e.Item.InHandVisualization);
    }

    private void clearHand() {
        foreach (Transform child in playerHand.transform) {
            Debug.Log("Clear hand of object");
            child.transform.localPosition = new Vector3(0, 10000, 0);
            GameObject.Destroy(child.gameObject, 1);
        }     
    }
    
    private void putInHand(GameObject inHandVisualization) {
        if (inHandVisualization == null)
            return;

        GameObject item = Instantiate(inHandVisualization, Vector3.zero, playerHand.transform.rotation);
        item.transform.parent = playerHand.transform;
        item.transform.localPosition = Vector3.zero;
    }
}
