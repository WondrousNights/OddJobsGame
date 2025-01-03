using Unity.Netcode;
using UnityEngine;

public class DamagableLimb : NetworkBehaviour
{
    [SerializeField] int damageMultiplier;
    [SerializeField] Network_HealthManager healthManager;
    

    public void DoDamage()
    {
        healthManager.DoDamage(damageMultiplier);
    }
}
