using UnityEngine;
using System.Linq;
using System.Collections;

public class DestructableObject : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject brokenObjectPrefab;
    [SerializeField] private float breakForce = 2;
    [SerializeField] private bool debug = false;
    [SerializeField] private float _maxHealth = 1;

    private float _health;
    private Rigidbody rb;
    private bool broken = false;
    private GameObject brokenObject;

    public float CurrentHealth {get => _health; private set => _health = value;}
    public float MaxHealth {get => _maxHealth; private set => _maxHealth = value;}

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _health = _maxHealth;
    }

    public void TakeDamageFromGun(Ray ray, float damage, float hitForce, Vector3 collisionPoint)
    {
        _health -= damage;
        if (debug) Debug.Log(name + " took " + damage + " damage, current health: " + _health);

        // rb.AddForceAtPosition(ray.direction * hitForce, collisionPoint, ForceMode.Impulse);
    }

    

    private void Update()
    {
        if (CurrentHealth <= 0 && !broken)
        {
            StartCoroutine(BreakObject());
            if (debug) Debug.Log("Breaking object " + name + " due to health depletion");
        }
    }

    // break object upon forceful collision with another body
    private void OnCollisionEnter(Collision other)
    {
        if (other.relativeVelocity.magnitude >= breakForce)
        {
            StartCoroutine(BreakObject());
            if (debug) Debug.Log("Breaking object from a force of " + other.relativeVelocity.magnitude);
        }
    }

    private IEnumerator BreakObject()
    {
        if (!broken)
        {
            broken = true; // this is hacky but must be done to prevent double breaking
            yield return null; // wait for one frame to make sure the physics update has finished

            if (!brokenObjectPrefab) // throw error if broken object prefab is not set
            {
                Debug.LogError("Attempted to break destructable object, but broken object prefab is not set for " + name);
            }
            else
            {
                // instantiate broken object
                brokenObject = Instantiate(brokenObjectPrefab, transform.position, transform.rotation);
                
                foreach (var brokenRb in brokenObject.GetComponentsInChildren<Rigidbody>())
                {
                    brokenRb.linearVelocity = rb.linearVelocity; // make all rigidbodies in the broken object inheret the velocity of the parent
                    brokenRb.AddForce(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * 0.1f, ForceMode.Impulse); // give each rigidbody a slight force in a random direction
                }
            }
        }
        Destroy(gameObject);

        yield return null;
    }


    public void TakeDamageFromMelee(Vector3 positionOfAttacker, float damage, float hitForce, Vector3 collsionPoint)
    {
        throw new System.NotImplementedException();
    }
}
