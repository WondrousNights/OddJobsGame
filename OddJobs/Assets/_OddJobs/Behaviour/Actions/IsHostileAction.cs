using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsHostile", story: "Is [Agent] Hostile?", category: "Action", id: "f8f05a00779f481c3bdb02914e987da4")]
public partial class IsHostileAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    EnemyManager enemyManager;

    protected override Status OnStart()
    {
        enemyManager = Agent.Value.GetComponent<EnemyManager>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(enemyManager.isHostile)
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

