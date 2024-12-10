using UnityEngine;

public class ItemPickup : Interactable
{

    [SerializeField] private bool autoPickup = false;
    [SerializeField] GunScriptableObject gun;
    protected override void Interact(Local_PlayerInteractionManager playerInteracting)
    {
        playerInteracting.GetComponent<PlayerGunHandler>().PickupGun(gun, gameObject);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (autoPickup && collision.gameObject.GetComponent<Local_PlayerInteractionManager>())
            Interact(collision.gameObject.GetComponent<Local_PlayerInteractionManager>());
    }
}