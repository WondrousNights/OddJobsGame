using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] float maxHealth = 100;

    private NetworkVariable<float> n_CurrentHealth = new NetworkVariable<float>();

    [SerializeField] Image currentHealthCanvas;

    [HideInInspector] public bool isDead;

    public EventHandler OnPlayerDeath;
    public EventHandler OnPlayerRevive;
    private void Awake()
    {
        n_CurrentHealth.Value = maxHealth;
        currentHealthCanvas.fillAmount = n_CurrentHealth.Value / maxHealth;
        isDead = false;
    }

    
    
    public void TakeDamage(int damage)
    {

        if (IsHost && IsServer && IsOwner)
        {
            this.n_CurrentHealth.Value -= damage;
            currentHealthCanvas.fillAmount = n_CurrentHealth.Value / maxHealth;

            if (n_CurrentHealth.Value <= 0)
            {
                PlayerDeath();
            }
        }
        else
        {
            TakeDamageServerRpc(damage);
        }
        
        //Debug.Log("Health:" + n_CurrentHealth.Value);
       
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int damage)
    {
        this.n_CurrentHealth.Value -= damage;

        if (n_CurrentHealth.Value <= 0)
        {
            PlayerDeath();
        }
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { NetworkObject.OwnerClientId }
            }
        };

        TakeDamageClientRpc(clientRpcParams);
        
        
    }

   

    [ClientRpc]
    public void TakeDamageClientRpc(ClientRpcParams clientRpcParams = default)
    {
        Debug.Log(n_CurrentHealth.Value + this.NetworkObject.OwnerClientId);

        currentHealthCanvas.fillAmount = n_CurrentHealth.Value / maxHealth;
    }



    private void PlayerDeath()
    {
        PlayerDeathServerRpc();
    }

    [ServerRpc(RequireOwnership = true)]
    private void PlayerDeathServerRpc()
    {
        PlayerDeathClientRpc();
    }

    [ClientRpc]
    private void PlayerDeathClientRpc()
    {
        
        StartCoroutine("PlayerDeathCoroutine", 2f);
    }

    IEnumerator PlayerDeathCoroutine(float respawnTime)
    {
        OnPlayerDeath.Invoke(this, EventArgs.Empty);
        isDead = true;

        
        yield return new WaitForSeconds(respawnTime);

        ReviveServerRpc();
        isDead = false;
        OnPlayerRevive.Invoke(this, EventArgs.Empty);

       
    }

    [ServerRpc(RequireOwnership = false)]
    void ReviveServerRpc()
    {
        n_CurrentHealth.Value = maxHealth;
    }
}
