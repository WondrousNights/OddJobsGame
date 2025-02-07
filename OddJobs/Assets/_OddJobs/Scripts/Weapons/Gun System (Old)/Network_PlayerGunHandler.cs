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

    /*
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

    PlayerManager playerManager;


    private void Start()
    {
        magicalIK = GetComponent<Network_MagicalIK>();
        playerInputController = GetComponent<Network_PlayerInputController>();
        playerHealthManager = GetComponent<Local_PlayerHealthManager>();
        ammoHandler = GetComponent<PlayerAmmoHandler>();
        inventoryUI = GetComponentInChildren<Network_InventoryUI>();
        playerManager = GetComponent<PlayerManager>();

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

    /* Inventory Functionality 
    

  


    




    

   



   

    





  /* Gun Functionality 


    public void ShootCurrentGun()
    {
        if(!IsOwner) return;
        if(isHoldingObject) return;
        if(ActiveGun == null) return;
        // if the gun has ammo in clip
        // I think this might be causing bugs?

        if (!isReloading)
        {

            if(ammoHandler.currentClipAmmo[currentGunIndex] > 0)
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

                    Ray ray = new Ray(playerManager.playerController.cam.transform.position, playerManager.playerController.cam.transform.forward);
                    
                    //We need to change bullet spread
                    ray.origin += spread;

                    Shoot(ray);

                }
                ammoHandler.currentClipAmmo[currentGunIndex] -= 1;
                gunEffects.ShootEffect();
                UpdateAmmoText();
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
                        gunEffects.CantShootEffect();
                    }
                }
                else
                {
                    // FAIL, NO AMMO!
                    gunEffects.CantShootEffect();
                }
            }
        }
    }

    [Rpc(SendTo.NotMe)]
    void GunEffectsRpc()
    {
        visualGun.GetComponent<Network_GunEffects>();
        //playsfx;
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
            gunEffects.ReloadEffect();
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


    /* Shoot functionality 

    



    [Rpc(SendTo.Everyone)]
    void HitLimbRpc(string name, Ray ray, float hitForce, Vector3 hitPoint, Vector3 hitNormal, NetworkBehaviourReference damageableLimb, float damage)
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
        limb.DoDamage(damage);
        
        
               
        }                      
    }

    
  // TODO: this should really be consolidated into a general UI update function
    public void UpdateAmmoText()
    {
        //playerInputController.playerUI.UpdateAmmoText(ActiveGun, ammoHandler.currentClipAmmo[currentGunIndex], ammoHandler.lightAmmo, ammoHandler.mediumAmmo, ammoHandler.heavyAmmo);
    }
*/
}


