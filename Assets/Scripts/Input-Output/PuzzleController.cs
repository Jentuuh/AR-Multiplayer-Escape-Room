using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class handles the puzzle's state
 * @author Joris Bertram
 */

public class PuzzleController : MonoBehaviour
{            
    [SerializeField]
    private bool isLastPuzzle = false;

    [SerializeField]
    private bool canSolve = true;
   
    [SerializeField]
    private bool inputAND = false;

    [SerializeField]
    private GameObject[] inputs;

    [SerializeField]
    private GameObject[] outputs;

    [SerializeField]
    private GameObject animated_object;

    

    private bool[] inputState;

    private bool solved = false;

    void Start() {
        inputState = new bool[inputs.Length];

        for (int i = 0; i < inputs.Length; i++) {
            inputs[i].GetComponent<InputScript>().setController(this);
            inputs[i].GetComponent<InputScript>().setID(i);
        }
    }

    void Update() {
        // If already solved and can solve don't check anymore
        if (canSolve && isSolved())
            return;

        // All inputs need to be correct
        if (inputAND) 
        {
            for (int i = 0; i < inputs.Length; i++) {
                if (!inputState[i] && !canSolve)
                    if(solved) setSolved(false);
                if (!inputState[i])
                    return;
            }
            if(!solved) setSolved(true);
        }
        // One input needs to be correct
        else
        {
            for (int i = 0; i < inputs.Length; i++) {
                if (inputState[i]) {
                    if(!solved) setSolved(true);
                    return;
                }
            }
            if(solved) setSolved(false);           
        }
    }
    public bool getInputState(int id) {
        return inputState[id];
    }
    public void setInputState(int id, bool state) {
        inputState[id] = state;
    }

    public bool isSolved() {
        if (canSolve)
            return solved;
        else
            return false;
    } 

    public void setSolved(bool solved) {
        Debug.Log("Puzzle solved: " + solved);
        this.solved = solved;
        for (int i = 0; i < outputs.Length; i++) {
            if (solved)
                outputs[i].GetComponent<OutputScript>().doOutput();
            else
                outputs[i].GetComponent<OutputScript>().destroy();
        }

        // Do the animation
        if(animated_object != null){
            animated_object.GetComponent<AnimationController>().setTrigger();
        }
            

        // End the game if it's the last puzzle
        if(solved && isLastPuzzle){
            GameObject.Find("GameController").GetComponent<GameController>().lastPuzzleSolved();
        }
    }
}
