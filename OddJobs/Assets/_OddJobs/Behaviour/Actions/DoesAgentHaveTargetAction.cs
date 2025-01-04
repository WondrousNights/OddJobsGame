using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DoesAgentHaveTarget", story: "Does [Agent] Have [Target]", category: "Action", id: "8597465285238bbf4e207d9791a0c057")]
public partial class DoesAgentHaveTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(Target.Value == null)
        {
            return Status.Failure;
        }
        else
        {
            return Status.Success;
        }
        
    }

    protected override void OnEnd()
    {
    }
}

