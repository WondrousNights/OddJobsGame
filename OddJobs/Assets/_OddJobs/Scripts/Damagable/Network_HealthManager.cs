using RootMotion.Dynamics;
using Unity.Behavior;
using Unity.Netcode;
using UnityEngine;

public class Network_HealthManager : NetworkBehaviour, IDamageable
{

    float health = 100;

    [SerializeField] PuppetMaster puppetMaster;

    [SerializeField] bool isPlayer = false;
    PlayerManager playerManager;
    public bool isDead = false;

    void Start()
    {
        if(isPlayer)
        {
            playerManager = GetComponent<PlayerManager>();
        }
    }

    public void TakeDamageRpc(float damage, float hitForce, Ray ray, Vector3 vector3)
    {
        health -= damage;

        if(isPlayer) playerManager.playerUIManager.UpdateHealthBar(damage, 100f);


        if(health <= 0)
        {
            puppetMaster.state = PuppetMaster.State.Dead;
            isDead = true;

            if(!isPlayer)
            {
                GetComponent<BehaviorGraphAgent>().End();
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
}
