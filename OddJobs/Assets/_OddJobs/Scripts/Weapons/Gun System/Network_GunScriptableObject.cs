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
    public Vector3 playerSpread = new Vector3(0.01f, 0.01f);
    public Vector3 enemySpread = new Vector3(0.09f, 0.09f);
    public bool automaticFire = false;
    public float fireRate = 0.25f;
    public int bulletsPerShot = 1;
    public float Damage;
    public int ClipSize;
    public float hitForce = 400;
    public GameObject ModelPrefab;
    public GameObject OtherPlayerModelPrefab;
    public Transform ShootPoint;
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation = new Vector3(0, 90, 0);
    public Vector3 OtherPlayerGunSpawnPos;
    public Vector3 OtherPlayerGunRotation;
    public GameObject DroppedPrefab;
    public ShootConfigScriptableObject ShootConfig;
    public LayerMask BulletCollisionMask;
    public LayerMask hitMask;
    public float range = Mathf.Infinity;
    public AudioObject shootSfx;
    public GameObject impactParticle;
    public GameObject bulletHoleDecal;
    public float maxRecoil;
    public float maxKickback;
    public float recoveryTime;
    public float reloadTime;

    private GameObject Model;
    private float LastShootTime;
    private ParticleSystem ShootSystem;
    private ObjectPool<TrailRenderer> TrailPool;
    private Transform parent;

    


    public void Spawn(Transform Parent)
    {
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

    
}