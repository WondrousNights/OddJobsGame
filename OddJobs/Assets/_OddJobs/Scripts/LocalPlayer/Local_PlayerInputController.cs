using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Local_PlayerInputController : MonoBehaviour
{
    private Local_PlayerMovement playerMovement;
    private Local_PlayerLook look;
    private Local_PlayerGunHandler gunHandler;
    private Local_PlayerStats playerStats;
    private Local_PlayerInteractionManager playerInteractionManager;
    
    [SerializeField] RagdollEnabler ragdollEnabler;
    [SerializeField] WeaponSway weaponSway;

    public Camera mycam;
    [SerializeField] Camera thirdPersonCam;
    [SerializeField] GameObject myVisuals;
    [SerializeField] GameObject myCanvas;
    [SerializeField] Transform hipPosition;

    AudioListener myListener;
    public bool hasSpawned = false;

    float count = 0;



    PlayerInput playerInput;
    //Input Values
    Vector2 moveInput;
    Vector2 lookInput;

    bool isRagdoll = false;
    bool isInteracting = false;
    

    //Player Mask
    [SerializeField] LayerMask player1Mask;
    [SerializeField]LayerMask player2Mask;
    [SerializeField]LayerMask player3Mask;
    [SerializeField]LayerMask player4Mask;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<Local_PlayerMovement>();
        look = GetComponent<Local_PlayerLook>();
        gunHandler = GetComponent<Local_PlayerGunHandler>();
        playerStats = GetComponent<Local_PlayerStats>();


        myListener = GetComponent<AudioListener>();
        //Event Subscription

        playerStats.OnPlayerDeath += HandlePlayerDeathEvent;
        playerStats.OnPlayerRevive += HandlePlayerReviveEvent;
 

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

        //Set Layers so we can disable player visuals accordingly
        //Camera is set to only render other player visuals, not our own
        Debug.Log("My player index is :" + playerInput.playerIndex);
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

            foreach (Transform child in gunHandler.currentGun.transform)
            {
                child.gameObject.layer = 14;
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

            foreach (Transform child in gunHandler.currentGun.transform)
            {
                child.gameObject.layer = 15;
            }
            mycam.cullingMask = player2Mask;
        }
        if(playerInput.playerIndex == 2)
        {
            foreach (Transform child in myVisuals.transform)
            {
                child.gameObject.layer = 12;
            }
            GetComponentInChildren<WeaponSway>().gameObject.layer = 12;
            mycam.cullingMask = player3Mask;
        }
        if(playerInput.playerIndex == 3)
        {
            foreach (Transform child in myVisuals.transform)
            {
                child.gameObject.layer = 13;
            }
            myVisuals.gameObject.layer = 13;
            GetComponentInChildren<WeaponSway>().gameObject.layer = 13;
            mycam.cullingMask = player4Mask;
        }

    }

    void FixedUpdate()
    {
        if(isRagdoll)
        {
            moveInput = new Vector2(0, 0);
        }
        playerMovement.ProcessMove(moveInput);
    }
    void LateUpdate()
    {
        if (!playerStats.isDead || !isRagdoll)
        {
            look.ProcessLook(lookInput);
            weaponSway.WeaponSwayAnimation(lookInput);
        }
    }


    public void ProcessMove(CallbackContext context)
    {
        if(isRagdoll) return;
        
         
        moveInput = context.ReadValue<Vector2>();
        playerMovement.ProcessAnimations(context.ReadValue<Vector2>());
    }
    public void ProcessLook(CallbackContext context)
    {
        if(isRagdoll) return;
        lookInput = context.ReadValue<Vector2>();
    }

    public void ProcessJump(CallbackContext context)
    {
        if(isRagdoll) return;
        if(context.performed)
        {
            playerMovement.Jump();
        }
        
    }

    public void ProcessShoot(CallbackContext context)
    {
        if(isRagdoll) return;
        if(context.performed)
        {
            gunHandler.Shoot();
        }
        
    }

    public void ProcessReload(CallbackContext context)
    {
        if(isRagdoll) return;
        if(context.performed)
        {
            gunHandler.Reload();
        }
        
    }

    public void ProcessRagdoll(CallbackContext context)
    {
        if(isRagdoll) return;

       
        if(context.performed)
        {
             Debug.Log("Ragdoll!!");
            StartCoroutine("ProcessRagdollAnimation", 3f);
        }
    }

    


    IEnumerator ProcessRagdollAnimation(float duration)
    {
    isRagdoll = true;
       ragdollEnabler.EnableRagdoll();
       thirdPersonCam.enabled = true;
       mycam.enabled = false;

       Debug.Log("Ragdoll enabled");
       
       yield return new WaitForSeconds(duration);

     transform.position = hipPosition.position;
       ragdollEnabler.EnableAnimator();
       thirdPersonCam.enabled = false;
       mycam.enabled = true;
       isRagdoll = false;
       

    }


//Old Death System - Need to rework
    private void HandlePlayerDeathEvent(object sender, EventArgs e)
    {
 
            myVisuals.SetActive(false);

            mycam.gameObject.SetActive(false);
            this.gameObject.transform.position = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position;
        

    }

    private void HandlePlayerReviveEvent(object sender, EventArgs e)
    {
        
            myVisuals.SetActive(true);
        

            mycam.gameObject.SetActive(true);
            gunHandler.currentGun.isReloading = false;
        

    }
  

}
