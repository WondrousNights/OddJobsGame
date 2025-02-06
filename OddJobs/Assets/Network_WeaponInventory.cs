using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Network_WeaponInventory : NetworkBehaviour
{
    [SerializeField] Transform gunHolder;
    [SerializeField] Weapon[] Inventory;
    private int currentWeaponIndex = 0;
    Weapon activeWeapon;


    [SerializeField] bool debug;

    public Weapon GetCurrentWeapon()
    {
        return Inventory[currentWeaponIndex];
    }


    public void PickupWeapon(Network_WeaponProperties weaponProperties)
    {
       if(!IsOwner) return;

        Weapon newWeapon = weaponProperties.Spawn();
        newWeapon.transform.SetParent(gunHolder);
        newWeapon.transform.localPosition = weaponProperties.PlayerSpawnPoint;
        newWeapon.transform.localRotation = Quaternion.Euler(weaponProperties.PlayerSpawnRotation);


        // find the next empty slot in the inventory
        int nextFreeIndex = -1;
        for (int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i] == null)
            {
                if (debug) Debug.Log(Inventory[i]);
                nextFreeIndex = i;
                break;
            }
        }

        // if there's an empty slot, add the gun to the inventory there
        if (nextFreeIndex != -1)
        {
            AddWeaponToInventory(newWeapon, nextFreeIndex);
            //newWeapon.gameObject.SetActive(false);
        } 
        else
        {
            //DropCurrentGun(); // throw current weapon
            AddWeaponToInventory(newWeapon, currentWeaponIndex); // add the gun to the inventory
        }
        
  

        if (debug) Debug.Log("Picked up gun: " + weaponProperties.name);
    }

    public void AddWeaponToInventory(Weapon weapon, int weaponIndex)
    {
        if(!IsOwner) return;
        Inventory[weaponIndex] = weapon;
       
        // if (equipImmediately) 
        //EquipGunFromInventory(weaponIndex);
    }

    public void EquipGunFromInventory(int index = -1)
    {
        if(!IsOwner) return;
        //DeEquipCurrentGun();

        if (index != -1) {
            currentWeaponIndex = index;
        }
        
        
        activeWeapon = Inventory[currentWeaponIndex];
    }

}
