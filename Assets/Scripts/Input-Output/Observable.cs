using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Observable class from Observer pattern
 * @author Joris Bertram
 */
public class Observable : MonoBehaviour
{

    [SerializeField]
    protected GameObject[] observers;

    //Send notifications if something has happened
    public void Notify() {
        OutputScript currentScript;
        for (int i = 0; i < observers.Length; i++) {
            currentScript = observers[i].GetComponent<OutputScript>();
            // Checking has script
            //if (currentScript != null)
               //currentScript.OnNotify();
        }
            
    }


}