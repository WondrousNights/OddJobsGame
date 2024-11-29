using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkAnimationController : NetworkBehaviour
{
    [SerializeField] Animator animator;

    Vector3 movement;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void ProcessVisuals(Vector2 moveInput)
    {
        ProcessVisualsServerRpc(moveInput);
        


        /*
        float velocityZ = Vector3.Dot(movement.normalized, forward.gameObject.transform.forward);
        float velocityX = Vector3.Dot(movement.normalized, forward.gameObject.transform.right);

        Debug.Log("Velocity Z :" + velocityZ + "Velocity X : " + velocityX);

        animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);
        */
    }

    [ServerRpc(RequireOwnership = false)]
    void ProcessVisualsServerRpc(Vector2 moveInput)
    {
        ProcessVisualsClientRpc(moveInput);
    }

    [ClientRpc]
    void ProcessVisualsClientRpc(Vector2 moveInput)
    {
        movement = new Vector3(moveInput.x, 0f, moveInput.y);

        animator.SetFloat("speed", movement.magnitude);
    }
    
}
