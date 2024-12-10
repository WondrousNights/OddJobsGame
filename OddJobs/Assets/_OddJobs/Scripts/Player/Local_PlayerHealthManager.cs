using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Local_PlayerHealthManager : MonoBehaviour, IDamageable
{
    
    public bool isRagdoll;

    RagdollEnabler ragdollEnabler;
    Local_PlayerInputController inputController;
    CharacterController characterController;
    BoxCollider triggerCollider;

   
    [SerializeField]
    private float _MaxHealth = 100;
    [SerializeField]
    private float _Health;
    public float CurrentHealth {get => _Health; private set => _Health = value;}

    public float MaxHealth {get => _MaxHealth; private set => _MaxHealth = value;}

    [SerializeField] float timeToGetUp;
    float count;

    bool isDead;

    void Start()
    {
        ragdollEnabler = GetComponentInChildren<RagdollEnabler>();
        inputController = GetComponent<Local_PlayerInputController>();
        characterController = GetComponent<CharacterController>();
        triggerCollider = GetComponent<BoxCollider>();
    }

    public void TakeDamageFromGun(Ray ray, float damage, float hitForce, Vector3 collisionPoint)
    {
        Rigidbody hitRigidbody = FindHitRigidbody(collisionPoint);
        hitRigidbody.AddForceAtPosition(ray.direction * hitForce, collisionPoint, ForceMode.Impulse);
    }


    public void TakeDamageFromMelee(Vector3 positionOfAttacker, float damage, float hitForce, Vector3 collsionPoint)
    {

        CurrentHealth -= damage;


       inputController.playerUI.UpdateHealthImage(CurrentHealth, MaxHealth);

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
        


        Vector3 forceDirection = this.transform.position - positionOfAttacker;
                forceDirection.y = 1;
                forceDirection.Normalize();

        Vector3 force = hitForce * forceDirection;


        Rigidbody hitRigidbody = FindHitRigidbody(collsionPoint);
        hitRigidbody.AddForceAtPosition(force, collsionPoint, ForceMode.Impulse);
    }

    private void HandleDeath()
    {
        isDead = true;
        if(!isRagdoll)
        {
            ProcessRagdollAnimation();
        }
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



    void ProcessRagdollAnimation()
    {
        isRagdoll = true;
       ragdollEnabler.EnableRagdoll();

       inputController.mycam.cullingMask = inputController.nohudLayerMask;
       inputController.mycam.transform.localPosition = inputController.thirdPersonCamPos.transform.localPosition;
       inputController.mycam.transform.localRotation = inputController.thirdPersonCamPos.transform.localRotation;

       Debug.Log("Ragdoll enabled");
       triggerCollider.enabled = false;
       characterController.enabled = false;
    }

     private void ProcessGetUp()
    {
        if(!isDead)
        {
            gameObject.transform.position = inputController.hipPosition.transform.position;
            

        
            inputController.mycam.transform.localPosition = inputController.firstPersonCamPos.transform.localPosition;
            inputController.mycam.transform.localRotation = inputController.firstPersonCamPos.transform.localRotation;
            ragdollEnabler.EnableAnimator();
            inputController.ResetSetCameraLayerMask();
            triggerCollider.enabled = true;
            isRagdoll = false;
            characterController.enabled = true;
        }
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
