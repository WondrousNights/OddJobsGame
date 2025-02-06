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
    }

    void Fire()
    {
        if(!IsOwner) return;
        

        Weapon currentWeapon = weaponInventory.GetCurrentWeapon();
        if(currentWeapon != null)
        {
            Debug.Log("I just shot my weapon");

            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            currentWeapon.UseWeapon(ray);
        }
    }
}
