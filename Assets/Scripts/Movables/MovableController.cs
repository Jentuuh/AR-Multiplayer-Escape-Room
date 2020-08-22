using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* @author Jente Vandersanden
*  This class controls what needs to happen regarding to movable objects when there's touch input from the user.
*/
public class MovableController : MonoBehaviour
{

    [SerializeField]
    private Camera first_person_camera;

    [SerializeField]
    private int triggerRange;

    private GameObject currently_moving;

    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

        /* Get touch position from screen if one touch is active */
        Touch touch;
        if (Input.touchCount > 0){
            touch = Input.GetTouch(0);
            switch(touch.phase){
                // TOUCH BEGIN
                case TouchPhase.Began:
                    /* Send out a ray from touch position */
                    Ray ray = first_person_camera.ScreenPointToRay(touch.position);
                    /* Gets the first hit from the ray */
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit)) 
                        {
                            // Check if we're in range of the movable object
                            if(hit.distance < triggerRange){

                                // We have a new currently moving gameobject
                                currently_moving = hit.collider.gameObject;
                                // Handle what to do with this gameobject
                                handleTouch(hit.collider.gameObject.GetComponent<MovableScript>());
                            }
                        }
                    break;

                // TOUCH END
                case TouchPhase.Ended:
                    MovableScript movable_script = currently_moving.GetComponent<MovableScript>();
                    if(movable_script != null){
                        // The movable object is now no longer grabbed
                        movable_script.setGrabbed(false);
                    }
                    // There's no object atm that's moving (because there's no touch)
                    currently_moving = null;
                    break;
            }
        }


        
    }


    // Is called when the wardrobe in the scene gets hit by a raycast
   private void handleTouch(MovableScript movable){
        Debug.Log("I was touched!");

        if(movable == null){
            return;
        }
        else{
            // The movable object is now grabbed
            movable.setGrabbed(true);
        }
    }
}
