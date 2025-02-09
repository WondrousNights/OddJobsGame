using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class Enemy_PerceptionManager : MonoBehaviour
{
    [SerializeField] GameObject perceptionsParent;
    IPerception[] perceptions;

    BehaviorGraphAgent behaviorGraph;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        behaviorGraph = GetComponent<BehaviorGraphAgent>();
        perceptions = perceptionsParent.GetComponents<IPerception>();

        foreach(IPerception perception in perceptions)
        {
            perception.OnTargetSpotted += HandleTargetSpotted;
            perception.OnTargetLost  += HandleTargetLost;
            perception.OnClosestTargetChanged  += HandleClosestTargetChanged;
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
}
