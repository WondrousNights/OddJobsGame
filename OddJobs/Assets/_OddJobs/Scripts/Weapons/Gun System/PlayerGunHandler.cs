using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerGunHandler : MonoBehaviour
{
    [SerializeField]
    private GunType Gun;
    [SerializeField]
    private Transform GunParent;
    [SerializeField]
    private List<GunScriptableObject> Guns;
    [SerializeField]
    //private PlayerIK InverseKinematics;

    [Space]
    [Header("Runtime Filled")]
    public GunScriptableObject ActiveGun;

    public GunScriptableObject[] GunInventory;

    [SerializeField] int currentGunIndex = 0;

    /// <summary>
    //GameObject ActiveGunGameObject;
    /// </summary>
    GunEffects gunEffects;
    

    private Local_PlayerInputController playerInputController;
    private PlayerAmmoHandler ammoHandler;
    private Local_PlayerHealthManager playerHealthManager;


    // float currentRecoil = 0;
    // float currentKickback = 0;
    Quaternion targetRotation;

    public bool isReloading = false;


    //Replace with Ammo Handler
    // int currentAmmo = 30;
    // int maxAmmo = 30;


    private void Start()
    {
        playerInputController = GetComponent<Local_PlayerInputController>();
        playerHealthManager = GetComponent<Local_PlayerHealthManager>();
        ammoHandler = GetComponent<PlayerAmmoHandler>();


        GunScriptableObject gun = Guns.Find(gun => gun.Type == Gun);

        if (gun == null)
        {
            Debug.LogError($"No GunScriptableObject found for GunType: {gun}");
            return;
        }

        SetupGun(gun, currentGunIndex);
        // some magic for IK
        //Transform[] allChildren = GunParent.GetComponentsInChildren<Transform>();
        //InverseKinematics.LeftElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "LeftElbow");
        //InverseKinematics.RightElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "RightElbow");
       // InverseKinematics.LeftHandIKTarget = allChildren.FirstOrDefault(child => child.name == "LeftHand");
        //InverseKinematics.RightHandIKTarget = allChildren.FirstOrDefault(child => child.name == "RightHand");
    }
    public void SetupGun(GunScriptableObject gun, int gunIndex)
    {
        GunInventory[gunIndex] = gun;
        ActiveGun = GunInventory[gunIndex];
        gun.Spawn(GunParent, this);
        gunEffects = GunParent.GetComponentInChildren<GunEffects>();

        if(ammoHandler.HasAmmoToReload(ActiveGun.AmmoType))
        {
            isReloading = true;
            ammoHandler.ReloadAmmo(ActiveGun.AmmoClipSize, ActiveGun.AmmoType, currentGunIndex);
            gunEffects.ReloadRotation(this);
        }
    }


    public void DespawnActiveGun()
    {
        ActiveGun.Despawn();
        gunEffects.gameObject.SetActive(false);
        Destroy(gunEffects.gameObject);
    }

    public void PickupGun(GunScriptableObject gun)
    {
        
        DespawnActiveGun();

        if(GunInventory[1] != null)
        {
            SetupGun(gun, currentGunIndex);
        }
        else
        {
           currentGunIndex = 1;
           SetupGun(gun, 1);
        }
      
    }

    public void ShootCurrentGun()
    {
        if(ActiveGun == null) return;

        if(ammoHandler.currentAmmo[currentGunIndex] > 0 && !isReloading)
        {
            ActiveGun.Shoot(
            playerInputController.mycam, 
            GunParent.GetComponentInChildren<MuzzleFlash>(),
            ammoHandler,
            currentGunIndex
            );

            gunEffects.KickbackAdjustment(0.1f);
        }
    }

    public void Reload()
    {

        if(ammoHandler.HasAmmoToReload(ActiveGun.AmmoType))
        {
            isReloading = true;
            ammoHandler.ReloadAmmo(ActiveGun.AmmoClipSize, ActiveGun.AmmoType, currentGunIndex);
            gunEffects.ReloadRotation(this);
        }
        else
        {
            // Debug.Log("No AMMO! PLAY SFX TO NOTIFY PLAYER");
        }
        
    }

    public void SwitchWeapon()
    {
        if(GunInventory[1] == null) return;

        if(currentGunIndex == 0)
        {
            currentGunIndex = 1;
        }
        else
        {
            currentGunIndex = 0;
        }

        DespawnActiveGun();
        ActiveGun = GunInventory[currentGunIndex];
        ActiveGun.Spawn(GunParent, this);
        gunEffects = GunParent.GetComponentInChildren<GunEffects>();

    
    }






    ///NEED A PERFORMANCE UPDATE HEREE! THis is way too much for this functionality
    void Update()
    {
        if(playerHealthManager.isRagdoll)
        {
            foreach(MeshRenderer render in gunEffects.meshRenderers)
            {
                render.enabled = false;
            }
        }
        else
        {
            foreach(MeshRenderer render in gunEffects.meshRenderers)
            {
                render.enabled = true;
            }
        }
    }

}
