using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* @author jentevandersanden
* Controller for the chest's animation
*/
public class ChestAnimator : MonoBehaviour, AnimationController
{

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void AnimationController.setTrigger(){
        animator.SetTrigger("OpenChest");
    }
}
