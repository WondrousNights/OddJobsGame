
using UnityEngine;

public class Local_AnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;

    Vector3 movement;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void ProcessVisuals(Vector2 moveInput)
    {
       
        movement = new Vector3(moveInput.x, 0f, moveInput.y);

        animator.SetFloat("speed", movement.magnitude);


        /*
        float velocityZ = Vector3.Dot(movement.normalized, forward.gameObject.transform.forward);
        float velocityX = Vector3.Dot(movement.normalized, forward.gameObject.transform.right);

        Debug.Log("Velocity Z :" + velocityZ + "Velocity X : " + velocityX);

        animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);
        */
    }

    
}
