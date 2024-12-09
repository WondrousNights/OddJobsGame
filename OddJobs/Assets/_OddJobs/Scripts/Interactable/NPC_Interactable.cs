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

    ConversationManager conversationManager;
    NPCConversation conversation;

    [SerializeField] BoxCollider uiBoxCollider;
    BoxCollider npcBoxCollider;

    bool checkForPlayer;
    [SerializeField] float checkForPlayerRadius;
    [SerializeField] LayerMask playerLayerMask;

    void Start()
    {
        conversationManager = GetComponentInChildren<ConversationManager>();
        conversation = GetComponent<NPCConversation>();
        npcBoxCollider = GetComponent<BoxCollider>();

        conversationManager.OnConversationStarted += ConversationStart;
        conversationManager.OnConversationEnded += ConversationEnd;

        if (uiBoxCollider) uiBoxCollider.enabled = false;
    }
    protected override void Interact(Local_PlayerInteractionManager playerInteracting)
    {
        // Debug.Log("Interacted with" + gameObject.name);
        conversationManager.StartConversation(conversation);

    }

    private void ConversationStart()
    {
        checkForPlayer = true;
        if (uiBoxCollider) uiBoxCollider.enabled = true;
        npcBoxCollider.enabled = false;
    }
    private void ConversationEnd()
    {
        checkForPlayer = false;
        if (uiBoxCollider) uiBoxCollider.enabled = false;
        npcBoxCollider.enabled = true;
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
