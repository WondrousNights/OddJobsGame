using RootMotion.Dynamics;
using Unity.Behavior;
using Unity.Netcode;
using UnityEngine;

public class Network_HealthManager : NetworkBehaviour
{

    float health = 100;

    [SerializeField] PuppetMaster puppetMaster;

    [SerializeField] bool isPlayer = false;
    [SerializeField] Network_PlayerUI playerUI;

    public bool isDead = false;

    public void DoDamage(float damage)
    {
        health -= damage;

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

        if(isPlayer)
        {
            playerUI.UpdateHealthImage(health, 100f);
        }
    }

    public void Respawn()
    {
        isDead = false;
        health = 100;
        puppetMaster.state = PuppetMaster.State.Alive;
        playerUI.UpdateHealthImage(health, 100f);
    }
}
