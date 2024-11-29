using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkEnemyStats : NetworkBehaviour
{
    [SerializeField] float enemyMaxHealth = 100;

    private NetworkVariable<float> n_EnemyCurrentHealth = new NetworkVariable<float>();

    NetworkEnemyMovement nEnemyMovement;

    [SerializeField] Image currentHealthCanvas;
    private void Awake()
    {
        nEnemyMovement = GetComponent<NetworkEnemyMovement>();
        n_EnemyCurrentHealth.Value = enemyMaxHealth;
        UpdateCanvas();
    }

    public void TakeDamage(int damage)
    {

        TakeDamageServerRpc(damage);
       
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int damage)
    {
        n_EnemyCurrentHealth.Value -= damage;
       
        if(n_EnemyCurrentHealth.Value <= 20)
        {
            nEnemyMovement.FleeServerRpc();
        }

        if (n_EnemyCurrentHealth.Value <= 0)
        {
            EnemyDeath();

        }
        //Debug.Log("Health:" + n_CurrentHealth.Value);
        UpdateCanvas();
    }

    private void EnemyDeath()
    {
        EnemyDeathServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void EnemyDeathServerRpc()
    {
        EnemyDeathClientRpc();
    }

    [ClientRpc]
    private void EnemyDeathClientRpc()
    {
        Destroy(this.gameObject);
    }


    private void UpdateCanvas()
    {

        UpdateCanvasServerRpc();
  
    }
    [ServerRpc(RequireOwnership = false)]
    private void UpdateCanvasServerRpc()
    {
        UpdateCanvasClientRpc();
    }
    [ClientRpc]
    private void UpdateCanvasClientRpc()
    {
        currentHealthCanvas.fillAmount = n_EnemyCurrentHealth.Value / enemyMaxHealth;
    }

}


