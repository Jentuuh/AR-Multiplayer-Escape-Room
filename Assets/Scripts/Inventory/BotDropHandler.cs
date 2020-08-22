using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;

public class BotDropHandler : DropHandler
{
    [SerializeField]
    private List<InvItem> requiredItems;

    [SerializeField]
    private ChatManager chat;



    private void Start()
    {
        startMessage();
    }

    public override void OnDrop(PointerEventData eventData)
    {
        DragHandler dragItem = eventData.pointerDrag.GetComponent<DragHandler>();
        if (dragItem != null)
        {
            InvItem item = dragItem.getItem();
            if (item != null)
            {
                if (isRequiredItem(item))
                {
                    sendRightItemMessage();
                    Debug.Log("BotDropHandler: DraggedItem == RequiredItem");
                    base.OnDrop(eventData);
                }
                else {
                    sendWrongItemMessage();
                   
                }
            }


        }
    }

    private void startMessage() {

        //TODO adjust text to fit tutorial story
        chat.AddLine("-- Hello new User --");
        chat.AddLine("I'll help you get through this problem right here.");
        chat.AddLine("The problem right now is that i dont have any food, ");
        chat.AddLine("coincidentally you have a carrot that i would like.");
        chat.AddLine("If you'd be so kind to drop it in the vertical inventory, ");
        chat.AddLine("I'll give you a compliment!");
    }

    private void sendRightItemMessage()
    {
        chat.AddLine("-- You' ve Completed the task --");
        chat.AddLine("Thank you so much for that carrot, ");
        chat.AddLine("U have a good heart mr.Mysterious.");

    }

    private void sendWrongItemMessage()
    {
        chat.AddLine("-- uh oh, someting went wrong --");
        chat.AddLine("It seems u gave me the wrong item");

    }

    private bool isRequiredItem(InvItem item) {
        foreach (InvItem newitem in requiredItems)
        {
            if (newitem.Name.Equals(item.Name))
            {
                return true;
            }
        }
        return false;
    }


}
