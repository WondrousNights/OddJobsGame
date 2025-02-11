using System;
using RootMotion.Dynamics;
using Unity.Behavior;
using Unity.Netcode;
using UnityEngine;

public class Network_HealthManager : NetworkBehaviour
{

    float health = 100;

    [SerializeField] PuppetMaster puppetMaster;

    [SerializeField] bool isPlayer = false;
    PlayerManager playerManager;
    Enemy_Manager enemyManager;
    public bool isDead = false;

    public event Action<Ray> OnDamageTaken;

    void Start()
    {
        if(isPlayer)
        {
            playerManager = GetComponent<PlayerManager>();
        }
        else
        {
            enemyManager = GetComponent<Enemy_Manager>();
        }
    }

    public void TakeDamageRpc(float damage, float hitForce, Ray ray, Vector3 vector3)
    {
        health -= damage;
        OnDamageTaken?.Invoke(ray);

        if(isPlayer) playerManager.playerUIManager.UpdateHealthBar(health, 100f);

        if(health <= 0)
        {
            puppetMaster.state = PuppetMaster.State.Dead;
            isDead = true;

            if(!isPlayer)
            {
                enemyManager.HandleDeath();
                
            }

            if(isPlayer)
            {
                Network_LevelManager levelManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Network_LevelManager>();
                if(levelManager != null)
                {
                    levelManager.CheckIfAllPlayersDeadRpc();
                }
            }
        }


    }

    public void Respawn()
    {
        isDead = false;
        health = 100;
        puppetMaster.state = PuppetMaster.State.Alive;
        
    }

    [Rpc(SendTo.Everyone)]
    public void IncreaseHealthRpc(float amount)
    {
        health += amount;
        if(isPlayer) playerManager.playerUIManager.UpdateHealthBar(health, 100f);
    }
}
