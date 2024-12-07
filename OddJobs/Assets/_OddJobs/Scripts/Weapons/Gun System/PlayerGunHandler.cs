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

    /// <summary>
    /// [SerializeField] GameObject ActiveGunGameObject;
    /// </summary>
    GunEffects gunEffects;
    

    private Local_PlayerInputController playerInputController;
    private PlayerAmmoHandler ammoHandler;


    float currentRecoil = 0;
    float currentKickback = 0;
    Quaternion targetRotation;

    public bool isReloading = false;


    //Replace with Ammo Handler
    int currentAmmo = 30;
    int maxAmmo = 30;


    private void Start()
    {
        playerInputController = GetComponent<Local_PlayerInputController>();
        ammoHandler = GetComponent<PlayerAmmoHandler>();
        GunScriptableObject gun = Guns.Find(gun => gun.Type == Gun);

        if (gun == null)
        {
            Debug.LogError($"No GunScriptableObject found for GunType: {gun}");
            return;
        }

        ActiveGun = gun;
        gun.Spawn(GunParent, this);
        gunEffects = GunParent.GetComponentInChildren<GunEffects>();

        ammoHandler.currentAmmo = ActiveGun.AmmoClipSize;
        // some magic for IK
        //Transform[] allChildren = GunParent.GetComponentsInChildren<Transform>();
        //InverseKinematics.LeftElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "LeftElbow");
        //InverseKinematics.RightElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "RightElbow");
       // InverseKinematics.LeftHandIKTarget = allChildren.FirstOrDefault(child => child.name == "LeftHand");
        //InverseKinematics.RightHandIKTarget = allChildren.FirstOrDefault(child => child.name == "RightHand");
    }


    public void ShootCurrentGun()
    {
        if(ActiveGun == null) return;

        if(ammoHandler.currentAmmo > 0 && !isReloading)
        {
            ActiveGun.Shoot(
            playerInputController.mycam, 
            GunParent.GetComponentInChildren<MuzzleFlash>(),
            ammoHandler
            );

            gunEffects.KickbackAdjustment(0.1f);
        }
            
        
        
    }

    public void Reload()
    {

        if(ammoHandler.HasAmmoToReload(ActiveGun.AmmoType))
        {
        isReloading = true;
        ammoHandler.ReloadAmmo(ActiveGun.AmmoClipSize, ActiveGun.AmmoType);
        gunEffects.ReloadRotation(this);
        }
        else
        {
            Debug.Log("No AMMO! PLAY SFX TO NOTIFY PLAYER");
        }
        
    }

 




}
