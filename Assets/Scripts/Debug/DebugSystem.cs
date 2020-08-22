using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugSystem : MonoBehaviour
{   
    [SerializeField]
    private GameObject chatPanel, textObject;
    List<DebugMessage> messageList = new List<DebugMessage>();
    public void log(string text) {
        // Text
        DebugMessage newMessage = new DebugMessage();
        newMessage.text = text;
        
        // UI Text
        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;

        messageList.Add(newMessage);
    }
}

[System.Serializable]
public class DebugMessage {
    public string text;
    public Text textObject;
}
