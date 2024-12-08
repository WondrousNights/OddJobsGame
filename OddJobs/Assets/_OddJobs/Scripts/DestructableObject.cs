using UnityEngine;
using System.Linq;

public class DestructableObject : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject brokenObjectPrefab;
    [SerializeField] private float breakForce = 2;
    [SerializeField] private bool debug = false;
    [SerializeField] private float _maxHealth = 1;

    private Rigidbody rb;
    private bool broken = false;
    private GameObject brokenObject;


    
    [SerializeField]
    private float _health;
    public float CurrentHealth {get => _health; private set => _health = value;}

    public float MaxHealth {get => _maxHealth; private set => _maxHealth = value;}


    public void TakeDamage(Ray ray, Vector3 positionOfAttacker, float Damage, float hitForce, Vector3 collisionPoint)
    {
        rb.AddForceAtPosition(ray.direction * hitForce, collisionPoint, ForceMode.Impulse);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _health = _maxHealth;
    }

    private void BreakObject()
    {
        if (!broken)
        {
            broken = true; // this is hacky but must be done to prevent double breaking
            if (!brokenObjectPrefab) // throw error if broken object prefab is not set
            {
                Debug.LogError("Attempted to break destructable object, but broken object prefab is not set for " + name);
            }
            else
            {
                // instantiate broken object
                brokenObject = Instantiate(brokenObjectPrefab, transform.position, transform.rotation);
                // make all rigidbodies in the broken object inheret the velocity of the parent
                foreach (var brokenRb in brokenObject.GetComponentsInChildren<Rigidbody>())
                {
                    brokenRb.linearVelocity = rb.linearVelocity;
                }
            }
        }
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (CurrentHealth <= 0)
        {
            if (debug) Debug.Log("Breaking object " + name + " due to health depletion");
            BreakObject();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.relativeVelocity.magnitude >= breakForce)
        {
            if (debug) Debug.Log("Breaking object from a force of " + other.relativeVelocity.magnitude);
            BreakObject();
        }
    }
}
