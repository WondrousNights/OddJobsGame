using System.Collections;
using System.Numerics;
//using AlmenaraGames;
using UnityEngine;
using UnityEngine.Pool;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Unity.Netcode;
using Vector2 = UnityEngine.Vector2;

[CreateAssetMenu(fileName = "GunProperties", menuName = "Guns/GunProperties", order = 0)]
public class Network_GunProperties : ScriptableObject
{
    [Header("Gun Settings")]
    public GunType Type;
    public AmmoType AmmoType;
    public string Name;

    
    public int ClipSize;
    public GameObject ModelPrefab;
    public Vector3 PlayerGunSpawnPoint;
    public Vector3 PlayerGunSpawnRotation;
    public Vector3 VisuaGunSpawnPos;
    public Vector3 VisualGunRotation;
    public Transform ShootPoint;
    public GameObject DroppedPrefab;
    public LayerMask BulletCollisionMask;


    [Header("Shoot Settings")]
    public float Damage;
    public LayerMask hitMask;
    public Vector2 playerSpread = new Vector3(0.01f, 0.01f);
    public Vector2 enemySpread = new Vector3(0.09f, 0.09f);
    public bool automaticFire = false;
    public float fireRate = 0.25f;
    public int bulletsPerShot = 1;
    public GameObject impactParticle;
    public GameObject bulletHoleDecal;
    public float maxRecoil;
    public float maxKickback;
    public float hitForce;
    public float recoveryTime;
    public float reloadTime;
    public float range;

    [Header("Audio Settings")]
    public AudioClip shootSfx;
    public AudioClip reloadSfx;
    
}