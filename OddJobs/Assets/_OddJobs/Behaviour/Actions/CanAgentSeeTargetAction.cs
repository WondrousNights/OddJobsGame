using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Can Agent see Target", story: "Can [Agent] see [Target]", category: "Action", id: "13418561318ce13181c28b8ddcd2bb06")]
public partial class CanAgentSeeTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    TargetDetector targetDetector;

    protected override Status OnStart()
    {
        targetDetector = Agent.Value.GetComponent<TargetDetector>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(targetDetector.CanSeeTarget())
        {
            return Status.Success;
        }
        else
        {
            return Status.Failure;
        }
        
    }

    protected override void OnEnd()
    {
    }
}

