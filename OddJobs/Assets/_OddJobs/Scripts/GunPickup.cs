using UnityEngine;

public class GunPickup : Interactable
{

  [SerializeField] GunScriptableObject gun;
  protected override void Interact(Local_PlayerInteractionManager playerInteracting)
    {
        playerInteracting.gunHandler.PickupGun(gun);
        Destroy(this.gameObject);
    }   
}
