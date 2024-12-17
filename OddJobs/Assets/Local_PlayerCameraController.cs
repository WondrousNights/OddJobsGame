using UnityEngine;

public class Local_PlayerCameraController : MonoBehaviour
{
    private Camera playerCamera;
    private Local_PlayerInputController inputController;

    [SerializeField] Vector3 posOffset;

    GameObject ragdollRoot;

    bool isRagdollCamera;

    void Start()
    {
        inputController = GetComponent<Local_PlayerInputController>();
        playerCamera = inputController.mycam;
        ragdollRoot = inputController.hipPosition;
    }


    public void SetRagdollCamera()
    {
        isRagdollCamera = true;
        playerCamera.cullingMask = inputController.nohudLayerMask;
        
    }

    public void SetNormalCamera()
    {
        isRagdollCamera = false;
        inputController.mycam.transform.localPosition = inputController.firstPersonCamPos.transform.localPosition;
        inputController.mycam.transform.localRotation = inputController.firstPersonCamPos.transform.localRotation;
    }

    void Update()
    {
        if(isRagdollCamera)
        {
            playerCamera.transform.localPosition = ragdollRoot.transform.position + posOffset;
            
        }
    }


}
