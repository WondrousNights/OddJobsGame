using DialogueEditor;
using UnityEngine;

public class Network_NPCInteractable : Network_Interactable
{

    [SerializeField] NPCConversation myConversation;

    void Start()
    {
        if(myConversation == null)
        {
            myConversation = GetComponent<NPCConversation>();
        }
        
    }
        
    protected override void Interact(Network_PlayerInteractionManager playerInteracting)
    {

        if(ConversationManager.Instance == null)
        {
            Debug.Log("Conversation Manager Instance is Null");
        }
        else
        {
        ConversationManager.Instance.StartConversation(myConversation);
        }

       
    }

}
