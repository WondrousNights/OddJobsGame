using UnityEngine;

public class Network_PlayerHealthManager : Network_HealthManager
{
    PlayerManager playerManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }


    public override void TakeDamageRpc(float damage, float hitForce, Ray ray, Vector3 vector3)
    {
       base.TakeDamageRpc(damage, hitForce, ray, vector3);

       playerManager.playerUIManager.UpdateHealthBar(health, 100f);

        if(health <= 0)
        {
            Network_LevelManager levelManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<Network_LevelManager>();
            if(levelManager != null)
            {
                    levelManager.CheckIfAllPlayersDeadRpc();
            }
            
        }

        
    }

    public override void IncreaseHealthRpc(float amount)
    {
        base.IncreaseHealthRpc(amount);
        playerManager.playerUIManager.UpdateHealthBar(health, 100f);
    }


}
