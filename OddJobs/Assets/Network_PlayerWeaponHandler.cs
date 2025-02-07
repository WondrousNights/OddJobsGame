using Unity.Netcode;
using UnityEngine;

public class Network_PlayerWeaponHandler : NetworkBehaviour
{
    Network_PlayerInputController inputController;
    Network_WeaponInventory weaponInventory;

    [SerializeField] Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputController = GetComponent<Network_PlayerInputController>();
        weaponInventory = GetComponent<Network_WeaponInventory>();

        inputController.onFoot.Shoot.performed += ctx => Fire();
        inputController.onFoot.Reload.performed += ctx => Reload();
    }

    void Fire()
    {
        if(!IsOwner) return;
        

        Weapon currentWeapon = weaponInventory.GetCurrentWeapon();
        if(currentWeapon != null)
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            if(currentWeapon.ammoInClip > 0)
            {
                currentWeapon.UseWeapon(ray);
                currentWeapon.ShootEffects();
                weaponInventory.UpdateAmmoText();
                FireRpc();
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
            currentWeapon.Reload();
            weaponInventory.UpdateAmmoText();
        }
    }

    void ReloadRpc()
    {
        Weapon visualWeapon = weaponInventory.GetCurrentVisualWeapon();
        if(visualWeapon != null)
        {
            //Need to change this to reload effects
            //visualWeapon.Reload();
            
        }
    }
}
