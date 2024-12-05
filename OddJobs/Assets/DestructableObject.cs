using UnityEngine;
using System.Linq;

public class DestructableObject : MonoBehaviour
{
    [SerializeField] private GameObject brokenObjectPrefab;
    [SerializeField] private float breakForce = 2;
    [SerializeField] private float health = 1f;
    [SerializeField] private bool debug = false;

    private Rigidbody rb;
    private bool broken = false;
    private GameObject brokenObject;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void BreakObject()
    {
        if (brokenObjectPrefab != null && !broken)
        {
            broken = true; // this is hacky but must be done to prevent double breaking
            brokenObject = Instantiate(brokenObjectPrefab, transform.position, transform.rotation);
            foreach (var rb in brokenObject.GetComponentsInChildren<Rigidbody>())
            {
                rb.AddExplosionForce(breakForce, transform.position, 10f);
            }
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.relativeVelocity.magnitude >= breakForce || health <= 0)
        {
            if (debug) Debug.Log("Breaking object from a force of " + other.relativeVelocity.magnitude);
            BreakObject();
        }
    }
}
