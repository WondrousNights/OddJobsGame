using UnityEngine;

public class AmmoPickup : Interactable
{

    [SerializeField] AmmoType ammoType;
    [SerializeField] int amount;
    protected override void Interact(Local_PlayerInteractionManager playerInteracting)
    {
        playerInteracting.ammoHandler.AddAmmo(ammoType, amount);
        playerInteracting.ammoHandler.UpdateAmmoText(playerInteracting.gunHandler.currentGunIndex, ammoType);
        Destroy(this.gameObject);
    }   
}
