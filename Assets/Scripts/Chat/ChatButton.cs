using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatButton : MonoBehaviour
{

    [SerializeField]
    private GameObject Chat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buttonTapped() {
        if (Chat != null)
        {
            Chat.SetActive(!Chat.activeSelf); // if chat is closed, open chat. if chat is open, close chat

        }
    }
}
