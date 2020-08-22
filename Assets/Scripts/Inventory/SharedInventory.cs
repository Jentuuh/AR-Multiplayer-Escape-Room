using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedInventory : AbstractInventory
{
    void Start()
    {
        SlotCount = 5;
        initialize();
    }
}
