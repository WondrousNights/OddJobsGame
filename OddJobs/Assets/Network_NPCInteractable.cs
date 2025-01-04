using DialogueEditor;
using UnityEngine;

public class Network_NPCInteractable : Network_Interactable
{

    NPCConversation myConversation;

    void Start()
    {
        myConversation = GetComponent<NPCConversation>();
    }
        
    protected override void Interact(Network_PlayerInteractionManager playerInteracting)
    {
       ConversationManager.Instance.StartConversation(myConversation);

    }

}
