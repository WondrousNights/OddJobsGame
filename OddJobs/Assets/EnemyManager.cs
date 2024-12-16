using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public bool isHostile;
    [SerializeField] bool hasConverstaion;

    NPC_Interactable interactable;

    void Start()
    {
        if(hasConverstaion)
        {
            interactable = GetComponent<NPC_Interactable>();
        }
    }

    public void SetHostile()
    {
        isHostile = true;

        if(hasConverstaion)
        {
            interactable.canInteract = false;
            interactable.conversationManager.EndConversation();
        }
    }


    public void SetNotHostile()
    {
        isHostile = false;
    }
}
