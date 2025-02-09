using Unity.Netcode;
using UnityEngine;

public class EnemyAttackController : NetworkBehaviour
{

    [SerializeField] Weapon weapon;
    [SerializeField] Transform shootPoint;

    void Start()
    {
        weapon.ShowWeapon();
    }
    
    [Rpc(SendTo.Everyone)]
    public void AttackEventRpc()
    {
        Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        Debug.Log("Attack event fired!");
        weapon.UseWeapon(ray);
        weapon.ShootEffects();
    }
    
   
}
