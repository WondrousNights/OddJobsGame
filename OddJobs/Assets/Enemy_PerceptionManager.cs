using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class Enemy_PerceptionManager : MonoBehaviour
{
    [SerializeField] GameObject perceptionsParent;
    IPerception[] perceptions;

    private Dictionary<string, Action<object>> perceptionEvents = new Dictionary<string, Action<object>>();

    BehaviorGraphAgent behaviorGraph;

    [SerializeField] LayerMask squadMask;
    [SerializeField] LayerMask playerMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        behaviorGraph = GetComponent<BehaviorGraphAgent>();
        perceptions = perceptionsParent.GetComponents<IPerception>();



        RegisterPerceptionEvent<GameObject>("OnCurrentTargetLostVisual", HandleTargetLostVisual);
        RegisterPerceptionEvent<GameObject>("OnClosestTargetChanged", HandleClosestTargetChanged);
        RegisterPerceptionEvent<GameObject>("OnTargetSpotted", HandleTargetSpotted);
        RegisterPerceptionEvent<GameObject>("OnTargetLost", HandleTargetLost);
        RegisterPerceptionEvent<GameObject>("OnEnemySpottedTarget", HandleEnemySpottedTarget);

        RegisterPerceptionEvent<List<Vector3>>("OnNavigationPerception", HandleNavigationPerception);

        RegisterPerceptionEvent<Ray>("OnDamageTaken", HandleDamageTaken);

        foreach(IPerception perception in perceptions)
        {
            perception.SetManager(this);
            perception.StartPerception();
        }
    }




    //Event Handlers

    /* Target Perception */
    private void HandleTargetSpotted(GameObject target)
    {
       // Debug.Log($"[PerceptionManager] Target spotted: {target.name}");
    }

    private void HandleTargetLost(GameObject target)
    {
       // Debug.Log($"[PerceptionManager] Target lost: {target.name}");
    }

    private void HandleClosestTargetChanged(GameObject target)
    {
        behaviorGraph.BlackboardReference.SetVariableValue("ClosestTarget", target);

        AlertNearbyAllies(target);
        //Debug.Log($"[PerceptionManager] New closest target: {target?.name ?? "None"}");
    }

    private void HandleTargetLostVisual(GameObject target)
    {
        behaviorGraph.BlackboardReference.SetVariableValue("LastTarget", target);
        //behaviorGraph.BlackboardReference.SetVariableValue("LastTargetPos", target.transform.position);
    }
    
    private void HandleEnemySpottedTarget(GameObject target)
    {
        bool hasCurrentTarget = behaviorGraph.BlackboardReference.GetVariableValue<GameObject>("CurrentTarget", out GameObject currentTarget);
        bool hasLastTarget = behaviorGraph.BlackboardReference.GetVariableValue<GameObject>("LastTarget", out GameObject lastTarget);

        if(currentTarget == null && lastTarget == null)
        {
            //Debug.Log("Going in for backup!");
            behaviorGraph.BlackboardReference.SetVariableValue("LastTarget", target);
            //behaviorGraph.BlackboardReference.SetVariableValue("LastTargetPos", target.transform.position);
        }
    }



    /* Navigation Perception */
    private void HandleNavigationPerception(List<Vector3> viablePositions)
    {
        behaviorGraph.BlackboardReference.SetVariableValue("ViablePositions", viablePositions);

    }


    //Damage Perception
    private void HandleDamageTaken(Ray ray)
    {
        Collider[] players = Physics.OverlapSphere(ray.origin, 1f, playerMask);

        foreach(Collider col in players)
        {

            PlayerManager player = col.GetComponent<PlayerManager>();
            if(player != null)
            {
                behaviorGraph.BlackboardReference.SetVariableValue("LastTarget", player.gameObject);
            }
        
        }

        
    }


    // Update is called once per frame
    void Update()
    {
        foreach(IPerception perception in perceptions)
        {
            perception.UpdatePerception();
        }
    }

    public void RegisterPerceptionEvent<T>(string eventName, Action<T> action)
    {
        perceptionEvents[eventName] = (obj) => action((T)obj);
    }

    // Invoke a perception event with any data type
    public void InvokePerceptionEvent(string eventName, object data)
    {
        if (perceptionEvents.TryGetValue(eventName, out var action))
        {
            action(data);
        }
    }

    void AlertNearbyAllies(GameObject target)
    {
        float alertRadius = 50f;  // How far allies can hear the alert
        Collider[] nearbyAllies = Physics.OverlapSphere(transform.position, alertRadius, squadMask);

        foreach(Collider col in nearbyAllies)
        {
            if(col.gameObject == this.gameObject) continue;

            Enemy_PerceptionManager allyPerception = col.GetComponent<Enemy_PerceptionManager>();
            if(allyPerception != null)
            {
                allyPerception.InvokePerceptionEvent("OnEnemySpottedTarget", target);
            }
        
        }
    }



}
