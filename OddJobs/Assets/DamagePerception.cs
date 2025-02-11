using System;
using UnityEngine;

public class DamagePerception : MonoBehaviour, IPerception
{
    Enemy_PerceptionManager PerceptionManager;

    [SerializeField] Network_HealthManager healthManager;

    public void SetManager(Enemy_PerceptionManager perceptionManager)
    {
        PerceptionManager = perceptionManager;
    }

    public void StartPerception()
    {
        healthManager.OnDamageTaken += HandleDamageTaken;
    }

    private void HandleDamageTaken(Ray fromPos)
    {
        PerceptionManager.InvokePerceptionEvent("OnDamageTaken", fromPos);
    } 

    public void UpdatePerception()
    {
       
    }
}
