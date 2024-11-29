using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Local_PlayerStats : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;

    private float currentHealth = 100;

    [SerializeField] Image currentHealthCanvas;

    [HideInInspector] public bool isDead;

    public EventHandler OnPlayerDeath;
    public EventHandler OnPlayerRevive;
    private void Awake()
    {
        currentHealth = maxHealth;
        currentHealthCanvas.fillAmount = currentHealth / maxHealth;
        isDead = false;
    }

    
    
    public void TakeDamage(int damage)
    {

       
            currentHealth -= damage;
            currentHealthCanvas.fillAmount = currentHealth / maxHealth;

            if (currentHealth <= 0)
            {
                PlayerDeath();
            }
        
        
        //Debug.Log("Health:" + n_CurrentHealth.Value);
       
    }


    private void PlayerDeath()
    {
         StartCoroutine("PlayerDeathCoroutine", 2f);
    }


    IEnumerator PlayerDeathCoroutine(float respawnTime)
    {
        OnPlayerDeath.Invoke(this, EventArgs.Empty);
        isDead = true;

        
        yield return new WaitForSeconds(respawnTime);

        Revive();
        isDead = false;
        OnPlayerRevive.Invoke(this, EventArgs.Empty);

       
    }

   
    void Revive()
    {
        currentHealth = maxHealth;
    }
}
