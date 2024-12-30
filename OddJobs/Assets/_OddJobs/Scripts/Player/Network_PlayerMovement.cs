using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Network_PlayerMovement : NetworkBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed;
    public float gravity = -9.8f;
    public float jumpHeight = 3;

 

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        //networkAnimationController = GetComponentInChildren<NetworkAnimationController>();
    }

    public void ProcessMove(Vector2 input)
    {

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
    
    public void Jump()
    {

        if (controller.isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3 * gravity);
        }
        
    }


   



}
