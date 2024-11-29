using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public int damage;
    [SerializeField] int maxAmmo;
    public int currentAmmo;

    public AudioClip shotsfx;

    float currentRecoil = 0;
    [SerializeField] float maxRecoil;

    float currentKickback = 0;
    [SerializeField] float maxKickback;

    Quaternion targetRotation;

    [SerializeField] GameObject muzzleFlash;

    public bool isReloading;




    private void Start()
    {
        muzzleFlash.SetActive(false);
    }
    public void Reload()
    {
        isReloading = true;
        currentAmmo = maxAmmo;
    
        StartCoroutine("Rotate", 0.25f);
    }
    public void GunVisuals()
    {
        StartCoroutine("MuzzleFlash", 0.1f);
        StartCoroutine("Kickback", 0.1f);
    }

    IEnumerator Rotate(float duration)
    {
        float startRotation = transform.eulerAngles.z;
        float endRotation = startRotation - 360.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);

            if (t >= duration)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                isReloading = false;
            }
            yield return null;
        }
        
    }


    IEnumerator MuzzleFlash(float duration)
    {
        muzzleFlash.SetActive(true);

        yield return new WaitForSeconds(duration);

        muzzleFlash.SetActive(false);
    }

    IEnumerator Kickback(float duration)
    {


        currentRecoil += maxRecoil;
        currentKickback += maxKickback;
        
        yield return new WaitForSeconds(duration);

        currentRecoil -= maxRecoil;
        currentKickback -= maxKickback;
    }

    private void FixedUpdate()
    {
        targetRotation.eulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, currentRecoil);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * 10);
       transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, 0f, currentKickback), Time.deltaTime * 10);
    }

}
