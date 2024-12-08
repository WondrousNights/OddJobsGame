using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsRagdoll", story: "[Agent] is [Ragdoll]", category: "Conditions", id: "2452ff8ef73bca13c01056dec870343a")]
public partial class IsRagdollCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<bool> Ragdoll;

    EnemyHealth enemyHealth;
    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
        enemyHealth = Agent.Value.GetComponent<EnemyHealth>();

    }

    public override void OnEnd()
    {
    }
}
