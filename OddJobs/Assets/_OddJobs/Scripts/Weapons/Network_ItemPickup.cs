using Unity.Netcode;
using UnityEngine;

public class Network_ItemPickup : Network_Interactable
{

    [SerializeField] bool isWeapon;
    [SerializeField] bool isAmmo;
    [SerializeField] bool isObjective;

    [SerializeField] Network_GunScriptableObject gun;
    [SerializeField] AmmoType ammoType;
    [SerializeField] int amountOfAmmo;
    [SerializeField] private bool autoPickup = false;
    public int ammoInClip = -1;
    [Tooltip("Set to -1 to use the gun's default ammo clip size")]
    // [SerializeField] private int timeToRespawn = -1;
    // [Tooltip("Set to -1 to never respawn")]

    bool isPickedUp = false;
    [SerializeField] Network_LevelManager levelManager;




    protected override void Interact(PlayerManager playerManager)
    {
        if(isWeapon)
        {
            if(isPickedUp) return;
            //playerInteracting.GetComponent<Network_PlayerGunHandler>().PickupGun(gun, gameObject);
            isPickedUp = true;
        }
        if(isAmmo)
        {
            //playerInteracting.ammoHandler.AddAmmo(ammoType, amountOfAmmo);
            DestoryItemRpc();
        }
        if(isObjective)
        {
            levelManager.AddWaterRpc();
            DestoryItemRpc();
        }
        
    }

    [Rpc(SendTo.Everyone, RequireOwnership = false)]
    public void DestoryItemRpc()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (autoPickup && collision.gameObject.GetComponent<Network_PlayerInteractionManager>())
            //Interact(collision.gameObject.GetComponent<Network_PlayerInteractionManager>());
    }

    private void Start() {

        if(isWeapon)
        {
            if (ammoInClip == -1) {
            ammoInClip = gun.ClipSize;
            }
        }
     
    }

   
}