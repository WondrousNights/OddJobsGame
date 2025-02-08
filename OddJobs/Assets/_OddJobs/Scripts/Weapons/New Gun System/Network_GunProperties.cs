using UnityEngine;

[CreateAssetMenu(fileName = "WeaponProperties", menuName = "Weapons/GunProperties", order = 0)]
public class Network_GunProperties : Network_WeaponProperties
{
    [Header("Shoot Settings")]
    public LayerMask BulletCollisionMask;
    public AmmoType AmmoType;
    public int ClipSize;
    public float Damage;
    public float bulletSpread = 0;
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
