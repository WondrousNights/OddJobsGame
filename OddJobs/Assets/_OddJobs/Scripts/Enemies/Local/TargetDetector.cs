using UnityEngine;

public class TargetDetector : MonoBehaviour {
    public GameObject currentTarget;

    [SerializeField] float distanceToSpot;

    [SerializeField] LayerMask playerMask;


    void Update()
    {
        CheckForTarget();


        if(currentTarget != null)
        {
            if(Vector3.Distance(this.transform.position, currentTarget.transform.position) < distanceToSpot)
            {
                currentTarget = null;
            }
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
}