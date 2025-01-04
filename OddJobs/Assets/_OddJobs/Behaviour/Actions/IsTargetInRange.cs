using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Is Target In Range", story: "Is [Target] In [Range] of [Agent]", category: "Action", id: "c84c2fcc4e946074e5b1fb7cb47d25fb")]
public partial class IsTargetOutOfRangeAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Range;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(Vector3.Distance(Target.Value.transform.position, Agent.Value.transform.position) <= Range.Value)
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

