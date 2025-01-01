using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerCharacterController : MonoBehaviour
{

    [SerializeField] Camera cam;
    [SerializeField] GameObject ragdoll;
    [SerializeField] PlayerAnimatorController playerAnimatorController;
    [SerializeField] ActiveRagdollController activeRagdollController;

    Vector2 lookInput;

    float xRotation;
    float yRotation;

    [SerializeField] float ySensitivity;
    [SerializeField] float xSensitivity;

    void Update()
    {
        //ProcessLook(lookInput);
    }

    public void ProcessForward(CallbackContext context)
    {
        if(context.performed)
        {
        playerAnimatorController.SetWalkForword();
        activeRagdollController.forward = true;
        activeRagdollController.backward = false;
        activeRagdollController.right = false;
        activeRagdollController.left = false;
        }
        if(context.canceled)
        {
            Reset();
        }
        
    }

    public void ProcessBackward(CallbackContext context)
    {
        playerAnimatorController.SetWalkBackward();
        activeRagdollController.forward = false;
        activeRagdollController.backward = true;
        activeRagdollController.right = false;
        activeRagdollController.left = false;
        if(context.canceled)
        {
            Reset();
        }
    }

    public void ProcessRight(CallbackContext context)
    {
        playerAnimatorController.SetWalkRight();
        activeRagdollController.forward = false;
        activeRagdollController.backward = false;
        activeRagdollController.right = true;
        activeRagdollController.left = false;
        if(context.canceled)
        {
            Reset();
        }
    }

    public void ProcessLeft(CallbackContext context)
    {
        playerAnimatorController.SetWalkLeft();
        activeRagdollController.forward = false;
        activeRagdollController.backward = false;
        activeRagdollController.right = false;
        activeRagdollController.left = true;
        if(context.canceled)
        {
            Reset();
        }
    }




    public void ProcessLook(CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    
    public void ProcessLook(Vector2 input)
    {
        /*
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80, 80f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);

        */
    }



    void Reset()
    {
        playerAnimatorController.Reset();
        activeRagdollController.forward = false;
        activeRagdollController.backward = false;
        activeRagdollController.right = false;
        activeRagdollController.left = false;
    }
}
