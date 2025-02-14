using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Agent Melee attacks Target", story: "[Agent] melee attacks [target]", category: "Action", id: "65be3749b7ca038107127b4dedc1e963")]
public partial class AgentMeleeAttacksTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    EnemyAttackController enemyAttackController;

    protected override Status OnStart()
    {
        enemyAttackController = Agent.Value.GetComponent<EnemyAttackController>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Agent.Value.transform.LookAt(Target.Value.transform);
        enemyAttackController.MeleeEventRpc();
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

