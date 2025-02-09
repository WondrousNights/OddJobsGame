using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DamagableLimb : NetworkBehaviour, IDamageable
{
    Rigidbody rb;
    [SerializeField] float damageMultiplier;
    [SerializeField] Network_HealthManager healthManager;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    [Rpc(SendTo.Everyone)]
    public void TakeDamageRpc(float damage, float hitForce, Ray ray, Vector3 hitPosition)
    {
        Debug.Log("Dealt damage to limb");
        float damageToDeal = damage * damageMultiplier;
        healthManager.TakeDamageRpc(damageToDeal, hitForce, ray, hitPosition);

        rb.AddForceAtPosition(ray.direction * hitForce, hitPosition, ForceMode.Impulse);
    }
}
