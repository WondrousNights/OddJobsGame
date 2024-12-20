using UnityEngine;

public class GrenadePickup : Interactable
{
    protected override void Interact(Local_PlayerInteractionManager playerInteracting)
    {
        playerInteracting.playerInputController.grenadeHandler.grenadeCount += 1;
        Destroy(this.gameObject);
    }   
}
