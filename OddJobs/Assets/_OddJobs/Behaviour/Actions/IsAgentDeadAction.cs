using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsAgentDead", story: "Is [Agent] Dead", category: "Action", id: "2364b5ca980e34128e5f10acfa9a605e")]
public partial class IsAgentDeadAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;


    EnemyHealth enemyHealth;
    protected override Status OnStart()
    {
        enemyHealth = Agent.Value.GetComponent<EnemyHealth>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(enemyHealth.isDead)
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

