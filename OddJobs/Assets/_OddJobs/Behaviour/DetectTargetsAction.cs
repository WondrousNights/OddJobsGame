using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DetectTargets", story: "[Agent] detects [Target]", category: "Action", id: "76fa799728324634d2b8aea7e11abf10")]
public partial class DetectTargetsAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    NavMeshAgent agent;
    Local_EnemyDetection enemyDetection;

    protected override Status OnStart()
    {

        agent = Agent.Value.GetComponent<NavMeshAgent>();
        enemyDetection = Agent.Value.GetComponent<Local_EnemyDetection>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {

        var target = enemyDetection.GetCurrentTarget();
        if(target == null) return Status.Running;

        Target.Value = target.gameObject;
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

