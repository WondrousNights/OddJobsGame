using System.Collections;
using System.Collections.Generic;
//using AlmenaraGames;
using Unity.Netcode;
using UnityEngine;

public class Network_GunEffects : MonoBehaviour
{
    [SerializeField] private bool debugLogs = true;
    [SerializeField] private float muzzleFlashTime = 0.05f;
    [SerializeField] private float kickBackVertical = 1f;
    [SerializeField] private float kickBackHorizontalRange = 0.2f;
    [SerializeField] private float kickBackReturnTime = 0.3f;
    [SerializeField] private bool doCameraKickback = true;

    public Animator animator;
    public ParticleSystem[] shootParticles;
    public GameObject muzzleFlash;
    public Transform cameraKickback;

    public AudioSource shootSoundClose;
    public AudioSource shootSoundFar;
    public AudioSource reloadSound;
    public AudioSource failShootSound;
    public AudioSource equipSound;


    private void Start()
    {
        ////////////////////////////////////// REMOVE
        doCameraKickback = false;


        if (!animator) animator = GetComponent<Animator>();
        muzzleFlash.SetActive(false);

        // camera kickback grabs it's parent's camera's gameobject's parent.
        // tl;dr: the camera should always have a "holster" parent that this script can grab and manipulate.
        if (doCameraKickback) cameraKickback = GetComponentInParent<Camera>().transform.parent;
        // elias: this is mad hacky but it works lol.
    }

    private void Awake() {
        
        equipSound.Play();
    }

    //  not used, animator plays automatically
    public void EquipEffect()
    {
        animator.SetTrigger("Equip");
    }

    public void ShootEffect()
    {
        if (debugLogs) Debug.Log("ShootEffectGun");

        animator.Play("Shoot");

        foreach(ParticleSystem i in shootParticles) {
            i.Play();
        }

        if (muzzleFlash) StartCoroutine(MuzzleFlash());
        if (cameraKickback && doCameraKickback) StartCoroutine(CameraKickback());

        shootSoundClose.PlayOneShot(shootSoundClose.clip); // use oneshot to allow simeltaneous plays
        shootSoundFar.Play(); // use play to disallow simeltaneous plays
    }

    private IEnumerator CameraKickback()
    {
        if (debugLogs) Debug.Log("Doing camera kickback on " + cameraKickback.name);

        Quaternion originalRotation = cameraKickback.rotation;
        float randomHorizontal = Random.Range(-kickBackHorizontalRange, kickBackHorizontalRange);
        cameraKickback.rotation = new Quaternion(
            cameraKickback.rotation.x - kickBackVertical,
            cameraKickback.rotation.y - randomHorizontal,
            cameraKickback.rotation.z,
            cameraKickback.rotation.w
        );

        // return to original position over time
        Quaternion targetRotation = new Quaternion(
            cameraKickback.rotation.x + kickBackVertical,
            cameraKickback.rotation.y + randomHorizontal,
            cameraKickback.rotation.z,
            cameraKickback.rotation.w
        );
        // targetRotation = originalRotation;
        float elapsedTime = 0;
        while (elapsedTime < kickBackReturnTime)
        {
            // use rotate function so camera can still be manipulated by other scripts, like mouse look
            Quaternion lerp = Quaternion.Lerp(cameraKickback.rotation, targetRotation, elapsedTime / kickBackReturnTime);
            cameraKickback.Rotate(lerp.x, lerp.y, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // cameraKickback.rotation = originalRotation;
        yield return null;
    }

    private IEnumerator MuzzleFlash()
    {
        if (debugLogs) Debug.Log("Showing muzzle flash");
        
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(muzzleFlashTime);
        muzzleFlash.SetActive(false);

        if (debugLogs) Debug.Log("Hiding muzzle flash");
    }

    public void ReloadEffect()
    {
        if (debugLogs) Debug.Log("ReloadEffectGun");

        animator.Play("Reload");
        reloadSound.Play();
    }
    public void CantShootEffect()
    {
        if (debugLogs) Debug.Log("CantShootEffectGun");
        
        failShootSound.Play();
    }


}
