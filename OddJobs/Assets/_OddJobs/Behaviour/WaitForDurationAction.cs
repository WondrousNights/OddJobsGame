using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "WaitForDuration", story: "Wait for [Duration]", category: "Action", id: "9a0223d6d7ff123f410f38140803909a")]
public partial class WaitForDurationAction : Action
{
    [SerializeReference] public BlackboardVariable<float> Duration;
    

    float count = 0;
    protected override Status OnStart()
    {
        count = 0;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        count += Time.deltaTime;
        if(count >= Duration.Value)
        {
            return Status.Success;
        }
        else
        {
            return Status.Running;
        }
        
    }

    protected override void OnEnd()
    {
    }
}

