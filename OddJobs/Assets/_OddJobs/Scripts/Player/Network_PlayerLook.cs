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

  
    public void ProcessLook(Vector2 input)
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
