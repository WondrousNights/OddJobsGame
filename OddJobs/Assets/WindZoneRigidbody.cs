using UnityEngine;

// this does not work
// colliders already in the collider do not get affected by wind

public class WindZoneRigidbody : MonoBehaviour
{
    [SerializeField] private float force = 1f;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.AddForce(transform.forward * force);
        }
    }


    private void Start()
    {
        if (TryGetComponent(out Collider c))
        {
            c.enabled = false;
            Invoke(nameof(EnableCollider), 1f);
        }
        if (TryGetComponent(out MeshCollider mc))
        {
            mc.enabled = false;
            Invoke(nameof(EnableCollider), 1f);
        }
    }

    private void EnableCollider()
    {
        if (TryGetComponent(out Collider c))
        {
            c.enabled = true;
        }
        if (TryGetComponent(out MeshCollider mc))
        {
            mc.enabled = true;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * force);
    }
}
