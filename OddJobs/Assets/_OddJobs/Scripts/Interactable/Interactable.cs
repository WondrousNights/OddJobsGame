//using AlmenaraGames;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string promptMessage;
    public bool isUI;

    //public AudioObject sfxToPlay;
    
    public void BaseInteract(Local_PlayerInteractionManager playerInteracting)
    {
        /*
        if(sfxToPlay != null)
        {
            //MultiAudioManager.PlayAudioObject(sfxToPlay, this.transform);
        }
        */
        Interact(playerInteracting);

    }

    protected virtual void Interact(Local_PlayerInteractionManager playerInteracting)
    {

    }
}
