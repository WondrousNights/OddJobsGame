using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetPerception : MonoBehaviour, IPerception
{

    [SerializeField] List<GameObject> previousTargetsICanSee = new List<GameObject>();


    [SerializeField] List<GameObject> targetsInRange = new List<GameObject>();
    [SerializeField] List<GameObject> targetsICanSee = new List<GameObject>();
    [SerializeField] GameObject closestTarget;
    [SerializeField] GameObject lastClosestTarget;

    [SerializeField] float distanceToSpot;
    [SerializeField] LayerMask perceptionMask;
    [SerializeField] float fieldOfView = 85;
    [SerializeField] Transform lookPos;

    [SerializeField] private float perceptionUpdateInterval = 0.2f;
    private float perceptionTimer = 0f;

    Enemy_PerceptionManager perceptionManager;

    public void UpdatePerception()
    {
        perceptionTimer += Time.deltaTime;
        if (perceptionTimer >= perceptionUpdateInterval)
        {
            CheckForTargetsInRange();
            CheckForTargetsICanSee();
            GameObject newClosest = GetClosestTarget(targetsICanSee);

            if (newClosest != closestTarget)
            {
                perceptionManager.InvokePerceptionEvent("OnCurrentTargetLostVisual", closestTarget);
                closestTarget = newClosest;
                perceptionManager.InvokePerceptionEvent("OnClosestTargetChanged", closestTarget);
            }

            DetectTargetChanges();
            perceptionTimer = 0f; // Reset timer
        }
        
        
    }

    private void DetectTargetChanges()
    {
        foreach (GameObject obj in targetsICanSee)
        {
            if (!previousTargetsICanSee.Contains(obj)) // New target spotted
                perceptionManager.InvokePerceptionEvent("OnTargetSpotted", obj);
        }

        foreach (GameObject obj in previousTargetsICanSee)
        {
            if (!targetsICanSee.Contains(obj)) // Target lost
                perceptionManager.InvokePerceptionEvent("OnTargetLost", obj);
        }

        previousTargetsICanSee = new List<GameObject>(targetsICanSee);
    }

    private void ForgetTargets()
    {
        targetsInRange.RemoveAll(obj => Vector3.Distance(obj.transform.position, transform.position) >= distanceToSpot);
    }


    public void CheckForTargetsInRange()
    {
        targetsInRange.Clear();  // Prevent duplicates
        Collider[] visibleTargets = Physics.OverlapSphere(transform.position, distanceToSpot, perceptionMask);

        foreach (Collider col in visibleTargets)
        {
            targetsInRange.Add(col.gameObject);
        }
    }

    public void CheckForTargetsICanSee()
    {
        targetsICanSee.Clear(); // Prevents stale data
        foreach (GameObject obj in targetsInRange)
        {
            if (CanSeeTarget(obj))
            {
                targetsICanSee.Add(obj);
            }
        }
    }


    GameObject GetClosestTarget(List<GameObject> targets)
    {
    return targets.OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).FirstOrDefault();
    }


    bool CanSeeTarget(GameObject target)
    {
        Vector3 targetDirection = target.transform.position - lookPos.position;

        float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

        if (angleToPlayer <= fieldOfView * 0.5f)
        {
            Ray ray = new Ray(lookPos.position, targetDirection.normalized);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, distanceToSpot))
            {
                if (hitInfo.collider.gameObject == target) // Ensure it's the target
                {
                    return true;
                }
                else
                {
                    //Debug.Log($"[Vision] Target blocked by: {hitInfo.collider.gameObject.name}");
                    return false;
                }
            }
            Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
        }
        return false;
    }

    public void SetManager(Enemy_PerceptionManager perceptionManager)
    {
        this.perceptionManager = perceptionManager;
    }

    public void StartPerception()
    {
       
    }
}
