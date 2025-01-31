using Unity.Netcode;
using UnityEngine;

public class DamagableLimb : NetworkBehaviour
{
    [SerializeField] float damageMultiplier;
    [SerializeField] Network_HealthManager healthManager;
    

    public void DoDamage(float damage)
    {
        float damageToDeal = damage * damageMultiplier;
        healthManager.DoDamage(damageToDeal);
    }
}
