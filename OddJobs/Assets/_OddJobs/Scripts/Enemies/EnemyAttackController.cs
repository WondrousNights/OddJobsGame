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
    public void ShootEventRpc()
    {
        if(enemy_Manager.isDead) return;
        Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        Debug.Log("Attack event fired!");
        if (Time.time > weapon.gunProperties.fireRate + weapon.LastShootTime)
        {
            weapon.UseWeapon(ray, false);
            weapon.ShootEffects();
        }
       
    }

    [Rpc(SendTo.Everyone)]
    public void MeleeEventRpc()
    {
        if(enemy_Manager.isDead) return;
        Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        Debug.Log("Attack event fired!");

            weapon.UseWeapon(ray, false);
            weapon.ShootEffects();
        
    }

    void DisableWeapon()
    {
        weapon.HideWeapon();
    }
    
   
}
