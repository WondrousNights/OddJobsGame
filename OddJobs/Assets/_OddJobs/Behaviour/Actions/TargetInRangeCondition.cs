using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Target in Range", story: "[Target] is in [Range] of [Agent]", category: "Conditions", id: "ac9fa570a857917f79108b84d8cd17c0")]
public partial class TargetInRangeCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Range;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    public override bool IsTrue()
    {
        if(Vector3.Distance(Target.Value.transform.position, Agent.Value.transform.position) <= Range.Value)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
