using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Find Random Point and Move To It", story: "[Agent] finds random waypoint within a [range] and navigates to it", category: "Action", id: "66ee31fbb6e366d1b6acb2876c29fc9e")]
public partial class FindRandomPointAndMoveToItAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<float> Range;
    [SerializeReference] public BlackboardVariable<float> Speed;

    [SerializeReference] public BlackboardVariable<string> AnimatorSpeedParam = new BlackboardVariable<string>("SpeedMagnitude");

    NavMeshAgent navMeshAgent;

    Vector3 newTarget;
    Vector3 startPos;

    Animator m_Animator;

    bool runningToTarget = false;

    protected override Status OnStart()
    {
        navMeshAgent = Agent.Value.GetComponent<NavMeshAgent>(); 

        startPos = Agent.Value.transform.position;
        newTarget = RandomNavmeshLocation(Range.Value);
        navMeshAgent.speed = Speed;
        m_Animator = Agent.Value.GetComponent<Animator>();
      
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(!runningToTarget)
        {
            navMeshAgent.SetDestination(newTarget);
            runningToTarget = true;
        }

        if(runningToTarget)
        {
            if(Vector3.Distance(Agent.Value.transform.position, newTarget) <= 2f)
            {
            newTarget = RandomNavmeshLocation(Range.Value);
            runningToTarget = false;
            }
            m_Animator.SetFloat(AnimatorSpeedParam, navMeshAgent.velocity.magnitude);
        }
        
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }

    public Vector3 RandomNavmeshLocation(float radius) {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += startPos;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
            finalPosition = hit.position;            
        }
        return finalPosition;
    }

}

