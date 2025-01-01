using Unity.Netcode;
using UnityEngine;

public class Network_ItemPickup : Network_Interactable
{
    [SerializeField] Network_GunScriptableObject gun;
    [SerializeField] private bool autoPickup = false;
    public int ammoInClip = -1;
    [Tooltip("Set to -1 to use the gun's default ammo clip size")]
    // [SerializeField] private int timeToRespawn = -1;
    // [Tooltip("Set to -1 to never respawn")]

    
    protected override void Interact(Network_PlayerInteractionManager playerInteracting)
    {
        playerInteracting.GetComponent<Network_PlayerGunHandler>().PickupGun(gun, gameObject);
    }

    [Rpc(SendTo.Everyone, RequireOwnership = false)]
    public void DestoryItemRpc()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (autoPickup && collision.gameObject.GetComponent<Network_PlayerInteractionManager>())
            Interact(collision.gameObject.GetComponent<Network_PlayerInteractionManager>());
    }

    private void Start() {
        if (ammoInClip == -1) {
            ammoInClip = gun.AmmoClipSize;
        }
    }
}