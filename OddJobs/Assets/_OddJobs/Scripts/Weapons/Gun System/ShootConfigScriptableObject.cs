using AlmenaraGames;
using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Guns/Shoot Config", order = 2)]
public class ShootConfigScriptableObject : ScriptableObject
{
    public LayerMask hitMask;
    public Vector3 enemySpread = new Vector3(0.1f, 0.1f, 0.1f);
    public float fireRate = 0.25f;
    public AudioObject shootSfx;
    public GameObject impactParticle;
    public GameObject bulletHoleDecal;

    public float maxRecoil;
    public float maxKickback;
    public float hitForce;
    public float reloadTime;

    public float Damage;
    
}
