using DialogueEditor;
using UnityEngine;

public class CheckGoalCompletion : MonoBehaviour
{
    [SerializeField] bool ObjectDelivery;

    [SerializeField] LayerMask checkMask;

    public string dialogueConditionName;

    NPC_Interactable nPC_Interactable;
    void Start()
    {
        nPC_Interactable = GetComponent<NPC_Interactable>();
    }
    public void CheckForGoalCompletion()
    {
        if(ObjectDelivery)
        {
            
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, checkMask);

            if (colliders.Length > 0)
            {
                foreach(Collider col in colliders)
                {
                    if(col.gameObject.tag == "Goal")
                    {
                        nPC_Interactable.conversationManager.SetBool(dialogueConditionName, true);
                    }
                }
            }
    
        }
    }
}
