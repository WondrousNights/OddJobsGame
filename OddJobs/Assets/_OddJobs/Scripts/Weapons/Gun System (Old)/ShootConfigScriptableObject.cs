//using AlmenaraGames;
using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Guns/Shoot Config", order = 2)]
public class ShootConfigScriptableObject : ScriptableObject
{
    public float Damage;
    public LayerMask hitMask;
    public Vector2 playerSpread = new Vector3(0.01f, 0.01f);
    public Vector2 enemySpread = new Vector3(0.09f, 0.09f);
    public bool automaticFire = false;
    public float fireRate = 0.25f;
    public int bulletsPerShot = 1;
    //public AudioObject shootSfx;
    public GameObject impactParticle;
    public GameObject bulletHoleDecal;

    public float maxRecoil;
    public float maxKickback;
    public float hitForce;
    public float recoveryTime;
    public float reloadTime;

    public float range;
    
}
