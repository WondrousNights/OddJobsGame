using System.Collections;
using System.Collections.Generic;
using AlmenaraGames;
using Unity.Netcode;
using UnityEngine;

public class Local_PlayerGunHandler : MonoBehaviour
{

    [SerializeField] Camera cam;

    public Gun currentGun;

    [SerializeField] GameObject visaulGunMuzzleEffect;

    [SerializeField] GameObject bulletHoleVisual;


    MultiAudioSource audioSource;
    Local_PlayerStats playerStats;
    private void Start()
    {
        audioSource = GetComponent<MultiAudioSource>();
        playerStats = GetComponent<Local_PlayerStats>();
    }

    public void Shoot()
    {
        if (playerStats.isDead) return;

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
            }
            currentGun.currentAmmo -= 1;
            GameObject holeVisual = Instantiate(bulletHoleVisual, hit.point, Quaternion.identity);

            // rotate the hole visual to shoot away from the mesh it hits
            holeVisual.transform.position = hit.point + (hit.normal * 0.01f);
            if (Quaternion.LookRotation(-hit.normal) != null)
                holeVisual.transform.rotation = Quaternion.LookRotation(-hit.normal);
           
            Destroy(holeVisual, 1f);
            GunVisuals();

            MultiAudioManager.PlayAudioObject(currentGun.shotsfx, transform.position);

        }
    }

   
    public void GunVisuals()
    {
        currentGun.GunVisuals();
        StartCoroutine("MuzzleFlash", 0.1f);
    }

 

    public void Reload()
    {
        if (playerStats.isDead) return;

        if(currentGun.isReloading == false) currentGun.Reload();
    }

     IEnumerator MuzzleFlash(float duration)
    {
        visaulGunMuzzleEffect.SetActive(true);

        yield return new WaitForSeconds(duration);

        visaulGunMuzzleEffect.SetActive(false);
    }



}
