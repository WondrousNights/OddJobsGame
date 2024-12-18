using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckForTargets", story: "Does [Agent] have [Target]", category: "Action", id: "77c2443d74a977d1858f228bfeae578d")]
public partial class CheckForTargetsAction : Action
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

        if(targetDetector.currentTarget != null)
        {
            Target.Value = targetDetector.currentTarget;
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

