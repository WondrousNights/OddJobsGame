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


 
    private Network_PlayerInputController playerInputController;
    private PlayerManager playerManager;
    private Player_UIManager uiManager;

    Camera cam;


    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        playerInputController = GetComponent<Network_PlayerInputController>();
        playerInputController.onFoot.Interact.performed += ctx => ProcessInteract();

        uiManager = playerManager.playerUIManager;
        cam = playerManager.playerController.cam;

    }
    
    void Update()
    {
        if(!IsOwner) return;
        uiManager.UpdateInteractText(string.Empty);

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, distance, interactionLayerMask))
        {
            if(hitInfo.collider.GetComponent<Network_Interactable>() != null)
            {
                Network_Interactable interactable = hitInfo.collider.GetComponent<Network_Interactable>();
                uiManager.UpdateInteractText(hitInfo.collider.GetComponent<Network_Interactable>().promptMessage);
           
                if(interact)
                {
                    interactable.BaseInteract(playerManager);
                    interact = false;
                }
            }
            else if(interact == true)
            {
                interact = false;
            }
        }

    }

    void ProcessInteract()
    {
        interact = true;
        Invoke("ResetInteract", 0.1f);
    }

    void ResetInteract()
    {
        interact = false;
    }


   
}
