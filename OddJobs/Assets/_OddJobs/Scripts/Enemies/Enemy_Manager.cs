using Unity.Behavior;
using UnityEngine;
using System;
using Action = System.Action;

public class Enemy_Manager : MonoBehaviour
{
    public EnemyPersonality enemyPersonality;
    public bool isDead = false;

    public event Action OnDeath;

    public void HandleDeath()
    {
        GetComponent<BehaviorGraphAgent>().End();
        isDead = true;
        OnDeath?.Invoke();
    }
}
