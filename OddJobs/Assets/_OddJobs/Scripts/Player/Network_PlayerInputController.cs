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
    private PlayerInputActions playerInput;
    private PlayerInputActions.OnFootActions onFoot;
    private CharacterController characterController;

    private Network_PlayerMovement playerMovement;
    private Network_PlayerLook look;
    private Network_PlayerGunHandler gunHandler;
    public Network_PlayerUI playerUI;
    private Network_PlayerInteractionManager interactionManager;
    private Network_HealthManager healthManager;

    NetworkAnimationController networkAnimationController;

    public Camera mycam;
    [SerializeField] CinemachineCamera cinemachineCamera;

    [SerializeField] GameObject[] bodyRenders;
   
    [SerializeField] GameObject myCanvas;

    AudioListener myListener;
    public bool hasSpawned = false;

    [SerializeField] PuppetMaster puppetMaster;
    [SerializeField] GameObject conversationManagerGO;
    ConversationManager conversationManager;
    Player_MenuHandler menuHandler;


    // float count = 0;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
        onFoot = playerInput.OnFoot;
        onFoot.Enable();

        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<Network_PlayerMovement>();
        look = GetComponent<Network_PlayerLook>();
        gunHandler = GetComponent<Network_PlayerGunHandler>();
        playerUI = GetComponent<Network_PlayerUI>();
        networkAnimationController = GetComponent<NetworkAnimationController>();
        interactionManager = GetComponent<Network_PlayerInteractionManager>();
        conversationManager = conversationManagerGO.GetComponent<ConversationManager>();
        healthManager = GetComponent<Network_HealthManager>();
        menuHandler = GetComponent<Player_MenuHandler>();


        myListener = GetComponent<AudioListener>();
        //Event Subscription

       // playerStats.OnPlayerDeath += HandlePlayerDeathEvent;
        //playerStats.OnPlayerRevive += HandlePlayerReviveEvent;
        //Actions subscriptions

        
        onFoot.Jump.performed += ctx => HandleJump();
        onFoot.Shoot.performed += ctx => gunHandler.ShootCurrentGun();
        onFoot.Reload.performed += ctx => gunHandler.Reload();
        onFoot.Interact.performed += ctx => interactionManager.ProcessInteract();
        onFoot.SwitchItemNext.performed += ctx => gunHandler.SwitchWeaponNext();
        onFoot.SwitchItemPrevious.performed += ctx => gunHandler.SwitchWeaponPrevious();
        onFoot.Menu.performed += ctx => menuHandler.OnMenuButtonClicked();
    }


    private void Start()
    {

        if(!IsOwner)
        {
            puppetMaster.mode = PuppetMaster.Mode.Disabled;
            Invoke("SetPuppetActive", 3f);
        }

        NetworkManager.Singleton.SceneManager.OnSceneEvent += SetSpawn;
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
            gameObject.layer = 15;

            foreach(GameObject go in bodyRenders)
            {
            go.layer = 15;
            }

        }
        else
        {
            mycam.gameObject.SetActive(false);
            myCanvas.gameObject.SetActive(false);
            characterController.enabled = false;
            myListener.enabled = false;
        }

    }
    

    
    private void Update()
    {
       
        if(!IsOwner || puppetMaster.state == PuppetMaster.State.Dead) return;
        if(conversationManager.IsConversationActive || !hasSpawned) return;
        if(menuHandler.MenuOpen) return;
        else { Cursor.lockState = CursorLockMode.Locked;}
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    {
        
       if(!IsOwner || puppetMaster.state == PuppetMaster.State.Dead) return;
        if(conversationManager.IsConversationActive || !hasSpawned) return;
        if(menuHandler.MenuOpen) return;
        playerMovement.ProcessMove(onFoot.Move.ReadValue<Vector2>());
          
        networkAnimationController.ProcessVisualsRpc(onFoot.Move.ReadValue<Vector2>());
        
    }


    void HandleJump()
    {
        if(!IsOwner || puppetMaster.state == PuppetMaster.State.Dead || !hasSpawned) return;
        playerMovement.Jump();
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
  


    
    void SetSpawn(SceneEvent sceneEvent)
    {

        if (sceneEvent.SceneEventType == SceneEventType.LoadEventCompleted)
        {
            Debug.Log("Spawn at pos");

            SetSpawnRpc();

        }

        if (sceneEvent.SceneEventType == SceneEventType.Unload)
        {
            SetHasSpawnedFalseRpc();
        }

    }
    
    [Rpc(SendTo.Everyone)]
    void SetSpawnRpc()
    {
        Transform spawnPos = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        this.gameObject.transform.position = spawnPos.position;
        Debug.Log(spawnPos.position);
        puppetMaster.mode = PuppetMaster.Mode.Disabled;
        Invoke("SetSpawnTrue", 2f);
    }

    [Rpc(SendTo.Everyone)]
    void SetHasSpawnedFalseRpc()
    {
        hasSpawned = false;
    }

    void SetSpawnTrue()
    {
        hasSpawned = true;
        puppetMaster.mode = PuppetMaster.Mode.Active;
        healthManager.Respawn();

        if(!IsOwner)
        {
            puppetMaster.mode = PuppetMaster.Mode.Disabled;
            Invoke("SetPuppetActive", 3f);
        }
    }

    void SetPuppetActive()
    {
        puppetMaster.mode = PuppetMaster.Mode.Active;
    }

  
    

}
