using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    [SerializeField]
    private InvItem item;

    [SerializeField]
    private Inventory inventory;
    private bool first = true;

    void Update() {
        if (first) {
            //Debug.Log("First: " + first.ToString());
            first = false;
            //Debug.Log("Test: add Item");
            //inventory.addItem(item);
        }
    }
}
