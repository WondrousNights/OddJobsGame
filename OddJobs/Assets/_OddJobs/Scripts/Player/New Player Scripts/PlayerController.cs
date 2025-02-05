using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{

    private Network_PlayerInputController inputController;
    private PlayerManager playerManager;


    [Header("Movement Settings")]
    public CharacterController controller;
    
    private Vector3 playerVelocity;
    public float speed;
    public float gravity = -9.8f;
    public float jumpHeight = 3;


    [Header("Look Settings")]
    public Camera cam;

    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;


    PlayerInputActions.OnFootActions currentActions;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputController = GetComponent<Network_PlayerInputController>();
        playerManager = GetComponent<PlayerManager>();

        currentActions = inputController.onFoot;
  
    }

    void Start()
    {
        inputController.onFoot.Jump.performed += ctx => Jump();
    }


    void Update()
    {
        if(!IsOwner) return;
        if(playerManager.currentPlayerState == PlayerManager.PlayerState.Dead) return;
        if(playerManager.currentPlayerState == PlayerManager.PlayerState.InMenu) return;

            
        ProcessMove(currentActions.Move.ReadValue<Vector2>());
        ProcessLook(currentActions.Look.ReadValue<Vector2>());

        playerManager.networkAnimationController.ProcessVisualsRpc(currentActions.Move.ReadValue<Vector2>());
    }

    void ProcessMove(Vector2 input)
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
    
    void Jump()
    {

        if (controller.isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3 * gravity);
        }

        playerManager.networkAnimationController.ProcessJump();
    }

    void ProcessLook(Vector2 input)
    {
        //if (!IsOwner) return;
        // Scale input by screen resolution to make sensitivity consistent across resolutions
        float mouseX = input.x / Screen.width;
        float mouseY = input.y / Screen.height;


    

        // Apply sensitivity
        xRotation -= mouseY * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * (mouseX * xSensitivity));
    
    }
}
