using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using RootMotion.Dynamics;
using Unity.Cinemachine;
using DialogueEditor;
public class Network_PlayerInputController : NetworkBehaviour
{
    public PlayerInputActions playerInput;
    public PlayerInputActions.OnFootActions onFoot;



    // float count = 0;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
        onFoot = playerInput.OnFoot;
        onFoot.Enable();

    
    }

    void OnEnable()
    {
        onFoot.Enable();
    }
    void OnDisable()
    {
        onFoot.Disable();
    }


    
  
    

}
