using AlmenaraGames;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isUI; // wtf does this do, need tooltip. is what UI? the prompt message? what UI where?
    public string promptMessage;

    public AudioObject sfxToPlay;
    
    public void BaseInteract(Local_PlayerInteractionManager playerInteracting)
    {
         if(sfxToPlay != null)
        {
            MultiAudioManager.PlayAudioObject(sfxToPlay, this.transform);
        }
        Interact(playerInteracting);

    }

    protected virtual void Interact(Local_PlayerInteractionManager playerInteracting)
    {

    }
}
