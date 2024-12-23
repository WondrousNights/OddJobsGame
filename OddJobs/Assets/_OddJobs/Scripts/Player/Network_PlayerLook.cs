using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Network_PlayerLook : NetworkBehaviour
{
    [SerializeField] Camera cam;

    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    Network_PlayerInputController inputController;

    private void Start()
    {
        inputController = GetComponent<Network_PlayerInputController>();
    }
    public void ProcessLook(Vector2 input)
    {
        if (!IsOwner) return;
        if (!inputController.hasSpawned) return;

        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80, 80f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}
