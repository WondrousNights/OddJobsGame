using Unity.Netcode;
using UnityEngine;

public class Network_PlayerWeaponHandler : NetworkBehaviour
{
    Network_PlayerInputController inputController;
    Network_WeaponInventory weaponInventory;
    PlayerManager playerManager;

    [SerializeField] Transform shootpoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputController = GetComponent<Network_PlayerInputController>();
        weaponInventory = GetComponent<Network_WeaponInventory>();
        playerManager = GetComponent<PlayerManager>();

        inputController.onFoot.Shoot.performed += ctx => Fire();
        inputController.onFoot.Reload.performed += ctx => Reload();
    }

    void Fire()
    {
        if(!IsOwner) return;
        if(playerManager.currentPlayerState == PlayerManager.PlayerState.InMenu) return;   

        Weapon currentWeapon = weaponInventory.GetCurrentWeapon();
        if(currentWeapon != null)
        {
            Ray ray = new Ray(shootpoint.transform.position, shootpoint.transform.forward);
            if(currentWeapon.ammoInClip > 0)
            {
                if (Time.time > currentWeapon.weaponProperties.fireRate + currentWeapon.LastShootTime)
                {
                    currentWeapon.UseWeapon(ray);
                    currentWeapon.ShootEffects();
                    weaponInventory.UpdateAmmoText();
                    FireRpc();
                }
            }
            
            
        }
    }

    [Rpc(SendTo.Everyone)]
    void FireRpc()
    {
        Weapon visualWeapon = weaponInventory.GetCurrentVisualWeapon();
        visualWeapon.ShootEffects();

    }

    void Reload()
    {
        Weapon currentWeapon = weaponInventory.GetCurrentWeapon();
        if(currentWeapon != null)
        {
            int ammoToReload = weaponInventory.ammoHandler.AmmoToReload(currentWeapon.weaponProperties.AmmoType, currentWeapon.ammoInClip, currentWeapon.weaponProperties.ClipSize);
            currentWeapon.Reload(ammoToReload);
            weaponInventory.UpdateAmmoText();
        }
    }

    void ReloadRpc()
    {
        Weapon visualWeapon = weaponInventory.GetCurrentVisualWeapon();
        if(visualWeapon != null)
        {
            //Need to change this to reload effects
            visualWeapon.ReloadEffects();
            
        }
    }
}
