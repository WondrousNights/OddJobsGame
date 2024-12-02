using System;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{

    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float multiplier;

    public void WeaponSwayAnimation(Vector2 lookInput)
    {
        Debug.Log("I am swaying" + lookInput.x + lookInput.y);
        float mouseX = lookInput.x * multiplier;
        float mouseY = lookInput.y * multiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        // rotate 
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}