using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using RootMotion.Dynamics;
using Unity.Cinemachine;
public class Network_PlayerInputController : NetworkBehaviour
{
    private PlayerInputActions playerInput;
    private PlayerInputActions.OnFootActions onFoot;

    private Network_PlayerMovement playerMovement;
    private Network_PlayerLook look;
    private Network_PlayerGunHandler gunHandler;
    private Network_PlayerStats playerStats;

    NetworkAnimationController networkAnimationController;

    [SerializeField] Camera mycam;
    [SerializeField] CinemachineCamera cinemachineCamera;
    //[SerializeField] Cinemachine

    //[SerializeField] GameObject myVisuals;
    //[SerializeField] GameObject myCanvas;

    AudioListener myListener;
    public bool hasSpawned = false;

    [SerializeField] BehaviourPuppet puppet;

    // float count = 0;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
        onFoot = playerInput.OnFoot;
        onFoot.Enable();

        playerMovement = GetComponent<Network_PlayerMovement>();
        look = GetComponent<Network_PlayerLook>();
        gunHandler = GetComponent<Network_PlayerGunHandler>();
        playerStats = GetComponent<Network_PlayerStats>();
        networkAnimationController = GetComponent<NetworkAnimationController>();


        myListener = GetComponent<AudioListener>();
        //Event Subscription

       // playerStats.OnPlayerDeath += HandlePlayerDeathEvent;
        //playerStats.OnPlayerRevive += HandlePlayerReviveEvent;
        //Actions subscriptions

        onFoot.Jump.performed += ctx => HandleJump();
        //onFoot.Shoot.performed += ctx => gunHandler.Shoot();
        //onFoot.Reload.performed += ctx => gunHandler.Reload();
    }



    private void Start()
    {
        //NetworkManager.Singleton.SceneManager.OnSceneEvent += SetSpawn;
        Cursor.lockState = CursorLockMode.Locked;
        //Application.targetFrameRate = 60;
        //Camera Controls
        if (IsHost)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                mainCam.gameObject.SetActive(false);
            }
        }
        if (IsOwner)
        {

            mycam.gameObject.SetActive(true);
            //myCanvas.gameObject.SetActive(true);
            //myVisuals.SetActive(false);
        }
        else
        {
            mycam.gameObject.SetActive(false);
            myListener.enabled = false;
        }





    }

    public override void OnNetworkSpawn()
    {
       if(IsOwner)
       {
        cinemachineCamera.Priority = 1;
       }
       else
       {
        cinemachineCamera.Priority = 0;
        GetComponent<AudioListener>().enabled = false;
       }


    }
    private void Update()
    {
        if(!IsOwner || puppet.state == BehaviourPuppet.State.Unpinned) return;
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
       if(!IsOwner || puppet.state == BehaviourPuppet.State.Unpinned) return;
        playerMovement.ProcessMove(onFoot.Move.ReadValue<Vector2>());
          
        networkAnimationController.ProcessVisualsRpc(onFoot.Move.ReadValue<Vector2>());
        
    }


    void HandleJump()
    {
        if(IsOwner)
        {
            playerMovement.Jump();
            
        }
        networkAnimationController.ProcessJump();
    }

    private void HandlePlayerDeathEvent(object sender, EventArgs e)
    {
        if (!IsOwner)
        {
            //myVisuals.SetActive(false);
        }
        else {
            mycam.gameObject.SetActive(false);
            this.gameObject.transform.position = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position;
        }

    }

    private void HandlePlayerReviveEvent(object sender, EventArgs e)
    {
        if (!IsOwner)
        {
            //myVisuals.SetActive(true);
        }
        else
        {
            mycam.gameObject.SetActive(true);
            //gunHandler.currentGun.isReloading = false;
        }

    }
  


    /*
    void SetSpawn(SceneEvent sceneEvent)
    {

        if (sceneEvent.SceneEventType == SceneEventType.LoadEventCompleted)
        {
            Debug.Log("Spawn at pos");

            SetSpawnServerRpc();

        }

        if (sceneEvent.SceneEventType == SceneEventType.Unload)
        {
            SetHasSpawnedFalseServerRpc();
        }

    }
    [ServerRpc]
    void SetSpawnServerRpc()
    {
        SetSpawnClientRpc();
    }

    [ClientRpc]
    void SetSpawnClientRpc()
    {
        Transform spawnPos = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        this.gameObject.transform.position = spawnPos.position;
        Debug.Log(spawnPos.position);
        Invoke("SetSpawnTrue", 1f);
    }

    [ServerRpc]
    void SetHasSpawnedFalseServerRpc()
    {
        hasSpawned = false;
        SetHasSpawnedFalseClientRpc();
    }

    [ClientRpc]
    void SetHasSpawnedFalseClientRpc()
    {
        hasSpawned = false;
    }

    void SetSpawnTrue()
    {
        hasSpawned = true;
    }
    */

}
