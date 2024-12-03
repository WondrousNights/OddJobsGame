using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class NetworkEnemyMovement : NetworkBehaviour
{

    private EnemyReferences enemyReferences;
    
    float pathUpdateDeadline;
    float targetCheckDeadline;
    float shootingDistance;

    Transform myCurrentTarget;
    [SerializeField] GameObject initialTarget;

    public NetworkVariable<bool> inRange = new NetworkVariable<bool>();

    public bool fleeing = false;
    private void Awake()
    {
        enemyReferences = GetComponent<EnemyReferences>();
    }
    // Start is called before the first frame update
    void Start()
    {
        shootingDistance = enemyReferences.navMeshAgent.stoppingDistance;

        //initialTarget = GameObject.FindGameObjectWithTag("HomeBase");
        //UpdatePathServerRpc(initialTarget.transform.position);

    }

    private void FixedUpdate()
    {
        if (!IsHost || !IsServer) return;
        if (enemyReferences.targetSelector.GetCurrentTarget() != null && !fleeing)
        {

            myCurrentTarget = enemyReferences.targetSelector.GetCurrentTarget();

            inRange.Value = Vector3.Distance(transform.position, enemyReferences.targetSelector.GetCurrentTarget().position) <= shootingDistance;

            if (inRange.Value)
            {
                LookAtTarget(myCurrentTarget);
            }
            else {
                UpdatePathServerRpc(myCurrentTarget.transform.position);
            }

            SetAnimatorBoolVariableServerRpc("shoot", inRange.Value);
        }
        else if(!fleeing && enemyReferences.targetSelector.GetCurrentTarget() == null)
        {
            EnemyCheck();
        }

       
        SetAnimatorFloatVariableServerRpc("speed", enemyReferences.navMeshAgent.desiredVelocity.sqrMagnitude);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePathServerRpc(Vector3 position)
    {
        UpdatePathClientRpc(position);
    }

    [ClientRpc]
    private void UpdatePathClientRpc(Vector3 position)
    {
        
            if (Time.time >= pathUpdateDeadline)
            {
                pathUpdateDeadline = Time.time + enemyReferences.pathUpdateDelay;
                enemyReferences.navMeshAgent.SetDestination(position);
            }

    }


    private void EnemyCheck()
    {
        if (Time.time >= targetCheckDeadline)
        {
            targetCheckDeadline = Time.time + enemyReferences.targetCheckDelay;
            enemyReferences.targetSelector.CheckForTarget();
        }
    }
    private void LookAtTarget(Transform target)
    {
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }

    [ServerRpc(RequireOwnership = false)]
    public void FleeServerRpc()
    {
        fleeing = true;
        Debug.Log("I'm fleeing!!");
        
        UpdatePathServerRpc(RandomNavmeshLocation(25f));

        Invoke("StopFleeing", 2f);
    }

    void StopFleeing()
    {
        
        fleeing = false;
        StopFleeingServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void StopFleeingServerRpc()
    {
        Debug.Log("I'm not fleeing anymore you bastard!!");
     
        fleeing = false;
        StopFleeingClientRpc();
    }
    [ClientRpc]
    void StopFleeingClientRpc()
    {

        fleeing = false;
    }

    //Animation

    [ServerRpc(RequireOwnership = false)]
    void SetAnimatorFloatVariableServerRpc(string name, float value)
    {
        SetAnimatorFloatVariableClientRpc(name, value);
    }

    [ClientRpc]
    void SetAnimatorFloatVariableClientRpc(string name, float value)
    {
        enemyReferences.animator.SetFloat(name, value);
    }

    [ServerRpc(RequireOwnership = false)]
    void SetAnimatorBoolVariableServerRpc(string name, bool value)
    {
        SetAnimatorBoolVariableClientRpc(name, value);
    }

    [ClientRpc]
    void SetAnimatorBoolVariableClientRpc(string name, bool value)
    {
        enemyReferences.animator.SetBool(name, value);
    }


    //Utility

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
