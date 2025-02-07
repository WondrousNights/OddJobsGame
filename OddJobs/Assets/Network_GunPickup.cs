using UnityEngine;

public class Network_GunPickup : Network_ItemPickup
{
    [SerializeField] Network_WeaponProperties weaponProperties;


    protected override void Interact(PlayerManager playerManager)
    {
        playerManager.weaponInventory.PickupWeapon(weaponProperties, ammoInClip);
      
        base.Interact(playerManager);
        DestoryItemRpc();
    }
}
