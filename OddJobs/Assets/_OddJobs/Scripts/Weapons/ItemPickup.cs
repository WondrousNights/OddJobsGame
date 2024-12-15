using UnityEngine;

public class ItemPickup : Interactable
{

    [SerializeField] private bool autoPickup = false;
    [SerializeField] GunScriptableObject gun;
    public int ammoInClip = -1;
    [Tooltip("Set to -1 to use the gun's default ammo clip size")]

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

    private void Start() {
        if (ammoInClip == -1) {
            ammoInClip = gun.AmmoClipSize;
        }
    }
}