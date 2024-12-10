using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Rotate Toward Target", story: "[Agent] Rotates to [Target]", category: "Action", id: "ad06b1c5fb759f87069a78711686def7")]
public partial class RotateTowardTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {

        Agent.Value.transform.LookAt(Target.Value.transform.position);

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

