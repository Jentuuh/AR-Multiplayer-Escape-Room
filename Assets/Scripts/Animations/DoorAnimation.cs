using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* @author jentevandersanden
* Controller for the door's animation
*/
public class DoorAnimation : MonoBehaviour, AnimationController
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Interface method from interface AnimationController
    void AnimationController.setTrigger(){
        animator.SetTrigger("LockerOpen");
    }

 
}
