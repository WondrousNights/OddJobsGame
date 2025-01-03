using System.Collections;
using System.Numerics;
using AlmenaraGames;
using UnityEngine;
using UnityEngine.Pool;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Unity.Netcode;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Network_Gun", order = 0)]
public class Network_GunScriptableObject : ScriptableObject
{
    
    public GunType Type;
    public AmmoType AmmoType;
    public string Name;
    public GameObject ModelPrefab;
    public GameObject OtherPlayerModelPrefab;
    public Transform ShootPoint;
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation = new Vector3(0, 90, 0);
    
    public Vector3 OtherPlayerGunSpawnPos;
    public Vector3 OtherPlayerGunRotation;
    public GameObject DroppedPrefab;

    public ShootConfigScriptableObject ShootConfig;

    public int ClipSize;

    private MonoBehaviour ActiveMonoBehaviour;
    private GameObject Model;
    private float LastShootTime;
    private ParticleSystem ShootSystem;
    private ObjectPool<TrailRenderer> TrailPool;

    private Transform parent;

    public LayerMask BulletCollisionMask;


    public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour)
    {
        this.ActiveMonoBehaviour = ActiveMonoBehaviour;
        LastShootTime = 0; // in editor this will not be properly reset, in build it's fine
        // TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);

        Model = Instantiate(ModelPrefab);
        Model.transform.SetParent(Parent, false);
        Model.transform.localPosition = SpawnPoint;
        Model.transform.localRotation = Quaternion.Euler(SpawnRotation);
        
        ShootSystem = Model.GetComponentInChildren<ParticleSystem>();

        parent = Parent;
    }

   

    public void Despawn()
    {
        // We do a bunch of other stuff on the same frame, so we really want it to be immediately destroyed, not at Unity's convenience.

        TrailPool.Clear();
        ShootSystem = null;
    }

    public void Shoot(Transform shootPoint = null)
    {
        // if (shootPoint == null) shootPoint = ShootPoint;
        if (Time.time > ShootConfig.fireRate + LastShootTime)
        {
            LastShootTime = Time.time;
            ShootSystem.Play();
        
            Ray ray = new Ray(shootPoint.position, shootPoint.forward);
            Vector3 offset = new Vector3(shootPoint.position.x, shootPoint.position.y, shootPoint.position.z);
            RaycastHit hit;

            for(int i = 0; i < ShootConfig.bulletsPerShot; i++)
            {
                // add bullet spread
                Vector3 spread = new Vector3(
                    Random.Range(
                        -ShootConfig.playerSpread.x,
                        ShootConfig.playerSpread.x
                    ),
                    Random.Range(
                        -ShootConfig.playerSpread.y,
                        ShootConfig.playerSpread.y
                    ), 0
                );
                ray.origin = offset += spread;

                
                
                // bullet hit something!
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, BulletCollisionMask))
                {
                    // if the bullet hit something, spawn a bullet hole on it and apply damage/force
                    if (hit.collider)
                    {

                        Debug.Log("I just hit : " + hit.collider.gameObject.name);
                        // spawn hit particle effects, rotating it to face away from the surface it hit
                        GameObject impactParticle = Instantiate(ShootConfig.impactParticle, hit.point, Quaternion.identity);
                        impactParticle.transform.position = hit.point + (hit.normal * 0.01f);
                        if (hit.normal != Vector3.zero) impactParticle.transform.rotation = Quaternion.LookRotation(-hit.normal);
                        Destroy(impactParticle, 10f);

                        // spawn bullet hole decal
                        GameObject bulletHoleDecal = Instantiate(ShootConfig.bulletHoleDecal, hit.point, Quaternion.identity);
                        bulletHoleDecal.transform.position = hit.point + (hit.normal * 0.01f);
                        bulletHoleDecal.transform.parent = hit.collider.transform; // make the bullethole decal move with the thing it hit
                        Quaternion normalRotation = Quaternion.LookRotation(-hit.normal); // Create rotation that faces away from the surface
                        bulletHoleDecal.transform.rotation = normalRotation * Quaternion.Euler(0, 0, Random.Range(0, 360)); // Add random rotation around that
                        Destroy(bulletHoleDecal, 60f);

                        // elias: note to self, need to use hit.transform here instead of hit.collider because the collider is not always the parent of the object hit
                        // If the object hit has a rigidbody, apply a force to it
                        if (hit.transform.GetComponent<Rigidbody>())
                        {
                            hit.transform.GetComponent<Rigidbody>().AddForceAtPosition(ray.direction * ShootConfig.hitForce, hit.point, ForceMode.Impulse);
                        }

                        // If the object hit has a damageable component, apply damage to it
                        if(hit.transform.TryGetComponent(out IDamageable damageable))
                        {
                            damageable.TakeDamageFromGun(ray, ShootConfig.Damage, ShootConfig.hitForce, hit.point, parent.gameObject, ShootConfig.recoveryTime);
                        }

                        // If the object hit has a damageable component in its parent, apply damage to it
                        if(hit.transform.GetComponentInParent<IDamageable>() != null)
                        {
                            hit.transform.GetComponentInParent<IDamageable>().TakeDamageFromGun(ray, ShootConfig.Damage, ShootConfig.hitForce, hit.point, parent.gameObject, ShootConfig.recoveryTime);
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
    }
}