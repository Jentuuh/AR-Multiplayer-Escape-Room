using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;
[RequireComponent(typeof(PhotonView))]

public class ChatManager : MonoBehaviourPunCallbacks, IPunObservable {
    private struct Message {
        public Color color;
        public string message;
    }


    [SerializeField]
    private Rect GuiRect = new Rect(0, 0, 250, 300);
    
    [SerializeField]
    private bool multiplayer = false; 
    
    [SerializeField]
    private bool AlignBottom = false;

    [SerializeField]
    private int textSize = 24;

    [SerializeField]
    private Color textColor = Color.white;

    private List<Message> messages = new List<Message>();
    private string inputLine = "";
    private Vector2 scrollPos = Vector2.zero;

    public static readonly string ChatRPC = "Chat";

    public void Start() {
        if (this.AlignBottom)
            this.GuiRect.y = Screen.height - this.GuiRect.height;


        gameObject.SetActive(false);
    }

    public void OnGUI()
    {

        /* If 'Enter' is pressed */
        if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return)) {
            if (!string.IsNullOrEmpty(this.inputLine)) {
                if (multiplayer)
                    this.photonView.RPC("Chat", RpcTarget.AllViaServer, this.inputLine);
                else
                    AddLine(this.inputLine);
                this.inputLine = "";
                GUI.FocusControl("");
                return; // printing the now modified list would result in an error. to avoid this, we just skip this single frame
            }
            else
                GUI.FocusControl("ChatInput");
        }

        GUIStyle TextStyle = new GUIStyle();
        TextStyle.fontSize = textSize;
        TextStyle.normal.background = MakeText(1, 1, new Color(0.0f, 0.0f, 0.0f, 0.6f));
        TextStyle.wordWrap = true;

        //TextStyle.normal.background = Color.yellow;


        GUIStyle InputStyle = new GUIStyle();
        InputStyle.fontSize = textSize;
        InputStyle.normal.textColor = Color.white;
        InputStyle.normal.background = MakeText(1, 1, new Color(0.0f, 0.0f, 0.0f, 0.6f));
        InputStyle.padding = new RectOffset(5, 5, 5, 5); 

        GUI.SetNextControlName("");
        GUILayout.BeginArea(this.GuiRect);
        

        scrollPos = GUILayout.BeginScrollView(scrollPos);
        GUILayout.FlexibleSpace();
        //for (int i = messages.Count - 1; i >= 0; i--)
        //{
        // GUILayout.Label(messages[i]);
        //}

        /* Put labels of the last 10 messages in the chat */
        for (int i = 0; i < messages.Count; i++) {
            TextStyle.normal.textColor = messages[i].color;
            GUILayout.Label(messages[i].message, TextStyle);

            if (messages.Count >= 10)
                this.messages.RemoveAt(0);
        }

        GUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        GUI.SetNextControlName("ChatInput");
        GUI.backgroundColor = Color.yellow;
        inputLine = GUILayout.TextField(inputLine, InputStyle);

        GUILayout.Space(40.0f);

        if (GUILayout.Button("Send", InputStyle, GUILayout.ExpandWidth(false))) {
            if (inputLine != "")
            {
                if (multiplayer)
                    this.photonView.RPC("Chat", RpcTarget.AllViaServer, this.inputLine);
                else
                    AddLine(this.inputLine);
                this.inputLine = "";
                GUI.FocusControl("");
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    [PunRPC]
    public void Chat(string newLine, PhotonMessageInfo mi) {
        string senderName = "anonymous";

        if (mi.Sender != null) {
            if (!string.IsNullOrEmpty(mi.Sender.NickName))
                senderName = mi.Sender.NickName;
            else
                senderName = "player " + mi.Sender.NickName;
        }

        Message message = new Message();
        message.message = senderName + ": " + newLine;
        message.color = textColor;

        this.messages.Add(message);
    }

    [PunRPC]
    public void Broadcast(string broadcast) {
        AddLine(broadcast);
    }

    public void AddLine(string newLine){
        Message message = new Message();
        message.message = newLine;
        message.color = textColor;

        this.messages.Add(message);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        throw new System.NotImplementedException();
    }

    public void SendChatBotMessage(string newLine, string botName, Color color) {
        Debug.Log("Sending Chatmessage with color: " + color.ToString());

        Message message = new Message();
        message.message = botName + ": " + newLine;
        message.color = color;

        this.messages.Add(message);
    }
    
    private Texture2D MakeText(int width, int height, Color col)
    {
        Color[] pix = new Color[width*height];

        for(int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    public void SendChatMessage(string newLine, Color color) {
        Debug.Log("Sending ChatMessage with color: " + color.ToString());

        Message message = new Message();
        message.message = "[BOT]:" + newLine;
        message.color = color;

        this.messages.Add(message);
    }

    public void setVisible(bool visible) {
        gameObject.SetActive(visible);
    }

}