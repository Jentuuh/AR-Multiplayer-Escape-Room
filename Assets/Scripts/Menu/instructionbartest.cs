using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instructionbartest : MonoBehaviour
{


    [SerializeField]
    InstructionController controller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void showBar() {
        controller.setInstructionBar("This is a very helpfull hint that you might need", 0);
    }

}
