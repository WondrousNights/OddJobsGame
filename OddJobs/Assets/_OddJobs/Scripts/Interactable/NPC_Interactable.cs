using UnityEngine;
using DialogueEditor;
using Unity.VisualScripting;

// public enum DialogueCanvasType

// {
//     WorldSpace,
//     CameraSpace, 
//     ScreenSpace
// }

public class NPC_Interactable : Interactable
{

    public ConversationManager conversationManager;
    NPCConversation conversation;

    [SerializeField] BoxCollider uiBoxCollider;
    Collider npcCollider;

    bool checkForPlayer;
    [SerializeField] float checkForPlayerRadius;
    [SerializeField] LayerMask playerLayerMask;

    public bool canInteract = true;

    
    void Start()
    {
        conversationManager = GetComponentInChildren<ConversationManager>();
        conversation = GetComponent<NPCConversation>();
        npcCollider = GetComponent<Collider>();

        //conversationManager.OnConversationStarted += ConversationStart;
        //conversationManager.OnConversationEnded += ConversationEnd;

        if (uiBoxCollider) uiBoxCollider.enabled = false;
    }
    protected override void Interact(Local_PlayerInteractionManager playerInteracting)
    {
        if(canInteract)
        {
        conversationManager.StartConversation(conversation);
        }
        // Debug.Log("Interacted with" + gameObject.name);
        

    }

    private void ConversationStart()
    {
        checkForPlayer = true;
        if (uiBoxCollider) uiBoxCollider.enabled = true;
        npcCollider.enabled = false;
    }
    public void ConversationEnd()
    {
        checkForPlayer = false;
        if (uiBoxCollider) uiBoxCollider.enabled = false;
        npcCollider.enabled = true;

        
    }

    void Update()
    {
        if(!checkForPlayer) return;

        if(Physics.CheckSphere(transform.position, checkForPlayerRadius, playerLayerMask) == false)
        {
            conversationManager.EndConversation();
        }
    }

    
}
