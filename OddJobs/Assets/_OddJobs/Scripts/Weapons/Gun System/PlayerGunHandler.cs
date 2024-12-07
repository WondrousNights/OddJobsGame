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

    public GameObject ActiveGunGameObject;

    

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
        ActiveGunGameObject = GunParent.GetComponentInChildren<Transform>().gameObject;

        // some magic for IK
        //Transform[] allChildren = GunParent.GetComponentsInChildren<Transform>();
        //InverseKinematics.LeftElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "LeftElbow");
        //InverseKinematics.RightElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "RightElbow");
       // InverseKinematics.LeftHandIKTarget = allChildren.FirstOrDefault(child => child.name == "LeftHand");
        //InverseKinematics.RightHandIKTarget = allChildren.FirstOrDefault(child => child.name == "RightHand");
    }
    private void FixedUpdate()
    {
        targetRotation.eulerAngles = new Vector3(ActiveGunGameObject.transform.localEulerAngles.x + currentRecoil, ActiveGunGameObject.transform.localEulerAngles.z, ActiveGunGameObject.transform.localEulerAngles.z);
        ActiveGunGameObject.transform.localRotation = Quaternion.Lerp(ActiveGunGameObject.transform.localRotation, targetRotation, Time.deltaTime * 10);
        ActiveGunGameObject.transform.localPosition = Vector3.Lerp(ActiveGunGameObject.transform.localPosition, new Vector3(ActiveGun.SpawnPoint.x, ActiveGun.SpawnPoint.y, ActiveGun.SpawnPoint.z + currentKickback), Time.deltaTime * 10);
    }


    public void ShootCurrentGun()
    {
        if(ActiveGun == null) return;

        if(currentAmmo > 0 && !isReloading)
        {
            ActiveGun.Shoot(
            playerInputController.mycam, 
            GunParent.GetComponentInChildren<MuzzleFlash>()
            );
            StartCoroutine("Kickback", 0.1f);

            currentAmmo -= 1;
            return;
        }
        
    }


    IEnumerator Kickback(float duration)
    {
        currentRecoil += ActiveGun.ShootConfig.maxRecoil;
        currentKickback += ActiveGun.ShootConfig.maxKickback;
        
        yield return new WaitForSeconds(duration);

        currentRecoil -= ActiveGun.ShootConfig.maxRecoil;
        currentKickback -= ActiveGun.ShootConfig.maxKickback;
    }

    
    public void Reload()
    {
        if(!isReloading)
        {
            isReloading = true;
            currentAmmo = 600;
    
            StartCoroutine("Rotate", ActiveGun.ShootConfig.reloadTime);
        }
        
    }

    IEnumerator Rotate(float duration)
    {
        float startRotation = ActiveGunGameObject.transform.eulerAngles.x;
        float endRotation = startRotation - 360.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float xRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            ActiveGunGameObject.transform.eulerAngles = new Vector3(xRotation, ActiveGunGameObject.transform.eulerAngles.y, ActiveGunGameObject.transform.eulerAngles.z);

            if (t >= duration)
            {
                ActiveGunGameObject.transform.eulerAngles = new Vector3(0, ActiveGunGameObject.transform.eulerAngles.y, ActiveGunGameObject.transform.eulerAngles.z);
                isReloading = false;
            }
            yield return null;
        }
        
    }




}
