using UnityEngine;

public class RagdollLimbControl : MonoBehaviour
{
    [SerializeField] Transform targetLimb;
    Quaternion startRot;
    ConfigurableJoint myJoint;

    public bool inverse;

    void Start() {
	    myJoint = GetComponent<ConfigurableJoint>();
	    startRot = transform.localRotation;
    }

    void Update() {
	    if(!inverse) myJoint.targetRotation = targetLimb.localRotation * startRot;
	    else myJoint.targetRotation = Quaternion.Inverse(targetLimb.localRotation) * startRot;
    }
}
