using UnityEngine;

public class Network_ItemPickup : Network_Interactable
{
    [SerializeField] GunScriptableObject gun;
    [SerializeField] private bool autoPickup = false;
    public int ammoInClip = -1;
    [Tooltip("Set to -1 to use the gun's default ammo clip size")]
    // [SerializeField] private int timeToRespawn = -1;
    // [Tooltip("Set to -1 to never respawn")]

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