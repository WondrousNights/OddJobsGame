using System.Collections;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Local_PlayerHealthManager : MonoBehaviour
{
    
    public bool isRagdoll;

    RagdollEnabler ragdollEnabler;
    Local_PlayerInputController inputController;
    CharacterController characterController;
    BoxCollider triggerCollider;

   


    void Start()
    {
        ragdollEnabler = GetComponentInChildren<RagdollEnabler>();
        inputController = GetComponent<Local_PlayerInputController>();
        characterController = GetComponent<CharacterController>();
        triggerCollider = GetComponent<BoxCollider>();
    }

  
    public void TakeDamage(Vector3 force, Vector3 hitPoint)
    {
        Debug.Log("Ragdoll!!");
        StartCoroutine("ProcessRagdollAnimation", 3f);


       Rigidbody hitRigidbody = FindHitRigidbody(hitPoint);

       hitRigidbody.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);

       
    }



    IEnumerator ProcessRagdollAnimation(float duration)
    {
       isRagdoll = true;
       ragdollEnabler.EnableRagdoll();

       inputController.mycam.cullingMask = inputController.nohudLayerMask;
       inputController.mycam.transform.position = inputController.thirdPersonCamPos.transform.position;
       Debug.Log("Ragdoll enabled");
       triggerCollider.enabled = false;
       
       yield return new WaitForSeconds(duration);
        
        ragdollEnabler.EnableAnimator();
       inputController.mycam.transform.position = inputController.firstPersonCamPos.transform.position;
     
       isRagdoll = false;
       inputController.ResetSetCameraLayerMask();
       triggerCollider.enabled = true;
       

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
