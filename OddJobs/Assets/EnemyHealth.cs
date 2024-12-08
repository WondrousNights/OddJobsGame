using System;
using System.Collections;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class EnemyHealth : MonoBehaviour, IDamageable
{

    [SerializeField]
    private float _MaxHealth = 100;
    [SerializeField]
    private float _Health;
    public float CurrentHealth {get => _Health; private set => _Health = value;}

    public float MaxHealth {get => _MaxHealth; private set => _MaxHealth = value;}


    private bool isRagdoll;
    bool isDead;

    RagdollEnabler ragdollEnabler;

    [SerializeField] GameObject ragdollRoot;
    [SerializeField] CapsuleCollider capsuleCollider;
    void Start()
    {
        ragdollEnabler = GetComponent<RagdollEnabler>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        CurrentHealth = MaxHealth;

    }

  

    public void TakeDamage(Vector3 positionOfAttacker, float Damage, float force, Vector3 collisionPoint)
    {
        

        CurrentHealth -= Damage;

        HandleDamageResponse(positionOfAttacker, force, collisionPoint);

        if(CurrentHealth <= 0)
        {
            HandleDeath();
        }
  
    }

    private void HandleDeath()
    {
        if(!isDead)
        {
        ragdollEnabler.EnableRagdoll();
        isDead = true;
        }
    }

    void HandleDamageResponse(Vector3 positionOfAttacker, float force, Vector3 collisionPoint)
    {

        if(!isDead)
        {
            StartCoroutine("ProcessRagdollAnimation", 3f);
        }
        
        Vector3 forceDirection = this.transform.position - positionOfAttacker;
                forceDirection.y = 1;
                forceDirection.Normalize();
        Vector3 forceAdjusment = force * forceDirection;


       Rigidbody hitRigidbody = FindHitRigidbody(collisionPoint);

       hitRigidbody.AddForceAtPosition(forceAdjusment, collisionPoint, ForceMode.Impulse);
    }



    IEnumerator ProcessRagdollAnimation(float duration)
    {
        isRagdoll = true;
       ragdollEnabler.EnableRagdoll();
       capsuleCollider.enabled = false;
       yield return new WaitForSeconds(duration);


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

    
}
