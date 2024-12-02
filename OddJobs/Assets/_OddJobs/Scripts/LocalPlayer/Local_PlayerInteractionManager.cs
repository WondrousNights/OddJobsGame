using TMPro;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Local_PlayerInteractionManager : MonoBehaviour
{

    [SerializeField] TMP_Text interactText;
    [SerializeField]private float distance = 3f;
    [SerializeField] LayerMask interactionLayerMask;

    bool interact;

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

                if(interact == true)
                {
                    interactable.BaseInteract();
                    interact = false;
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
}
