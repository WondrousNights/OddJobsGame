using Unity.Netcode;
using UnityEngine;

public class EnemyAttackController : NetworkBehaviour
{

    [SerializeField] Weapon weapon;
    [SerializeField] Transform shootPoint;
    Enemy_Manager enemy_Manager;

    void Start()
    {
        enemy_Manager = GetComponent<Enemy_Manager>();
        weapon.ShowWeapon();

        enemy_Manager.OnDeath += DisableWeapon;
    }
    
    [Rpc(SendTo.Everyone)]
    public void AttackEventRpc()
    {
        Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        Debug.Log("Attack event fired!");
        if (Time.time > weapon.weaponProperties.fireRate + weapon.LastShootTime)
        {
            weapon.UseWeapon(ray, false);
            weapon.ShootEffects();
        }
       
    }

    void DisableWeapon()
    {
        weapon.HideWeapon();
    }
    
   
}
