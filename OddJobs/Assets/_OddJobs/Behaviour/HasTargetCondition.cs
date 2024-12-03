using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "HasTarget", story: "[Agent] has [Target]", category: "Conditions", id: "c3373159d955ddcbdcaeb9fba57818ab")]
public partial class HasTargetCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    Local_EnemyDetection enemyDetection;

    public override bool IsTrue()
    {
        if(enemyDetection.GetCurrentTarget() != null)
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
        enemyDetection = Agent.Value.GetComponent<Local_EnemyDetection>();
    }

    public override void OnEnd()
    {
    }
}
