using Unity.Netcode;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] Network_GunProperties gunProperties;

    GameObject model;

    void Start()
    {
        
    }

    protected override void DropWeapon()
    {
        
    }

    protected override void PickupWeapon()
    {
        
    }

    protected override void SpawnWeapon(Transform parent, Vector3 position, Vector3 rotation)
    {
        model = Instantiate(gunProperties.ModelPrefab);
        model.transform.SetParent(parent, false);
        model.transform.localPosition = position;
        model.transform.localRotation = Quaternion.Euler(rotation);
    }

    protected override void UpdateUI()
    {
        
    }

    public override void UseWeapon(Ray ray)
    {
        if(!IsOwner) return;
        Shoot(ray);
    }

    protected void Shoot(Ray ray)
    {
        RaycastHit hit;

        //Add Bullet Spread
                
                // bullet hit something!
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, gunProperties.BulletCollisionMask))
                {
                    // if the bullet hit something
                    if (hit.transform)
                    {
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
            

    }

    
}
