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


    float currentRecoil = 0;
    float currentKickback = 0;
    Quaternion targetRotation;

    private bool isReloading;


    //Replace with Ammo Handler
    int currentAmmo = 30;
    int maxAmmo = 30;


    private void Start()
    {
        playerInputController = GetComponent<Local_PlayerInputController>();
        GunScriptableObject gun = Guns.Find(gun => gun.Type == Gun);

        if (gun == null)
        {
            Debug.LogError($"No GunScriptableObject found for GunType: {gun}");
            return;
        }

        ActiveGun = gun;
        gun.Spawn(GunParent, this);
        gunEffects = GunParent.GetComponentInChildren<GunEffects>();

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

       
            ActiveGun.Shoot(
            playerInputController.mycam, 
            GunParent.GetComponentInChildren<MuzzleFlash>()
            );

            gunEffects.KickbackAdjustment(0.1f);

            return;
        
        
    }


    
    public void Reload()
    {

            currentAmmo = 600;
    
            gunEffects.ReloadRotation();
        
        
    }

 




}
