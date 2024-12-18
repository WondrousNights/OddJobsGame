using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AmIRagdoll", story: "Am [I] [Ragdoll]", category: "Action", id: "0943acc667cbd372ff114e484ae54e0e")]
public partial class AmIRagdollAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> I;
    [SerializeReference] public BlackboardVariable<bool> Ragdoll;
    EnemyHealth enemyHealth;

    protected override Status OnStart()
    {
        enemyHealth = I.Value.GetComponent<EnemyHealth>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {

        Ragdoll.Value = enemyHealth.GetIsRagdoll();

        if(Ragdoll.Value == true)
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

