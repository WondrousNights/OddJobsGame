using System.Collections;
using System.Collections.Generic;
using AlmenaraGames;
using Unity.Netcode;
using UnityEngine;

public class Network_PlayerGunHandler : NetworkBehaviour
{

    [SerializeField] Camera cam;

    //public Gun currentGun;

    [SerializeField] GameObject visaulGunMuzzleEffect;

    [SerializeField] GameObject bulletHoleVisual;


    MultiAudioSource audioSource;
    Network_PlayerStats playerStats;
    private void Start()
    {
        audioSource = GetComponent<MultiAudioSource>();
        playerStats = GetComponent<Network_PlayerStats>();
    }

    public void Shoot()
    {
        if (!IsOwner || playerStats.isDead) return;

        //if (currentGun.currentAmmo > 0 && !currentGun.isReloading)
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("I just hit" + hit.transform.name);
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    //hit.transform.GetComponent<NetworkEnemyStats>().TakeDamage(currentGun.damage);
                }
            }
            //currentGun.currentAmmo -= 1;
            GameObject holeVisual = Instantiate(bulletHoleVisual, hit.point, Quaternion.identity);
           
            Destroy(holeVisual, 1f);
            GunVisuals();
            //audioSource.AudioObject = currentGun.shotsfx;
            audioSource.Play();

            ShootServerRpc();

        }
    }

    [ServerRpc(RequireOwnership = true)]
    public void ShootServerRpc()
    {
        ShootClientRpc();
    }

    [ClientRpc]
    private void ShootClientRpc()
    {
        if (IsOwner) return;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("A player just hit" + hit.transform.name);
            GameObject holeVisual = Instantiate(bulletHoleVisual, hit.point, Quaternion.identity);
            Destroy(holeVisual, 1f);
            // Destroy(bulletHoleVisual, 2f);
        }
    }

    public void GunVisuals()
    {
        // currentGun.GunVisuals();
        GunVisualsServerRpc();
    }

    [ServerRpc(RequireOwnership = true)]
    public void GunVisualsServerRpc()
    {
        GunVisaulsClientRpc();
    }

    [ClientRpc]
    public void GunVisaulsClientRpc()
    {
        if (IsOwner) return;

        StartCoroutine("MuzzleFlash", 0.1f);
        //audioSource.AudioObject = currentGun.shotsfx;
        audioSource.Play();
    }

    public void Reload()
    {
        if (!IsOwner || playerStats.isDead) return;

        //if(currentGun.isReloading == false) currentGun.Reload();
    }

    IEnumerator MuzzleFlash(float duration)
    {
        visaulGunMuzzleEffect.SetActive(true);

        yield return new WaitForSeconds(duration);

        visaulGunMuzzleEffect.SetActive(false);
    }

    

}
