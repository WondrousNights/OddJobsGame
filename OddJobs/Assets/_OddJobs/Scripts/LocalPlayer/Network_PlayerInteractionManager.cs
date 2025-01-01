using TMPro;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Network_PlayerInteractionManager : NetworkBehaviour
{

    [SerializeField] TMP_Text interactText;
    [SerializeField]private float distance = 3f;
    [SerializeField] LayerMask interactionLayerMask;

    bool interact;
    bool nextOption = false;
    bool previousOption = false;
    bool pressOption = false;

    Camera cam;
    private Network_PlayerUI playerUI;

    public Network_PlayerInputController playerInputController;

    public PlayerAmmoHandler ammoHandler;
    public Network_PlayerGunHandler gunHandler;

    public Transform grabPoint;

    void Start()
    {
        playerInputController = GetComponent<Network_PlayerInputController>();
        cam = GetComponent<Network_PlayerInputController>().mycam;
        playerUI = GetComponent<Network_PlayerUI>();

        ammoHandler = GetComponent<PlayerAmmoHandler>();
        gunHandler = GetComponent<Network_PlayerGunHandler>();
    }
    
    void Update()
    {
        if(!IsOwner) return;
        playerUI.UpdateText(string.Empty);

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, distance, interactionLayerMask))
        {
            if(hitInfo.collider.GetComponent<Network_Interactable>() != null)
            {
                Network_Interactable interactable = hitInfo.collider.GetComponent<Network_Interactable>();
                playerUI.UpdateText(hitInfo.collider.GetComponent<Network_Interactable>().promptMessage);


                //UI Interactions
                if(interactable.isUI)
                {
                    NPCUI_Interactable uiInteractable = interactable.GetComponent<NPCUI_Interactable>();
                    if(nextOption == true)
                    {
                        uiInteractable.SelectNextOption();
                        nextOption = false;
                    }
                    if(previousOption == true)
                    {
                        uiInteractable.SelectPreviousOption();
                        previousOption = false;
                    }
                    if(pressOption == true)
                    {
                        uiInteractable.PressSelectedOption();
                        pressOption = false;
                    }
                }
                else
                {
                    if(interact == true)
                    {   
                        interactable.BaseInteract(this);
                        interact = false;
                    } 
                }

            }
            else if(interact == true)
            {
                interact = false;
            }
        }
        else if(interact == true)
        {
            interact = false;
        }

    }

    public void ProcessInteract()
    {
        interact = true;
    }

    //UI Interactions

    public void ProcessNextOption(CallbackContext context)
    {
        if(context.performed)
        {
            nextOption = true;
        }
    }

    public void ProcessPreviousOption(CallbackContext context)
    {
        if(context.performed)
        {
            previousOption = true;
        }
    }

    public void ProcessPressOption(CallbackContext context)
    {
        if(context.performed)
        {
            pressOption = true;
        }
    }
}
