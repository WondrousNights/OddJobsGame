using UnityEngine;

public class AmmoPickup : Interactable
{

    [SerializeField] AmmoType ammoType;
    [SerializeField] int amount;
    protected override void Interact(Local_PlayerInteractionManager playerInteracting)
    {
        playerInteracting.ammoHandler.AddAmmo(ammoType, amount);
        Destroy(this.gameObject);
    }   
}
