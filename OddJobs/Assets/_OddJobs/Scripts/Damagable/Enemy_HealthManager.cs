using UnityEngine;

public class Enemy_HealthManager : Network_HealthManager
{
    Enemy_Manager enemyManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyManager = GetComponent<Enemy_Manager>();
    }

    public override void TakeDamageRpc(float damage, float hitForce, Ray ray, Vector3 vector3)
    {
        base.TakeDamageRpc(damage, hitForce, ray, vector3);

        if(health <= 0)
        {
            enemyManager.HandleDeath();
        }
    }
}
