using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]


//Most of this handled client side
public class Network_PlayerGunHandler : NetworkBehaviour
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
    private Network_InventoryUI inventoryUI;

    [SerializeField]
    [Space]
    [Header("Runtime Filled")]
    public Network_GunScriptableObject ActiveGun;
    public Network_GunScriptableObject[] Inventory;
    public int currentGunIndex = 0;

    /// <summary>
    //GameObject ActiveGunGameObject;
    /// </summary>
    private GunEffects gunEffects;
    private Network_PlayerInputController playerInputController;
    private PlayerAmmoHandler ammoHandler;
    private HeldItemInteraction heldItem;
    private Local_PlayerHealthManager playerHealthManager;

    Quaternion targetRotation;
    public bool isReloading = false;
    public bool isEquipping = false; // not used

    public bool isHoldingObject = false;


    [SerializeField] Network_GunScriptableObjectList network_GunScriptableObjectList;
    GameObject visualGun;
    Network_MagicalIK magicalIK;

    private void Start()
    {
        magicalIK = GetComponent<Network_MagicalIK>();
        playerInputController = GetComponent<Network_PlayerInputController>();
        playerHealthManager = GetComponent<Local_PlayerHealthManager>();
        ammoHandler = GetComponent<PlayerAmmoHandler>();

        Network_GunScriptableObject gun = Inventory[0];

        if (gun)
        {
            EquipGunFromInventory(0, gun);
        } 
        else
        {
            if (debug) Debug.LogWarning("No gun found on player " + gameObject.name + ", equipping nothing");
        }
    }

    public void EquipGunFromInventory(int index = -1, Network_GunScriptableObject gun = null)
    {
        if(!IsOwner) return;
        DeEquipCurrentGun();

        if (index != -1) {
            currentGunIndex = index;
        }
        ActiveGun = Inventory[currentGunIndex];
        ActiveGun?.Spawn(weaponHolder, this);
        //gunEffects = weaponHolder.GetComponentInChildren<GunEffects>();
        //heldItem = weaponHolder.GetComponentInChildren<HeldItemInteraction>();
        
        if (ammoHandler.currentClipAmmo[currentGunIndex] == 0)
            Reload();

        UpdateAmmoText();
        inventoryUI.UpdateInventoryUI(Inventory);

        EquipGunVisualRpc(ActiveGun.Type);

    }

    // spawn gun for other players
    [Rpc(SendTo.NotMe)]
    void EquipGunVisualRpc(GunType gunType)
    {
        if(visualGun != null) Destroy(visualGun);
        for(var i = 0; i < network_GunScriptableObjectList.GunScriptableObjectList.Count; i++)
        {
            if(network_GunScriptableObjectList.GunScriptableObjectList[i].Type == gunType)
            {
                //Spawn shit here

                ActiveGun = network_GunScriptableObjectList.GunScriptableObjectList[i];

                GameObject Model = Instantiate(network_GunScriptableObjectList.GunScriptableObjectList[i].OtherPlayerModelPrefab);
                Model.transform.SetParent(gameObject.transform, false);
                Model.transform.localPosition = network_GunScriptableObjectList.GunScriptableObjectList[i].OtherPlayerGunSpawnPos;
                Model.transform.localRotation = Quaternion.Euler(network_GunScriptableObjectList.GunScriptableObjectList[i].OtherPlayerGunRotation);

                visualGun = Model;

                magicalIK.DoMagicalIK(visualGun);
            }
        }
    }

    public void DeEquipCurrentGun()
    {
        if(!IsOwner) return;
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


    public void DropCurrentGunRpc()
    {
        if(!IsOwner) return;
        if (ActiveGun) {
            // drop a pickup item for it
            GameObject droppedModel = Instantiate(ActiveGun.DroppedPrefab);
            droppedModel.transform.position = weaponHolder.position;
            droppedModel.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), 0);
            droppedModel.GetComponent<Network_ItemPickup>().ammoInClip = ammoHandler.currentClipAmmo[currentGunIndex];

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

    public void PickupGun(Network_GunScriptableObject gun, GameObject pickupObject)
    {
        if(!IsOwner) return;
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
            DropCurrentGunRpc(); // throw current weapon
            AddGunToInventory(gun, currentGunIndex, true, pickupObject); // add the gun to the inventory
        }

        pickupObject.GetComponent<Network_ItemPickup>().DestoryItemRpc();

        if (debug) Debug.Log("Picked up gun: " + gun.name);
    }


    public void AddGunToInventory(Network_GunScriptableObject gun, int gunIndex, bool equipImmediately = true, GameObject pickupObject = null)
    {
        if(!IsOwner) return;
        Inventory[gunIndex] = gun;
        if (pickupObject) ammoHandler.currentClipAmmo[gunIndex] = pickupObject.GetComponent<Network_ItemPickup>().ammoInClip;
        // if (equipImmediately) 
        EquipGunFromInventory(gunIndex);
    }

  
    public void ShootCurrentGun()
    {
        if(!IsOwner) return;
        if(isHoldingObject) return;
        if(ActiveGun == null) return;

        // if the gun has ammo in clip //I think this might be causing bugs?
        if(ammoHandler.currentClipAmmo[currentGunIndex] > 0)
        {
            // and we're not reloading
            if (!isReloading)
            {
                // shoot the gun
                
                ShootCurrentGunRpc();
                // play shooting related effects
                //gunEffects.KickbackAdjustment(0.1f);
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

    [Rpc(SendTo.Everyone)]
    void ShootCurrentGunRpc()
    {
        ActiveGun.Shoot(playerInputController.mycam.transform);
    }

    public void Reload()
    {
        if(!IsOwner) return;
        if(!ActiveGun) return;

        if(ammoHandler.HasAmmoToReload(ActiveGun.AmmoType))
        {
            isReloading = true;
            ammoHandler.ReloadAmmo(ActiveGun.ClipSize, ActiveGun.AmmoType, currentGunIndex);
            UpdateAmmoText();
            StartCoroutine(ReloadWaitTimer(ActiveGun.ShootConfig.reloadTime));
            //gunEffects.ReloadRotation(this);
        }
        else
        {
            // Debug.Log("No AMMO! PLAY SFX TO NOTIFY PLAYER");
        }
        
    }

    public void SwitchWeaponNext()
    {
        if(!IsOwner) return;
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
        if(!IsOwner) return;
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
        if(!IsOwner) return;
        playerInputController.playerUI.UpdateAmmoText(ActiveGun, ammoHandler.currentClipAmmo[currentGunIndex], ammoHandler.lightAmmo, ammoHandler.mediumAmmo, ammoHandler.heavyAmmo);
    }

    IEnumerator ReloadWaitTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        isReloading = false;
    }
}
