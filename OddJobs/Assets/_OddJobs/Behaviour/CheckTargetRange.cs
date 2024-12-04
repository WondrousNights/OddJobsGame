using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckTargetRange", story: "Is [Target] in [Range] of [Agent]", category: "Action", id: "61cd464643f448a27b32b24549b17ad3")]
public partial class CheckTargetRangeAction : Action
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

