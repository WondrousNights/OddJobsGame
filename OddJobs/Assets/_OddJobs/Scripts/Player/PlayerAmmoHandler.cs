using System;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class PlayerAmmoHandler : MonoBehaviour
{
    public int lightAmmo;
    public int mediumAmmo;
    public int heavyAmmo;

    private Network_WeaponInventory weaponInventory;

    void Start()
    {
        weaponInventory = GetComponent<Network_WeaponInventory>();
    }


    public int AmmoToReload(AmmoType type, int currentAmmoInClip, int clipSize)
    {
    int ammoNeededToReload = clipSize - currentAmmoInClip;
    int ammoToReload = 0;

    switch (type)
    {
        case AmmoType.Light:
            ammoToReload = Math.Min(ammoNeededToReload, lightAmmo);
            lightAmmo -= ammoToReload;
            break;
        case AmmoType.Medium:
            ammoToReload = Math.Min(ammoNeededToReload, mediumAmmo);
            mediumAmmo -= ammoToReload;
            break;
        case AmmoType.Heavy:
            ammoToReload = Math.Min(ammoNeededToReload, heavyAmmo);
            heavyAmmo -= ammoToReload;
            break;
        default:
            throw new ArgumentException("Invalid ammo type", nameof(type));
    }

    return ammoToReload;
    }

    public void AddAmmo(AmmoType ammoType, int amount)
    {
        if(ammoType == AmmoType.Light)
        {
            lightAmmo += amount;
        }
        if(ammoType == AmmoType.Medium)
        {
            mediumAmmo += amount;
        }
        if(ammoType == AmmoType.Heavy)
        {
            heavyAmmo += amount;
        }

        weaponInventory.UpdateAmmoText();
    }

    public int TotalAmmo(AmmoType ammoType)
    {
        if(ammoType == AmmoType.Light)
        {
            return lightAmmo;
        }
        else if(ammoType == AmmoType.Medium)
        {
            return mediumAmmo;
        }
        else
        {
            return heavyAmmo;
        }
    }
    
}
