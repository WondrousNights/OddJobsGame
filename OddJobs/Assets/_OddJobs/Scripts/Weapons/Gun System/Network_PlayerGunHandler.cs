using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Unity.Netcode.Components;

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
    private Network_GunEffects gunEffects;
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

    [SerializeField] public LayerMask BulletCollisionMask;


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

    /* Inventory Functionality */ 
    public void EquipGunFromInventory(int index = -1, Network_GunScriptableObject gun = null)
    {
        if(!IsOwner) return;
        DeEquipCurrentGun();

        if (index != -1) {
            currentGunIndex = index;
        }
        ActiveGun = Inventory[currentGunIndex];
        ActiveGun?.Spawn(weaponHolder);
        gunEffects = weaponHolder.GetComponentInChildren<Network_GunEffects>();
        //heldItem = weaponHolder.GetComponentInChildren<HeldItemInteraction>();
        
        if (ammoHandler.currentClipAmmo[currentGunIndex] == 0)
            Reload();

        //UpdateAmmoText();
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


    //Destroy gun for other players
    [Rpc(SendTo.NotMe)]
    void DeEquipVisualGunRpc()
    {
        magicalIK.UndoMagicalIk();
        if(visualGun != null) Destroy(visualGun);
    }

    public void DeEquipCurrentGun()
    {
        if(!IsOwner) return;
        if (ActiveGun) 
        {
            ActiveGun.Despawn();
            DeEquipVisualGunRpc();
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

            /* This needs to an rpc */
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

            //UpdateAmmoText();
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



  /* Gun Functionality */ 


    public void ShootCurrentGun()
    {
        if(!IsOwner) return;
        if(isHoldingObject) return;
        if(ActiveGun == null) return;
        // if the gun has ammo in clip
        // I think this might be causing bugs?

        if (!isReloading)
            {
                for(int i = 0; i < ActiveGun.ShootConfig.bulletsPerShot; i++)
                {
                    Vector3 spread = new Vector3(
                    Random.Range(
                        -ActiveGun.ShootConfig.playerSpread.x,
                        ActiveGun.ShootConfig.playerSpread.x
                    ),
                    Random.Range(
                        -ActiveGun.ShootConfig.playerSpread.y,
                        ActiveGun.ShootConfig.playerSpread.y
                    ), 0
                    );

                    Ray ray = new Ray(playerInputController.mycam.transform.position, playerInputController.mycam.transform.forward);
                    
                    //We need to change bullet spread
                    ray.origin += spread;

                    Shoot(ray);
                }
                gunEffects.ShootEffect();

                //UpdateAmmoText();
            }


        if(ammoHandler.currentClipAmmo[currentGunIndex] > 0)
        {
            
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
        if(!IsOwner) return;
        if(!ActiveGun) return;

        if(ammoHandler.HasAmmoToReload(ActiveGun.AmmoType))
        {
            isReloading = true;
            ammoHandler.ReloadAmmo(ActiveGun.ClipSize, ActiveGun.AmmoType, currentGunIndex);
            //UpdateAmmoText();
            StartCoroutine(ReloadWaitTimer(ActiveGun.ShootConfig.reloadTime));
            //gunEffects.ReloadRotation(this);
        }
        else
        {
            // Debug.Log("No AMMO! PLAY SFX TO NOTIFY PLAYER");
        }
        
    }


    IEnumerator ReloadWaitTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        isReloading = false;
    }


    /* Shoot functionality */

    public void Shoot(Ray ray)
    {
        RaycastHit hit;

        //Add Bullet Spread
                
                // bullet hit something!
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, BulletCollisionMask))
                {
                    // if the bullet hit something
                    if (hit.transform)
                    {

                        

                        if(hit.transform.GetComponent<DamagableLimb>())
                        {
                            var limb = hit.transform.GetComponent<DamagableLimb>();
                            HitLimbRpc(hit.transform.name, ray,ActiveGun.ShootConfig.hitForce, hit.point, hit.normal, limb);
                        }

                    
                        // We are going to switch to doing damage to rigidbodies

                        /*
                        // If the object hit has a damageable component, apply damage to it
                        if(hit.transform.TryGetComponent(out IDamageable damageable))
                        {
                            //damageable.TakeDamageFromGun(ray, ActiveGun.ShootConfig.Damage, ActiveGun.ShootConfig.hitForce, hit.point, ActiveGun.parent.gameObject, ShootConfig.recoveryTime);
                        }

                        // If the object hit has a damageable component in its parent, apply damage to it
                        if(hit.transform.GetComponentInParent<IDamageable>() != null)
                        {
                            //hit.transform.GetComponentInParent<IDamageable>().TakeDamageFromGun(ray, ShootConfig.Damage, ShootConfig.hitForce, hit.point, parent.gameObject, ShootConfig.recoveryTime);
                        }
                        */
                    }
                }
                // bullet did not hit something. 
                else
                {
                    // ActiveMonoBehaviour.StartCoroutine(
                    //     PlayTrail(
                    //         ShootSystem.transform.position,
                    //         shootPoint.transform.position + (shootPoint.transform.forward * TrailConfig.MissDistance),
                    //         new RaycastHit(),
                    //         ray
                    //     )
                    // );
                }
            

    }



    [Rpc(SendTo.Everyone)]
    void HitLimbRpc(string name, Ray ray, float hitForce, Vector3 hitPoint, Vector3 hitNormal, NetworkBehaviourReference damageableLimb)
    {

        if (damageableLimb.TryGet(out DamagableLimb limb))
        {
           Debug.Log("A player just hit" + name);

         
        GameObject impactParticle = Instantiate(ActiveGun.ShootConfig.impactParticle, hitPoint, Quaternion.identity);
        impactParticle.transform.position = hitPoint + (hitNormal * 0.01f);
        if (hitNormal != Vector3.zero) impactParticle.transform.rotation = Quaternion.LookRotation(-hitNormal);
        Destroy(impactParticle, 10f);

        // spawn bullet hole decal
        GameObject bulletHoleDecal = Instantiate(ActiveGun.ShootConfig.bulletHoleDecal, hitPoint, Quaternion.identity);
        bulletHoleDecal.transform.position = hitPoint + (hitNormal * 0.01f);
        
        bulletHoleDecal.transform.parent = limb.transform; // make the bullethole decal move with the thing it hit
       
        Quaternion normalRotation = Quaternion.LookRotation(-hitNormal); // Create rotation that faces away from the surface
        bulletHoleDecal.transform.rotation = normalRotation * Quaternion.Euler(0, 0, Random.Range(0, 360)); // Add random rotation around that
        Destroy(bulletHoleDecal, 60f);
        

                        // elias: note to self, need to use hit.transform here instead of hit.collider because the collider is not always the parent of the object hit
                        // If the object hit has a rigidbody, apply a force to it
        
        
        limb.transform.GetComponent<Rigidbody>().AddForceAtPosition(ray.direction * hitForce, hitPoint, ForceMode.Impulse);
        limb.DoDamage();
        
        
               
        }                      
    }

 


}


