using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed;
    public float gravity = -9.8f;
    public float jumpHeight = 3;

    [SerializeField] NetworkAnimationController networkAnimationController;

    PlayerInputController inputController;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputController = GetComponent<PlayerInputController>();
        //networkAnimationController = GetComponentInChildren<NetworkAnimationController>();
    }

    public void ProcessMove(Vector2 input)
    {
        if (!inputController.hasSpawned) return;

        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }        
        controller.Move(playerVelocity * Time.deltaTime);

    }
    public void ProcessAnimations(Vector2 input)
    {
        networkAnimationController.ProcessVisuals(input);
    }
    
    public void Jump()
    {
        if (!IsOwner) return;

        if (controller.isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3 * gravity);
        }
    }


   



}
