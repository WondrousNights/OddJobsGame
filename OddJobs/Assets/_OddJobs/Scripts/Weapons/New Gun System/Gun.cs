using Unity.Netcode;
using UnityEngine;

public class Gun : Weapon
{
    

    protected override void UpdateUI()
    {
        
    }

    public override void UseWeapon(Ray ray)
    {
        Shoot(ray);
    }

    protected void Shoot(Ray ray)
    {
        
        RaycastHit hit;

        //Add Bullet Spread
                
                // bullet hit something!
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, weaponProperties.BulletCollisionMask))
                {
                    // if the bullet hit something
                    if (hit.transform)
                    {
                        Debug.Log(transform.name);
                        // We are going to switch to doing damage to rigidbodies
                        // If the object hit has a damageable component, apply damage to it
                        if(hit.transform.TryGetComponent(out IDamageable damageable))
                        {
                            damageable.TakeDamage();
                        }
                    }
                }
                // bullet did not hit something. 
                else
                {
                    // ActiveMonoBehaviour.StartCoroutine(
                    //     PlayTrail(
                    //         ShootSystem.transform.position,
                    //         shootPoint.transform.position + (shootPoint.transform.forward * TrailConfig.MissDistance),
                    //         new RaycastHit(),
                    //         ray
                    //     )
                    // );
                }
        ammoInClip -= 1;

    }

    public override void ShootEffects()
    {
        Network_GunEffects gunEffects = GetComponent<Network_GunEffects>();
        gunEffects.ShootEffect();
    }

    public override void DestroyWeapon()
    {
        Destroy(this.gameObject);
    }

    public override void Reload()
    {
        ammoInClip = weaponProperties.ClipSize;

        Network_GunEffects gunEffects = GetComponent<Network_GunEffects>();
        gunEffects.ReloadEffect();
    }

    public override void HideWeapon()
    {
        gameObject.SetActive(false);
    }

    public override void ShowWeapon()
    {
        gameObject.SetActive(true);
    }
}
