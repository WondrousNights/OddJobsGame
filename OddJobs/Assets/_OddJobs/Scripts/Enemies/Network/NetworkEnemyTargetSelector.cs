using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkEnemyTargetSelector : NetworkBehaviour
{
    private Transform target;

    public float distanceToSpot;

    public LayerMask playerMask;
    public void CheckForTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, distanceToSpot, playerMask);

        if (colliders.Length > 0)
        {
            Collider closesetCollider = GetClosestPlayer(transform.position, colliders);

            if (closesetCollider != null)
            {
                target = closesetCollider.gameObject.transform;
            }
        }
    }

    public Transform GetCurrentTarget()
    {
        return target;
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
        Debug.Log(closestCollider.gameObject.name);
        return closestCollider;
    }

}
