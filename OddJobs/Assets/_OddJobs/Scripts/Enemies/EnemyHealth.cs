using System;
using System.Collections;
using System.Numerics;
using Unity.Behavior;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Vector3 = UnityEngine.Vector3;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField]
    private float _MaxHealth = 10;
    [SerializeField]
    private float _Health = 10;
    public float CurrentHealth {get => _Health; private set => _Health = value;}

    public float MaxHealth {get => _MaxHealth; private set => _MaxHealth = value;}

    [SerializeField] float timeToGetUp;

    float count;
    private bool isRagdoll;
    public bool isDead;

    RagdollEnabler ragdollEnabler;

    [SerializeField] GameObject ragdollRoot;
    [SerializeField] CapsuleCollider capsuleCollider;

    TargetDetector targetDetector;

    //Events
    public event EventHandler OnDeath;
    public event EventHandler<OnDamagedEventArgs> OnDamaged;
    public class OnDamagedEventArgs : EventArgs
    {
        public GameObject Sender;
    }


    void Start()
    {
        ragdollEnabler = GetComponent<RagdollEnabler>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        targetDetector = GetComponent<TargetDetector>();

    }

    private void OnEnable()
    {
        count = 0;
    }

    void Update()
    {
        if(isRagdoll && !isDead)
        {
            count += Time.deltaTime;

            if(count >= timeToGetUp)
            {
                ProcessGetUp();
            }
        }
    }

   

    public void TakeDamage()
    {
        /*
        timeToGetUp = recoveryTime;
        if(CurrentHealth == MaxHealth)
        {
            if(!sender.GetComponent<EnemyManager>())
            {
                OnDamaged.Invoke(this, new OnDamagedEventArgs { Sender = sender});
                targetDetector.currentTarget = sender;
            }
            
            
        }
        CurrentHealth -= Damage;

        HandleDamageResponseGun(ray, force, collisionPoint);


        if(CurrentHealth <= 0)
        {
            HandleDeath();
        }
    */
    

  
    }

    private void HandleDeath()
    {
        if(!isDead)
        {
            OnDeath.Invoke(this, EventArgs.Empty);

            ragdollEnabler.EnableRagdoll();
            isDead = true;
        }
    }

    void HandleDamageResponseGun(Ray ray, float force, Vector3 collisionPoint)
    {

        if(!isDead)
        {
            count = 0;

            if(!isRagdoll)
            {
                ProcessRagdollAnimation();
            }
            
        }
        
  
       Rigidbody hitRigidbody = FindHitRigidbody(collisionPoint);

       hitRigidbody.AddForceAtPosition(ray.direction * force, collisionPoint, ForceMode.Impulse);
    }

    void HandleDamageResponseMelee(Vector3 positionOfAttacker,float damage, float force, Vector3 collisionPoint)
    {

       


        Vector3 forceDirection = this.transform.position - positionOfAttacker;
                forceDirection.y = 1;
                forceDirection.Normalize();

        Vector3 forceAdjustment = force * forceDirection;


        Rigidbody hitRigidbody = FindHitRigidbody(positionOfAttacker);
        hitRigidbody.AddForceAtPosition(forceAdjustment, positionOfAttacker, ForceMode.Impulse);
    }



    void ProcessRagdollAnimation()
    {
        isRagdoll = true;
       ragdollEnabler.EnableRagdoll();
       capsuleCollider.enabled = false;
    }

     private void ProcessGetUp()
    {
        if(!isDead)
        {
        transform.position = ragdollRoot.transform.position;
        capsuleCollider.enabled = true;
        ragdollEnabler.EnableAnimator();
        isRagdoll = false;
        }
    }
    

    public bool GetIsRagdoll()
    {
        return isRagdoll;
    }


    private Rigidbody FindHitRigidbody(Vector3 hitPoint)
    {
        Rigidbody closestRigidbody = null;
        float closestDistance = 0;

        foreach (var rigidbody in ragdollEnabler.Rigidbodies)
        {
            float distance = Vector3.Distance(rigidbody.position, hitPoint);

            if (closestRigidbody == null || distance < closestDistance)
            {
                closestDistance = distance;
                closestRigidbody = rigidbody;
            }
        }

        return closestRigidbody;
    }

    

    public void TakeDamageFromMelee(Vector3 positionOfAttacker, float damage, float hitForce, Vector3 collsionPoint, float recoveryTime)
    {
       timeToGetUp = recoveryTime;
         CurrentHealth -= damage;

        if(CurrentHealth <= 0)
        {
            HandleDeath();
        }

        if(!isDead)
        {
            count = 0;

            if(!isRagdoll)
            {
                ProcessRagdollAnimation();
            }
            
        }
        

        HandleDamageResponseMelee(positionOfAttacker, damage, hitForce, collsionPoint);


    

    }
}
