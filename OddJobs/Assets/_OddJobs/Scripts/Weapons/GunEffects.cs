using System.Collections;
using System.Collections.Generic;
//using AlmenaraGames;
using Unity.Netcode;
using UnityEngine;

public class GunEffects : MonoBehaviour
{
    float currentRecoil = 0;
    float currentKickback = 0;
    Quaternion targetRotation;
    Vector3 startRot;
    
    [SerializeField] GunScriptableObject myGunProperties;
    public MeshRenderer[] meshRenderers;


    [SerializeField] bool isEnemyWeapon = false;
    
    public void ReloadRotation(PlayerGunHandler gunHandler)
    {
        if(isEnemyWeapon) return;
        StartCoroutine(Rotate(myGunProperties.ShootConfig.reloadTime, gunHandler) );
    }

    public void KickbackAdjustment(float duration)
    {
        if(isEnemyWeapon) return;
        StartCoroutine(Kickback(duration, myGunProperties.ShootConfig.maxRecoil, myGunProperties.ShootConfig.maxKickback));
    }

    IEnumerator Rotate(float duration, PlayerGunHandler gunHandler)
    {
        
        float startRotation = transform.localEulerAngles.x;
        float endRotation = startRotation - 360.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float xRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            transform.localEulerAngles = new Vector3(xRotation, myGunProperties.SpawnRotation.y, myGunProperties.SpawnRotation.z);

            if (t >= duration)
            {
                gunHandler.isReloading = false;
                transform.localEulerAngles = new Vector3(myGunProperties.SpawnRotation.x, myGunProperties.SpawnRotation.y, myGunProperties.SpawnRotation.z);
            }
            yield return null;
        }
        
    }


    IEnumerator Kickback(float duration, float maxRecoil, float maxKickback)
    {
        currentRecoil += maxRecoil;
        currentKickback += maxKickback;
        
        yield return new WaitForSeconds(duration);

        currentRecoil -= maxRecoil;
        currentKickback -= maxKickback;
    }

    private void FixedUpdate()
    {
        if(isEnemyWeapon) return;
        targetRotation.eulerAngles = new Vector3(currentRecoil, transform.localEulerAngles.y, transform.localEulerAngles.z);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * 10);
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, 0f, currentKickback), Time.deltaTime * 10);
    }

}
