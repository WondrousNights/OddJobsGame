using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "HasTarget", story: "[Agent] has a [Target]", category: "Conditions", id: "d0201d4dc09076d9e2d0f3523689dca5")]
public partial class HasTargetCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    public override bool IsTrue()
    {
        if(Target.Value == null)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
