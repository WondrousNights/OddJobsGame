using Unity.AppUI.UI;
using UnityEngine;

public class HeldItemInteraction : Interactable
{

    Rigidbody rb;
    Transform grabPoint;

    bool isGrabbed = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    protected override void Interact(Local_PlayerInteractionManager playerInteracting)
    {
       if(isGrabbed)
       {
        playerInteracting.GetComponent<PlayerGunHandler>().isHoldingObject = false;
        Drop();
       }
       else
       {
        playerInteracting.GetComponent<PlayerGunHandler>().isHoldingObject = true;
        Grab(playerInteracting.grabPoint);
       }
       
    }

    void Grab(Transform grapPosition)
    {
        grabPoint = grapPosition;
        rb.useGravity = false;
        isGrabbed = true;
        promptMessage = "Drop cake [Be Careful]";
    }

    void Drop()
    {
        grabPoint = null;
        rb.linearVelocity = Vector3.zero;
        rb.useGravity = true;
        isGrabbed = false;
        promptMessage = "Pick up Cake [Be Careful]";
    }

    void FixedUpdate()
    {
        if(grabPoint != null)
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, grabPoint.position,Time.deltaTime * lerpSpeed);
            rb.MovePosition(newPosition);
        }
    }
}
