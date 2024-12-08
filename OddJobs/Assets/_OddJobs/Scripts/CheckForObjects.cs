using UnityEngine;

public class CheckForObjects : MonoBehaviour
{
    private float stepOffset;
    [SerializeField] private CharacterController controller;

    void Start() {
        controller = GetComponentInParent<CharacterController>();
        stepOffset = controller.stepOffset;
    }


    private void OnTriggerStay(Collider other) {
        if (other.GetComponent<Rigidbody>() != null) {
            controller.stepOffset = 0;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<Rigidbody>() != null) {
            controller.stepOffset = stepOffset;
        }
    }
}
