using UnityEngine;
using DialogueEditor;
public class NPCUI_Interactable : Interactable
{
     ConversationManager conversationManager;

     void Start()
    {
        conversationManager = GetComponent<ConversationManager>();
    }

    public void SelectNextOption()
    {
        conversationManager.SelectNextOption();
    }
    public void SelectPreviousOption()
    {
        conversationManager.SelectPreviousOption();
    }
    public void PressSelectedOption()
    {
        conversationManager.PressSelectedOption();
    }
    
}
