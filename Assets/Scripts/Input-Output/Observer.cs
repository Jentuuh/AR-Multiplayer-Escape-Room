using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Observer interface from Observer pattern
 * @author Joris Bertram
 */
public abstract class Observer : MonoBehaviour
{
    public abstract void OnNotify();
}