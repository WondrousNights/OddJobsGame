using System;
using RootMotion.Dynamics;
using Unity.Behavior;
using Unity.Netcode;
using UnityEngine;

public class Network_HealthManager : NetworkBehaviour
{

    public float health = 100;
    public float MaxHealth;
    public PuppetMaster puppetMaster;
    public bool isDead = false;
    public event Action<Ray> OnDamageTaken;

    void Start()
    {
        health = MaxHealth;
    }

    public virtual void TakeDamageRpc(float damage, float hitForce, Ray ray, Vector3 vector3)
    {
        health -= damage;
        OnDamageTaken?.Invoke(ray);

        if(health <= 0)
        {
            puppetMaster.state = PuppetMaster.State.Dead;
            isDead = true;
        }


    }

    [Rpc(SendTo.Everyone)]
    public virtual void IncreaseHealthRpc(float amount)
    {
        health += amount;
        if(health >= MaxHealth)
        {
            health = MaxHealth;
        }
    }

    public void Respawn()
    {
        isDead = false;
        health = MaxHealth;
        puppetMaster.state = PuppetMaster.State.Alive;
        
    }
}
