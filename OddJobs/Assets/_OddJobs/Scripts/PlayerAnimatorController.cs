using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{

    [SerializeField] Animator animator;
    public void SetWalkForword()
    {
        animator.SetBool("walkForward", true);
        animator.SetBool("walkBackward", false);
        animator.SetBool("walkLeft", false);
        animator.SetBool("walkRight", false);
    }

    public void SetWalkBackward()
    {
        animator.SetBool("walkBackward", true);
        animator.SetBool("walkForward", false);
        animator.SetBool("walkLeft", false);
        animator.SetBool("walkRight", false);
    }

    public void SetWalkLeft()
    {
        animator.SetBool("walkLeft", true);
        animator.SetBool("walkForward", false);
        animator.SetBool("walkBackward", false);
        animator.SetBool("walkRight", false);
    }  

    public void SetWalkRight()
    {
        animator.SetBool("walkRight", true);
        animator.SetBool("walkForward", false);
        animator.SetBool("walkLeft", false);
        animator.SetBool("walkBackward", false);
    }


    public void Reset()
    {
        animator.SetBool("walkRight", false);
        animator.SetBool("walkForward", false);
        animator.SetBool("walkLeft", false);
        animator.SetBool("walkBackward", false);
    }


}
