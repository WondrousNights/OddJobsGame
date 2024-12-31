using AlmenaraGames;
using Unity.Netcode;
using UnityEngine;

public class Network_Interactable : NetworkBehaviour
{
    public string promptMessage;
    public bool isUI;

    public AudioSource sfxToPlay;
    
    public void BaseInteract(Local_PlayerInteractionManager playerInteracting)
    {
         if(sfxToPlay != null)
        {
            sfxToPlay.Play();
        }
        Interact(playerInteracting);

    }

    protected virtual void Interact(Local_PlayerInteractionManager playerInteracting)
    {

    }
}
