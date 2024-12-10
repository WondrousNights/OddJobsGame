using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using Vector3 = UnityEngine.Vector3;

public class Local_PlayerInputController : MonoBehaviour
{
    private Local_PlayerMovement playerMovement;
    private Local_PlayerLook look;
    private PlayerGunHandler gunHandler;
    private Local_PlayerStats playerStats;
    private Local_PlayerInteractionManager playerInteractionManager;
    private Local_PlayerHealthManager playerHealthManager;
    public Local_PlayerUI playerUI;
    
    [SerializeField] WeaponSway weaponSway;

    public Camera mycam;
    public Transform firstPersonCamPos;
    public Transform thirdPersonCamPos;

    [SerializeField] GameObject myVisuals;
    [SerializeField] GameObject gunHolder;
    // [SerializeField] GameObject myCanvas;
    public GameObject hipPosition;

    AudioListener myListener;
    public bool hasSpawned = false;

    // float count = 0;



    public PlayerInput playerInput;
    //Input Values
    UnityEngine.Vector2 moveInput;
    UnityEngine.Vector2 lookInput;

    // bool isInteracting = false;
    bool isShooting = false;

    //Player Mask
    [SerializeField] LayerMask player1Mask;
    [SerializeField]LayerMask player2Mask;
    [SerializeField]LayerMask player3Mask;
    [SerializeField]LayerMask player4Mask;

    public LayerMask nohudLayerMask;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<Local_PlayerMovement>();
        look = GetComponent<Local_PlayerLook>();
        gunHandler = GetComponent<PlayerGunHandler>();
        playerStats = GetComponent<Local_PlayerStats>();
        playerHealthManager = GetComponent<Local_PlayerHealthManager>();
        playerUI = GetComponent<Local_PlayerUI>();


        myListener = GetComponent<AudioListener>();
        //Event Subscription

        //playerStats.OnPlayerDeath += HandlePlayerDeathEvent;
        //playerStats.OnPlayerRevive += HandlePlayerReviveEvent;
 

    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Application.targetFrameRate = 60;
        //Camera Controls
        
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            mainCam.gameObject.SetActive(false);
        }

       
        SetLayers();
    }

    void FixedUpdate()
    {
        if(playerHealthManager.isRagdoll)
        {
            moveInput = new UnityEngine.Vector2(0, 0);
        }
        
    }
    void LateUpdate()
    {
        if(!playerHealthManager.isRagdoll)
        {
            playerMovement.ProcessMove(moveInput);
        }
       
    }
    void Update()
    {
        if(playerHealthManager.isRagdoll)
        {
            lookInput = new UnityEngine.Vector2(0, 0);
        }
        if (!playerStats.isDead || !playerHealthManager.isRagdoll)
        {
            look.ProcessLook(lookInput);
            weaponSway.WeaponSwayAnimation(lookInput);
        }
        

        if(isShooting) 
        {
            gunHandler.ShootCurrentGun();
        }
         
    }

    public void ResetSetCameraLayerMask()
    {
         if(playerInput.playerIndex == 0)
         {
            mycam.cullingMask = player1Mask;
         }
         if(playerInput.playerIndex == 1)
         {
            mycam.cullingMask = player2Mask;
         }
         if(playerInput.playerIndex == 2)
         {
            mycam.cullingMask = player3Mask;
         }
         if(playerInput.playerIndex == 3)
         {
            mycam.cullingMask = player4Mask;
         }
    }


    public void ProcessMove(CallbackContext context)
    {
        if(playerHealthManager.isRagdoll) return;
        
         
        moveInput = context.ReadValue<UnityEngine.Vector2>();
        playerMovement.ProcessAnimations(context.ReadValue<UnityEngine.Vector2>());
    }
    public void ProcessLook(CallbackContext context)
    {
        if(playerHealthManager.isRagdoll) return;
        lookInput = context.ReadValue<UnityEngine.Vector2>();
    }

    public void ProcessJump(CallbackContext context)
    {
        if(playerHealthManager.isRagdoll) return;
        if(context.performed)
        {
            playerMovement.Jump();
        }
        
    }

    public void ProcessShoot(CallbackContext context)
    {
        if(playerHealthManager.isRagdoll) return;
        if(context.performed)
        {
            isShooting = true;
           
        }

        if(context.canceled)
        {
            isShooting = false;
        }
        
    }

    public void ProcessReload(CallbackContext context)
    {
        if(playerHealthManager.isRagdoll) return;
        if(context.performed)
        {
           gunHandler.Reload();
        }
        
    }

    
    public void ProcessRagdoll(CallbackContext context)
    {
        if(playerHealthManager.isRagdoll) return;

       
        if(context.performed)
        {
            playerHealthManager.TakeDamageFromMelee(this.transform.position, 0, 100f, this.transform.position);
        }    
    }

    public void ProcessSwitchGun(CallbackContext context)
    {
        if(playerHealthManager.isRagdoll) return;

        if(context.performed)
        {
            gunHandler.SwitchWeapon();
        }
    }



  
    //Utils

    public void SetLayers()
    {
        //Set Layers so we can disable player visuals accordingly
        //Camera is set to only render other player visuals, not our own
        //Need to use getcomponents in children for this!!!
        if(playerInput.playerIndex == 0)
        {

            foreach (Transform child in myVisuals.transform)
            {
                child.gameObject.layer = 10;

                foreach (Transform nestedChild in child.transform)
                {
                    nestedChild.gameObject.layer = 10;

                    foreach(Transform muzzleFlash in nestedChild.transform)
                    {
                        muzzleFlash.gameObject.layer = 10;
                    }
                }
            }

            
            mycam.cullingMask = player1Mask;
        }
        
        
        if(playerInput.playerIndex == 1)
        {
            foreach (Transform child in myVisuals.transform)
            {
                child.gameObject.layer = 11;

                foreach (Transform nestedChild in child.transform)
                {
                    nestedChild.gameObject.layer = 11;

                    foreach(Transform muzzleFlash in nestedChild.transform)
                    {
                        muzzleFlash.gameObject.layer = 11;
                    }
                }
            }

            
            mycam.cullingMask = player2Mask;
        }
       
    }

 
}
