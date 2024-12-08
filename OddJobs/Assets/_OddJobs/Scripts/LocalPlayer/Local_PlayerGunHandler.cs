using System.Collections;
using System.Collections.Generic;
using AlmenaraGames;
using Unity.Netcode;
using UnityEngine;

// obsolete
public class Local_PlayerGunHandler : MonoBehaviour
{

    // [SerializeField] int hitForce = 1000;
    // [SerializeField] bool autoReload = true;
    // // [SerializeField] float autoReloadDelay = 0.4f;

    // [SerializeField] Camera cam;
    // public GunEffects currentGun;

    // [SerializeField] GameObject bulletHoleVisual;

    // MultiAudioSource audioSource;
    // Local_PlayerStats playerStats;

/*
    private void Start()
    {
        audioSource = GetComponent<MultiAudioSource>();
        playerStats = GetComponent<Local_PlayerStats>();
    }

    public void Shoot()
    {
        if (playerStats && playerStats.isDead) return;

        if (currentGun.currentAmmo > 0 && !currentGun.isReloading)
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Debug.Log("I just hit" + hit.transform.name);
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    //hit.transform.GetComponent<NetworkEnemyStats>().TakeDamage(currentGun.damage);
                }
                if (hit.transform.GetComponent<Rigidbody>())
                {
                    hit.transform.GetComponent<Rigidbody>().AddForceAtPosition(ray.direction * hitForce, hit.point);
                } 
                else {
                    GameObject holeVisual = Instantiate(bulletHoleVisual, hit.point, Quaternion.identity);

                    // rotate the hole visual to shoot away from the mesh it hits
                    holeVisual.transform.position = hit.point + (hit.normal * 0.01f);
                    if (hit.normal != Vector3.zero)
                        holeVisual.transform.rotation = Quaternion.LookRotation(-hit.normal);
                
                    Destroy(holeVisual, 10f);
                }

                currentGun.GunVisuals(hit.point);
            }
            currentGun.currentAmmo -= 1;

            MultiAudioManager.PlayAudioObject(currentGun.shotsfx, transform.position);

        }

        if (autoReload && currentGun.currentAmmo <= 0) {
            Invoke("Reload", autoReloadDelay);
        }
    }

    public void Reload()
    {
        if (playerStats.isDead) return;

        if(currentGun.isReloading == false) currentGun.Reload();
    }
    */
}
