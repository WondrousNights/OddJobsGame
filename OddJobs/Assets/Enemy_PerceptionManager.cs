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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        behaviorGraph = GetComponent<BehaviorGraphAgent>();
        perceptions = perceptionsParent.GetComponents<IPerception>();



        RegisterPerceptionEvent<GameObject>("OnCurrentTargetLostVisual", HandleTargetLostVisual);
        RegisterPerceptionEvent<GameObject>("OnClosestTargetChanged", HandleClosestTargetChanged);
        RegisterPerceptionEvent<GameObject>("OnTargetSpotted", HandleTargetSpotted);
        RegisterPerceptionEvent<GameObject>("OnTargetLost", HandleTargetLost);

        RegisterPerceptionEvent<List<Vector3>>("OnNavigationPerception", HandleNavigationPerception);

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
        Debug.Log($"[PerceptionManager] Target spotted: {target.name}");
    }

    private void HandleTargetLost(GameObject target)
    {
        Debug.Log($"[PerceptionManager] Target lost: {target.name}");
    }

    private void HandleClosestTargetChanged(GameObject target)
    {
        behaviorGraph.BlackboardReference.SetVariableValue("ClosestTarget", target);
        Debug.Log($"[PerceptionManager] New closest target: {target?.name ?? "None"}");
    }

    private void HandleTargetLostVisual(GameObject target)
    {
        behaviorGraph.BlackboardReference.SetVariableValue("LastTarget", target);
    }


    /* Navigation Perception */
    private void HandleNavigationPerception(List<Vector3> viablePositions)
    {
        behaviorGraph.BlackboardReference.SetVariableValue("ViablePositions", viablePositions);

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
}
