using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
* @author Jente Vandersanden
*  This class tells a movable object what to do when it gets triggered by the MovableController
*/

public class MovableScript : MonoBehaviour
{
    [SerializeField]
    private GameObject object_to_move;

    [SerializeField]
    private PuzzleController puzzle_controller;

    [SerializeField]
    private bool horizontalMovement;

    [SerializeField]
    private bool verticalMovement;
    private bool isGrabbed;

    [SerializeField]
    private float slowpushfactor = 0.01f;
    

    // Start is called before the first frame update
    void Start()
    {
        isGrabbed = false;   
    }

    // Update is called once per frame
    void Update()
    {
        // If we're grabbing the closet and we're in range, and the puzzle is not solved yet
        if(isGrabbed && !puzzle_controller.isSolved()){
            // Horizontal moving objects
            // Get difference in position between closet and camera, then move the closet along it's x-axis as far as that difference.
            if(horizontalMovement){
                object_to_move.transform.Translate(Vector3.right * (-(object_to_move.transform.position.x - Camera.current.transform.position.x)) * slowpushfactor);
            }
            // Vertical moving objects
            if(verticalMovement){
                object_to_move.transform.Translate(Vector3.up * (Mathf.Abs(object_to_move.transform.position.y - Camera.current.transform.position.y)) * slowpushfactor);
            }
        }
    }

    public void setGrabbed(bool value){
        isGrabbed = value;
    }
}
