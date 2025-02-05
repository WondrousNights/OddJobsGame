//using AlmenaraGames;
using Unity.Netcode;
using UnityEngine;

public class Network_Interactable : NetworkBehaviour
{
    public string promptMessage;
    public bool isUI;

    public AudioSource sfxToPlay;
    
    public void BaseInteract(PlayerManager player)
    {
        /*
         if(sfxToPlay != null)
        {
            sfxToPlay.Play();
        }
        */
        Interact(player);

    }

    protected virtual void Interact(PlayerManager player)
    {

    }
}
