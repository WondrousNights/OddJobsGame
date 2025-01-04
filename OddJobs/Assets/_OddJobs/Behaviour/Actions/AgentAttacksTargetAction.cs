using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Agent Attacks Target", story: "[Agent] Attacks [Target]", category: "Action", id: "b67b44b7e3c94d0ceabe9df995c8070d")]
public partial class AgentAttacksTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    Network_EnemyAttackController enemyAttackController;
    TargetDetector targetDetector;

    protected override Status OnStart()
    {
        enemyAttackController = Agent.Value.GetComponent<Network_EnemyAttackController>();
        targetDetector = Agent.Value.GetComponent<TargetDetector>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Agent.Value.transform.LookAt(Target.Value.transform);
        enemyAttackController.Attack();
        
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

