using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Agent Searches For Target", story: "[Agent] Searches for [Target]", category: "Action", id: "df47ea3fe4c58473df7bf19e55c46329")]
public partial class AgentSearchesForTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    TargetDetector targetDetector;

    protected override Status OnStart()
    {
        targetDetector = Agent.Value.GetComponent<TargetDetector>();
        targetDetector.OnFoundTarget += HandleTargetFound;
        return Status.Running;
    }

  

    protected override Status OnUpdate()
    {
        if(Target.Value == null)
        {
            return Status.Running;
        }
        else
        {
            return Status.Success;
        }
    }

    protected override void OnEnd()
    {
    }



    private void HandleTargetFound(object sender, EventArgs e)
    {
        Target.Value = targetDetector.currentTarget;
    }
}

