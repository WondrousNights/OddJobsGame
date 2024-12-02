using UnityEngine;
using DialogueEditor;
public class NPC_Interactable : Interactable
{

    ConversationManager conversationManager;
    NPCConversation conversation;

    void Start()
    {
        conversationManager = GetComponentInChildren<ConversationManager>();
        conversation = GetComponent<NPCConversation>();
    }
    protected override void Interact()
    {
        Debug.Log("Interacted with" + gameObject.name);
        conversationManager.StartConversation(conversation);
    }
}
