using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BestPositionToMove", story: "[Agent] calculates the best [positiontomove] considering my [target] and my [validpositions]", category: "Action", id: "f88d1966133509c187e45930bf6fa1d4")]
public partial class BestPositionToMoveAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Vector3> Positiontomove;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<List<Vector3>> Validpositions;
    Enemy_Manager enemy_Manager;
    protected override Status OnStart()
    {
        enemy_Manager = Agent.Value.GetComponent<Enemy_Manager>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Positiontomove.Value = PositionEvaluator.ChooseBestPosition(Validpositions.Value, Target.Value.transform.position, enemy_Manager.enemyPersonality);

        if(Positiontomove.Value != null)
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

