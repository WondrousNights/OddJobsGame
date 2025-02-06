using UnityEngine;

[CreateAssetMenu(fileName = "WeaponProperties", menuName = "Weapons/GunProperties", order = 0)]
public class Network_GunProperties : Network_WeaponProperties
{
    [Header("Shoot Settings")]
    public LayerMask BulletCollisionMask;
    public AmmoType AmmoType;
    public int ClipSize;
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
