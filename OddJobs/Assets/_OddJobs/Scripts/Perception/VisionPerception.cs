using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisionPerception : MonoBehaviour, IPerception
{

    public event Action<GameObject> OnTargetSpotted;
    public event Action<GameObject> OnTargetLost;
    public event Action<GameObject> OnClosestTargetChanged;

    [SerializeField] List<GameObject> previousTargetsICanSee = new List<GameObject>();


    [SerializeField] List<GameObject> targetsInRange = new List<GameObject>();
    [SerializeField] List<GameObject> targetsICanSee = new List<GameObject>();
    [SerializeField] GameObject closestTarget;

    [SerializeField] float distanceToSpot;
    [SerializeField] LayerMask perceptionMask;
    [SerializeField] float fieldOfView = 85;
    [SerializeField] Transform lookPos;

    [SerializeField] private float perceptionUpdateInterval = 0.2f;
    private float perceptionTimer = 0f;

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
            closestTarget = newClosest;
            OnClosestTargetChanged?.Invoke(closestTarget);
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
                OnTargetSpotted?.Invoke(obj);  
        }

        foreach (GameObject obj in previousTargetsICanSee)
        {
            if (!targetsICanSee.Contains(obj)) // Target lost
                OnTargetLost?.Invoke(obj); 
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
        Vector3 targetDirection = target.transform.position - transform.position;

        float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

        if (angleToPlayer <= fieldOfView * 0.5f)
        {
            Ray ray = new Ray(lookPos.position, targetDirection.normalized);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, distanceToSpot, perceptionMask))
            {
                if (hitInfo.collider.gameObject == target) // Ensure it's the target
                {
                    return true;
                }
            }
            Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
        }
        return false;
    }

}
