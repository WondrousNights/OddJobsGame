using System.Collections;
using System.Collections.Generic;
using AlmenaraGames;
using Unity.Netcode;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public int damage;
    [SerializeField] int maxAmmo;
    public int currentAmmo;

    public AudioObject shotsfx;

    float currentRecoil = 0;
    [SerializeField] float maxRecoil;

    float currentKickback = 0;
    [SerializeField] float maxKickback;

    Quaternion targetRotation;

    [SerializeField] GameObject muzzleFlash;
    [SerializeField] GameObject particleEffect;
    [SerializeField] GameObject lineParticleEffect;

    public bool isReloading;


    private void Start()
    {
        muzzleFlash.SetActive(false);
        lineParticleEffect.SetActive(false);
    }

    public void Reload()
    {
        isReloading = true;
        currentAmmo = maxAmmo;
    
        StartCoroutine("Rotate", 0.25f);
    }

    public void GunVisuals(Vector3 target)
    {
        StartCoroutine("Kickback", 0.1f);
        
        if (muzzleFlash)
            StartCoroutine("MuzzleFlash", 0.1f);

        if (lineParticleEffect)
            StartCoroutine("LineParticle", target);

        if (particleEffect )
        {
            particleEffect.SetActive(true);
            particleEffect.GetComponent<ParticleSystem>().Play();
        }

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

    IEnumerator LineParticle(Vector3 target)
    {
        lineParticleEffect.transform.localPosition = muzzleFlash.transform.localPosition;
        lineParticleEffect.SetActive(true);

        // yield return new WaitForSeconds(0.1f);

        if (target != null) {
            lineParticleEffect.transform.position = target;
        } else {
            lineParticleEffect.transform.position = transform.position + (transform.forward * -30f);
        }

        yield return new WaitForSeconds(lineParticleEffect.GetComponent<TrailRenderer>().time);
        
        lineParticleEffect.SetActive(false);
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
