using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]

// this script might be getting too bloated, but who cares! -elias
// TODO: move ragdoll control (and reloading?) to a separate script

public class PlayerGunHandler : MonoBehaviour
{
    [SerializeField]
    private bool debug = false;
    [SerializeField]
    private bool autoReload = true;
    [SerializeField]
    private float itemDropForce = 10;
    [SerializeField]
    private Transform weaponHolder;
    [SerializeField]
    private InventoryUI inventoryUI;

    [SerializeField]
    [Space]
    [Header("Runtime Filled")]
    public GunScriptableObject ActiveGun;
    public GunScriptableObject[] Inventory;
    public int currentGunIndex = 0;

    /// <summary>
    //GameObject ActiveGunGameObject;
    /// </summary>
    private GunEffects gunEffects;
    private Local_PlayerInputController playerInputController;
    private PlayerAmmoHandler ammoHandler;
    private HeldItemInteraction heldItem;
    private Local_PlayerHealthManager playerHealthManager;

    Quaternion targetRotation;
    public bool isReloading = false;
    public bool isEquipping = false; // not used

    private void Start()
    {
        playerInputController = GetComponent<Local_PlayerInputController>();
        playerHealthManager = GetComponent<Local_PlayerHealthManager>();
        ammoHandler = GetComponent<PlayerAmmoHandler>();

        GunScriptableObject gun = Inventory[0];

        if (gun)
        {
            EquipGunFromInventory(0, gun);
        } 
        else
        {
            if (debug) Debug.LogWarning("No gun found on player " + gameObject.name + ", equipping nothing");
        }
    }

    public void EquipGunFromInventory(int index = -1, GunScriptableObject gun = null)
    {
        DeEquipCurrentGun();

        if (index != -1) {
            currentGunIndex = index;
        }
        ActiveGun = Inventory[currentGunIndex];
        ActiveGun?.Spawn(weaponHolder, this);
        gunEffects = weaponHolder.GetComponentInChildren<GunEffects>();
        heldItem = weaponHolder.GetComponentInChildren<HeldItemInteraction>();
        
        if (ammoHandler.currentClipAmmo[currentGunIndex] == 0)
            Reload();

        UpdateAmmoText();
        inventoryUI.UpdateInventoryUI(Inventory);
    }

    public void DeEquipCurrentGun()
    {
        if (ActiveGun) 
        {
            ActiveGun.Despawn();
        }
        if (gunEffects)
        {
            gunEffects.gameObject.SetActive(false);
            Destroy(gunEffects.gameObject);
        }
        
        inventoryUI.UpdateInventoryUI(Inventory);
    }

    public void DropCurrentGun()
    {
        if (ActiveGun) {
            // drop a pickup item for it
            GameObject droppedModel = Instantiate(ActiveGun.droppedModelPrefab);
            droppedModel.transform.position = weaponHolder.position;
            droppedModel.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), 0);
            droppedModel.GetComponent<ItemPickup>().ammoInClip = ammoHandler.currentClipAmmo[currentGunIndex];

            // add throwing force to the weapon
            Rigidbody rb = droppedModel.GetComponent<Rigidbody>();
            rb.AddForce(weaponHolder.forward * itemDropForce, ForceMode.VelocityChange);
            
            // remove the gun from the inventory
            DeEquipCurrentGun();
            Inventory[currentGunIndex] = null;
            ActiveGun = null;
            ammoHandler.currentClipAmmo[currentGunIndex] = 0;

            UpdateAmmoText();
            inventoryUI.UpdateInventoryUI(Inventory);
        }    
    }

    public void PickupGun(GunScriptableObject gun, GameObject pickupObject)
    {
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
            AddGunToInventory(gun, nextFreeIndex, true, pickupObject);
        } 
        else
        {
            DropCurrentGun(); // throw current weapon
            AddGunToInventory(gun, currentGunIndex, true, pickupObject); // add the gun to the inventory
        }

        Destroy(pickupObject);

        if (debug) Debug.Log("Picked up gun: " + gun.name);
    }

    public void AddGunToInventory(GunScriptableObject gun, int gunIndex, bool equipImmediately = true, GameObject pickupObject = null)
    {
        Inventory[gunIndex] = gun;
        if (pickupObject) ammoHandler.currentClipAmmo[gunIndex] = pickupObject.GetComponent<ItemPickup>().ammoInClip;
        // if (equipImmediately) 
        EquipGunFromInventory(gunIndex);
    }

    public void ShootCurrentGun()
    {
        if(ActiveGun == null || playerInputController.playerHealthManager.isRagdoll) return;

        if (heldItem) heldItem.Interact(this);

        // if the gun has ammo in clip
        if(ammoHandler.currentClipAmmo[currentGunIndex] > 0)
        {
            // and we're not reloading
            if (!isReloading)
            {
                // shoot the gun
                ActiveGun.Shoot(
                playerInputController.shootPoint, 
                weaponHolder.GetComponentInChildren<MuzzleFlash>(),
                ammoHandler,
                currentGunIndex
                );

                // play shooting related effects
                gunEffects.KickbackAdjustment(0.1f);
                UpdateAmmoText();
            }
        }
        else
        {
            // if we have ammo to reload and auto reload is enabled, reload the gun
            if (ammoHandler.HasAmmoToReload(ActiveGun.AmmoType))
            {
                if (autoReload && !isReloading) {
                    Reload();
                }
                else
                {
                    // FAIL, NEED TO RELOAD!
                }
            }
            else
            {
                // FAIL, NEED AMMO!
            }
        }
    }

    public void Reload()
    {
        if(!ActiveGun) return;

        if(ammoHandler.HasAmmoToReload(ActiveGun.AmmoType))
        {
            isReloading = true;
            ammoHandler.ReloadAmmo(ActiveGun.AmmoClipSize, ActiveGun.AmmoType, currentGunIndex);
            UpdateAmmoText();
            gunEffects.ReloadRotation(this);
        }
        else
        {
            // Debug.Log("No AMMO! PLAY SFX TO NOTIFY PLAYER");
        }
        
    }

    public void SwitchWeaponNext()
    {
        if (debug) Debug.Log("Switching to next weapon");

        currentGunIndex++;
        if (currentGunIndex >= Inventory.Length) currentGunIndex = 0;

        // if the index has no gun in it, find the next gun in the inventory
        if (Inventory[currentGunIndex] == null)
        {
            for (int i = 0; i < Inventory.Length; i++)
            {
                if (Inventory[i] != null) { currentGunIndex = i; break; }
            }
        }

        EquipGunFromInventory(currentGunIndex);
        inventoryUI.UpdateInventoryUI(Inventory);
    }
    public void SwitchWeaponPrevious()
    {
        if (debug) Debug.Log("Switching to previous weapon");
        
        currentGunIndex--;
        if (currentGunIndex < 0) currentGunIndex = Inventory.Length - 1;

        // if the index has no gun in it, find the previous gun in the inventory
        if (Inventory[currentGunIndex] == null)
        {
            for (int i = Inventory.Length - 1; i >= 0; i--)
            {
                if (Inventory[i] != null) { currentGunIndex = i; break; }
            }
        }

        EquipGunFromInventory(currentGunIndex);
        inventoryUI.UpdateInventoryUI(Inventory);
    }

    // TODO: this should really be consolidated into a general UI update function
    public void UpdateAmmoText()
    {
        playerInputController.playerUI.UpdateAmmoText(ActiveGun, ammoHandler.currentClipAmmo[currentGunIndex], ammoHandler.lightAmmo, ammoHandler.mediumAmmo, ammoHandler.heavyAmmo);
    }


    // TODO: NEED A PERFORMANCE UPDATE! This is way too much for this functionality
    // shits nullrefs whenever the player ragdolls line 281
    void Update()
    {
        RagdollUpdate();
    }

    private void RagdollUpdate() 
    {
        if(playerHealthManager.isRagdoll)
        {
            foreach(MeshRenderer render in gunEffects?.meshRenderers)
            {
                render.enabled = false;
            }
        }
        else
        {
            if (gunEffects)
            {
                foreach(MeshRenderer render in gunEffects?.meshRenderers)
                {
                    render.enabled = true;
                }
            }
                
        }
    }

}
