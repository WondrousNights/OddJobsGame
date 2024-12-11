using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Local_PlayerLook : MonoBehaviour
{
    [SerializeField] Camera cam;

    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    Local_PlayerInputController inputController;

    private void Start()
    {
        inputController = GetComponent<Local_PlayerInputController>();
    }
    public void ProcessLook(Vector2 input)
    {
        // Scale input by screen resolution to make sensitivity consistent
        float mouseX = input.x / Screen.width;
        float mouseY = input.y / Screen.height;

        // Apply sensitivity scaling
        xRotation -= (mouseY * ySensitivity);
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * (mouseX * xSensitivity));
    }
}
