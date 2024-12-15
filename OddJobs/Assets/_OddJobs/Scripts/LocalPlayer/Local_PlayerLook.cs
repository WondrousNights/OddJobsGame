using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Local_PlayerLook : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] private float gamepadMultiplier = 100f;
    public float xSensitivity = 40f;
    public float ySensitivity = 40f;
    private float xRotation = 0f;

    Local_PlayerInputController inputController;

    private void Start()
    {
        inputController = GetComponent<Local_PlayerInputController>();
    }
    public void ProcessLook(Vector2 input = default)
    {
        // Scale input by screen resolution to make sensitivity consistent across resolutions
        float mouseX = input.x / Screen.width;
        float mouseY = input.y / Screen.height;

        // Make controller sensitivity similar to mouse sensitivity
        if (playerInput.currentControlScheme == "Controller" || playerInput.currentControlScheme == "Gamepad") {
            mouseX *= gamepadMultiplier;
            mouseY *= gamepadMultiplier;
        }

        // Apply sensitivity
        xRotation -= mouseY * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * (mouseX * xSensitivity));
    }
}
