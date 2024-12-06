using TMPro;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Local_PlayerInteractionManager : MonoBehaviour
{

    [SerializeField] TMP_Text interactText;
    [SerializeField]private float distance = 3f;
    [SerializeField] LayerMask interactionLayerMask;

    bool interact;
    bool nextOption = false;
    bool previousOption = false;
    bool pressOption = false;

    Camera cam;
    private Local_PlayerUI playerUI;

    void Start()
    {
        cam = GetComponent<Local_PlayerInputController>().mycam;
        playerUI = GetComponent<Local_PlayerUI>();
    }
    
    void Update()
    {
        playerUI.UpdateText(string.Empty);

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, distance, interactionLayerMask))
        {
            if(hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(hitInfo.collider.GetComponent<Interactable>().promptMessage);


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
                        interactable.BaseInteract();
                        interact = false;
                    } 
                }

            }
        }
    }

    public void ProcessInteract(CallbackContext context)
    {
        
        if(context.performed)
        {
            interact = true;
        }
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
