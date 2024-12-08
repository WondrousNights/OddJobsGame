using System.Collections;
using System.Numerics;
using AlmenaraGames;
using UnityEngine;
using UnityEngine.Pool;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunScriptableObject : ScriptableObject
{
    
    public GunType Type;
    public AmmoType AmmoType;
    public string Name;
    public GameObject ModelPrefab;
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation;

    public ShootConfigScriptableObject ShootConfig;
    public TrailConfigScriptableObject TrailConfig;

    public int AmmoClipSize;

    private MonoBehaviour ActiveMonoBehaviour;
    private GameObject Model;
    private float LastShootTime;
    private ParticleSystem ShootSystem;
    private ObjectPool<TrailRenderer> TrailPool;

    private Transform parent;

    public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour)
    {
        this.ActiveMonoBehaviour = ActiveMonoBehaviour;
        LastShootTime = 0; // in editor this will not be properly reset, in build it's fine
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);

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

    public void Shoot(Camera shootCam, MuzzleFlash muzzleFlash, PlayerAmmoHandler ammoHandler, int gunIndex)
    {
        if (Time.time > ShootConfig.FireRate + LastShootTime)
        {
            LastShootTime = Time.time;
            ShootSystem.Play();
            muzzleFlash.Play();
            MultiAudioManager.PlayAudioObject(ShootConfig.shootSfx, parent);
            Ray ray = shootCam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;

            ammoHandler.currentAmmo[gunIndex] -= 1;

            Vector3 shootDirection = shootCam.transform.forward;
            if (Physics.Raycast(ray, out hit))
            {
                
                ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        shootCam.transform.position,
                        hit.point,
                        hit,
                        shootCam.transform.position
                    )
                );
            }
            else
            {
                ActiveMonoBehaviour.StartCoroutine(
                    PlayTrail(
                        shootCam.transform.position,
                        shootCam.transform.position + (shootDirection * TrailConfig.MissDistance),
                        new RaycastHit(),
                        shootCam.transform.position
                    )
                );
            }
        }
    }

    private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit, Vector3 shootPos)
    {
        TrailRenderer instance = TrailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = StartPoint;
        yield return null; // avoid position carry-over from last frame if reused

        instance.emitting = true;

        float distance = Vector3.Distance(StartPoint, EndPoint);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(
                StartPoint,
                EndPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance))
            );
            remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = EndPoint;

        if (Hit.collider != null)
        {
            //Particle
            GameObject impactParticle = Instantiate(ShootConfig.impactParticle, Hit.point, Quaternion.identity);

            // rotate the hole visual to shoot away from the mesh it hits
            impactParticle.transform.position = Hit.point + (Hit.normal * 0.01f);
            if (Hit.normal != Vector3.zero)
            impactParticle.transform.rotation = Quaternion.LookRotation(-Hit.normal);
            impactParticle.transform.parent = Hit.collider.transform; // make the bullethole move with the thing it hit
                
            Destroy(impactParticle, 10f);

            if(Hit.collider.TryGetComponent(out IDamageable damageable))
            {
                Debug.Log("HIT DAMAGEABLE");
                damageable.TakeDamage(Model.transform.position, ShootConfig.Damage, ShootConfig.hitForce, Hit.point);
            }
            /*
            if (Hit.transform.GetComponent<Rigidbody>())
            {
                Hit.transform.GetComponent<Rigidbody>().AddForceAtPosition(ray.direction * hitForce, hit.point);
            }
            */
        }

        yield return new WaitForSeconds(TrailConfig.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        TrailPool.Release(instance);
    }

    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = TrailConfig.Color;
        trail.material = TrailConfig.Material;
        trail.widthCurve = TrailConfig.WidthCurve;
        trail.time = TrailConfig.Duration;
        trail.minVertexDistance = TrailConfig.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }


}