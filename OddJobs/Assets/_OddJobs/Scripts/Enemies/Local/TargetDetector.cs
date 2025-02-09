using System;
using UnityEngine;

public class TargetDetector : MonoBehaviour {
    public GameObject currentTarget;

    [SerializeField] float distanceToSpot;

    [SerializeField] LayerMask playerMask;

    [SerializeField] float fieldOfView = 85;

    public event EventHandler OnFoundTarget;

    [SerializeField] Transform eyePos;
    [SerializeField] LayerMask playerLayerMask;

    void Update()
    {
        CheckForTarget();
        CanSeeTarget();

        if(currentTarget != null)
        {

            /*
            if(Vector3.Distance(this.transform.position, currentTarget.transform.position) < distanceToSpot)
            {
                currentTarget = null;
            }
            */
        }
        
    }
    
    public void CheckForTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, distanceToSpot, playerMask);

        if (colliders.Length > 0)
        {
            Collider closesetCollider = GetClosestPlayer(transform.position, colliders);

            if (closesetCollider != null)
            {
                currentTarget = closesetCollider.gameObject;
                OnFoundTarget?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    Collider GetClosestPlayer(Vector3 myPos, Collider[] colliders)
    {
        float closestDistance = 999999f;
        Collider closestCollider = null;

        foreach (Collider obj in colliders)
        {
            float distance = Vector3.Distance(myPos, obj.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCollider = obj;
            }
        }
        // Debug.Log(closestCollider.gameObject.name);
        return closestCollider;
    }

    public bool CanSeeTarget()
    {
        if(currentTarget != null)
        {
            Vector3 targetDirection = currentTarget.transform.position - transform.position;
            float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
            if(angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
            {
                Ray ray = new Ray(eyePos.position, targetDirection);
                RaycastHit hitInfo;
                if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, playerLayerMask))
                {
                    return true;
                }
                Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
            }
        }
        return false;
    }
}