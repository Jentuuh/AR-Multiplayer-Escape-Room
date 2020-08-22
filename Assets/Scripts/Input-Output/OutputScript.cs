using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class handles the output of a puzzel
 * @author Joris Bertram
 */

public class OutputScript : MonoBehaviour
{
    [SerializeField]
    private bool placeGameObject;

    [SerializeField]
    private bool sendChatMessage;

    [SerializeField]
    private GameObject placeObject;
    
    [SerializeField]
    private string message;

    [SerializeField]
    private Color messageColor;

    public void doOutput() {
        if (placeGameObject) {
            Debug.Log("Place output object");
            GameObject item = Instantiate(placeObject, Vector3.zero, gameObject.transform.rotation);
            item.transform.parent = transform;
            item.transform.localPosition = Vector3.zero;
        }
        else if (sendChatMessage) {
            Debug.Log("Send: '" + message + "' to Chat");
            ChatManager chat = Resources.FindObjectsOfTypeAll<ChatManager>()[0];
            chat.setVisible(true);
            chat.SendChatMessage(message, Color.green);
        }
    }

    public void destroy() {
        Debug.Log("Destroy output object");
        foreach (Transform child in transform)
            GameObject.Destroy(child.gameObject);
    }
}