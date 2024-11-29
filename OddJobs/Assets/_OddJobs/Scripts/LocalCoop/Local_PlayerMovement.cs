using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Local_PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed;
    public float gravity = -9.8f;
    public float jumpHeight = 3;

    [SerializeField] Local_AnimationController localAnimationController;

    Local_PlayerInputController inputController;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputController = GetComponent<Local_PlayerInputController>();
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
    public void ProcessAnimations(Vector2 input)
    {
        localAnimationController.ProcessVisuals(input);
    }
    
    public void Jump()
    {

        if (controller.isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3 * gravity);
        }
    }


   



}
