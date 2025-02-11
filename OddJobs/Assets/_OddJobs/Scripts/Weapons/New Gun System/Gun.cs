using System.Numerics;
using Unity.Netcode;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Gun : Weapon
{
    bool active;


    public override void UseWeapon(Ray ray, bool isPlayer)
    {
        Shoot(ray, isPlayer);
    }

    protected void Shoot(Ray ray, bool isPlayer)
    {
        
        LastShootTime = Time.time;

            for(int i = 0; i < weaponProperties.bulletsPerShot; i++)
            {
                RaycastHit hit;
                // bullet hit something!
                Vector3 spread;
                if(isPlayer) spread = Random.insideUnitCircle * weaponProperties.playerBulletSpread;
                else { spread = Random.insideUnitCircle * weaponProperties.enemyBulletSpread;; }
                UnityEngine.Quaternion spreadRotation = UnityEngine.Quaternion.Euler(spread.y, spread.x, 0);
                ray.direction = spreadRotation * ray.direction; 

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, weaponProperties.BulletCollisionMask))
                {
                    // if the bullet hit something
                    Debug.Log(hit.transform.name);
                    if(hit.transform.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamageRpc(weaponProperties.Damage, weaponProperties.hitForce, ray, hit.point);
                    }
                    
                }
                // bullet did not hit something. 
                else
                {
                    Debug.Log("Raycast hit nothing");
                }


            }
            ammoInClip -= 1;
    }

    public override void ShootEffects()
    {
        if(!active) return;
        Network_GunEffects gunEffects = GetComponent<Network_GunEffects>();
        gunEffects.ShootEffect();
    }

    public override void DestroyWeapon()
    {
        Destroy(this.gameObject);
    }

    public override void Reload(int ammoToReload)
    {
        ammoInClip += ammoToReload;

        Network_GunEffects gunEffects = GetComponent<Network_GunEffects>();
        gunEffects.ReloadEffect();
    }

    public override void ReloadEffects()
    {
        if(!active) return;
        Network_GunEffects gunEffects = GetComponent<Network_GunEffects>();
        gunEffects.ReloadEffect();
    }

    public override void HideWeapon()
    {
        gameObject.SetActive(false);
        active = false;
    }

    public override void ShowWeapon()
    {
        gameObject.SetActive(true);
        active = true;
    }
}
