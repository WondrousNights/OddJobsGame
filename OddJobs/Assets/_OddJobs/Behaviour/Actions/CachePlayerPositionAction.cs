using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Cache Player Position", story: "[Agent] caches [AttackPos] from [Gameobject]", category: "Action", id: "b4eb3d5f1af59a1a651ddcb7d7ac6e15")]
public partial class CachePlayerPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Vector3> AttackPos;
    [SerializeReference] public BlackboardVariable<GameObject> Gameobject;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        AttackPos.Value = Gameobject.Value.transform.position;

        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

