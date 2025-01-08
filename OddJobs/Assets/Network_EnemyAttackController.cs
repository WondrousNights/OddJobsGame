using Unity.Netcode;
using UnityEngine;

public class Network_EnemyAttackController : NetworkBehaviour
{
    [SerializeField] Network_GunScriptableObject enemyGun;

    [SerializeField] LayerMask shootMask;
    
    [SerializeField] Transform shootPoint;


    Network_HealthManager healthManager;
    void Start()
    {
        healthManager = GetComponent<Network_HealthManager>();
    }

    public void Attack()
    {
        if(healthManager.isDead) return;
        
         for(int i = 0; i < enemyGun.ShootConfig.bulletsPerShot; i++)
        {
            Vector3 spread = new Vector3(
                        Random.Range(
                            -enemyGun.ShootConfig.enemySpread.x,
                            enemyGun.ShootConfig.enemySpread.x
                        ),
                        Random.Range(
                            -enemyGun.ShootConfig.enemySpread.y,
                            enemyGun.ShootConfig.enemySpread.y
                        ), 0
                        );

            Ray ray = new Ray(shootPoint.position, shootPoint.forward);
                        
                        //We need to change bullet spread
            ray.origin += spread;

            RaycastHit hit;

                // bullet hit something!
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, shootMask))
                {
                    // if the bullet hit something
                    if (hit.transform)
                    {
                        if(hit.transform.GetComponent<DamagableLimb>())
                        {
                            var limb = hit.transform.GetComponent<DamagableLimb>();
                            HitLimbRpc(hit.transform.name, ray, enemyGun.ShootConfig.hitForce, hit.point, hit.normal, limb, enemyGun.ShootConfig.Damage);
                        }

                    }

                }
        }
    }

 


    [Rpc(SendTo.Everyone)]
    void HitLimbRpc(string name, Ray ray, float hitForce, Vector3 hitPoint, Vector3 hitNormal, NetworkBehaviourReference damageableLimb, float damage)
    {

        if (damageableLimb.TryGet(out DamagableLimb limb))
        {
           Debug.Log("An enemy just hit" + name);

         
        GameObject impactParticle = Instantiate(enemyGun.ShootConfig.impactParticle, hitPoint, Quaternion.identity);
        impactParticle.transform.position = hitPoint + (hitNormal * 0.01f);
        if (hitNormal != Vector3.zero) impactParticle.transform.rotation = Quaternion.LookRotation(-hitNormal);
        Destroy(impactParticle, 10f);

        // spawn bullet hole decal
        GameObject bulletHoleDecal = Instantiate(enemyGun.ShootConfig.bulletHoleDecal, hitPoint, Quaternion.identity);
        bulletHoleDecal.transform.position = hitPoint + (hitNormal * 0.01f);
        
        bulletHoleDecal.transform.parent = limb.transform; // make the bullethole decal move with the thing it hit
       
        Quaternion normalRotation = Quaternion.LookRotation(-hitNormal); // Create rotation that faces away from the surface
        bulletHoleDecal.transform.rotation = normalRotation * Quaternion.Euler(0, 0, Random.Range(0, 360)); // Add random rotation around that
        Destroy(bulletHoleDecal, 60f);
        

                        // elias: note to self, need to use hit.transform here instead of hit.collider because the collider is not always the parent of the object hit
                        // If the object hit has a rigidbody, apply a force to it
        
        
        limb.transform.GetComponent<Rigidbody>().AddForceAtPosition(ray.direction * hitForce, hitPoint, ForceMode.Impulse);
        limb.DoDamage(damage);
               
        }                      
    }
}
