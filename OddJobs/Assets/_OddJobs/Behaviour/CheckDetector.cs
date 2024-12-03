using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckDetector", story: "Check if [EnemyDetector] has a [Target]", category: "Action", id: "286a268b6e75bddf547e884b50948f77")]
public partial class CheckDetectorAction : Action
{
    [SerializeReference] public BlackboardVariable<Local_EnemyDetection> EnemyDetector;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    protected override Status OnStart()
    {

        if(EnemyDetector.Value.GetCurrentTarget() == null)
        {
            return Status.Failure;
        }
        else
        {
            Target.Value = EnemyDetector.Value.GetCurrentTarget();
            return Status.Success;
        }
        
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

