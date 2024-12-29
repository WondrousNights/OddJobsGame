using UnityEngine;

public class ActiveRagdollController : MonoBehaviour
{

    [Header("RAGDOLL Joints")]

    [SerializeField] ConfigurableJoint ragdollSpine1;
    [SerializeField] ConfigurableJoint ragdollSpine2;
    [SerializeField] ConfigurableJoint ragdollChest;
    [SerializeField] ConfigurableJoint ragdollLeftShoulder;
    [SerializeField] ConfigurableJoint ragdollLeftElbow;
    [SerializeField] ConfigurableJoint ragdollLeftHand;

    [SerializeField] ConfigurableJoint ragdollRightShoulder;
    [SerializeField] ConfigurableJoint ragdollRightElbow;
    [SerializeField] ConfigurableJoint ragdollRightHand;
    [SerializeField] ConfigurableJoint ragdollNeck;
    [SerializeField] ConfigurableJoint ragdollHead;
    [SerializeField] ConfigurableJoint ragdollLeftThigh;
    [SerializeField] ConfigurableJoint ragdollLeftShin;
    [SerializeField] ConfigurableJoint ragdollLeftFoot;

    [SerializeField] ConfigurableJoint ragdollRightThigh;
    [SerializeField] ConfigurableJoint ragdollRightShin;
    [SerializeField] ConfigurableJoint ragdollRightFoot;


    [Header("Animator Joints")]
    [SerializeField] Transform animatorSpine1;
    [SerializeField] Transform animatorSpine2;
    [SerializeField] Transform animatorChest;
    [SerializeField] Transform animatorLeftShoulder;
    [SerializeField] Transform animatorLeftElbow;
    [SerializeField] Transform animatorLeftHand;

    [SerializeField] Transform animatorRightShoulder;
    [SerializeField] Transform animatorRightElbow;
    [SerializeField] Transform animatorRightHand;
    [SerializeField] Transform animatorNeck;
    [SerializeField] Transform animatorHead;
    [SerializeField] Transform animatorLeftThigh;
    [SerializeField] Transform animatorLeftShin;
    [SerializeField] Transform animatorLeftFoot;

    [SerializeField] Transform animatorRightThigh;
    [SerializeField] Transform animatorRightShin;
    [SerializeField] Transform animatorRightFoot;


    void Update()
    {
        CopyMotion();
        
    }
    


    void CopyMotion()
    {
        ragdollSpine1.targetRotation = animatorSpine1.rotation;
        ragdollSpine2.targetRotation = animatorSpine2.rotation;
        ragdollChest.targetRotation = animatorChest.rotation;
        ragdollLeftShoulder.targetRotation = animatorLeftShoulder.rotation;
        ragdollLeftElbow.targetRotation = animatorLeftElbow.rotation;
        ragdollLeftHand.targetRotation = animatorLeftHand.rotation;
        ragdollRightShoulder.targetRotation = animatorRightShoulder.rotation;
        ragdollRightElbow.targetRotation = animatorRightElbow.rotation;
        ragdollRightHand.targetRotation = animatorRightHand.rotation;
        ragdollLeftThigh.targetRotation = animatorLeftThigh.rotation;
        ragdollLeftShin.targetRotation = animatorLeftShin.rotation;
        ragdollLeftFoot.targetRotation = animatorLeftFoot.rotation;
        ragdollRightThigh.targetRotation = animatorRightThigh.rotation;
        ragdollRightShin.targetRotation = animatorRightShin.rotation;
        ragdollRightFoot.targetRotation = animatorRightFoot.rotation;

    }
}
